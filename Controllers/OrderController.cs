using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using server.Dto;
using server.Dto.Order;
using server.Entities;
using server.Interface.Repository;
using server.Interface.Service;
using server.Service;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService orderService;
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IEmailService _emailService;


        public OrderController(
            IOrderService orderService,
            IPaymentService paymentService,
            IMapper mapper, IUserRepository userRepository,
            IEmailService config
        )
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.mapper = mapper;
            this.userRepository = userRepository;
            _emailService = config;


        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ResponseDto>> CreateOrder([FromBody] CreateOrderDTO order)
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            var res = new ResponseDto();

            Order createdOrder = await orderService.CreateOrderAsync(userId, order.CartId, order.ShipToAddress);

            PaymentDetails details = await paymentService.InitializePayment(userId, createdOrder.Id, createdOrder.TotalPriceAfterDiscount);
            return Ok(res.success("Order Created Successfully", details));
        }

        [HttpGet("Get-all-orders")]
        public async Task<ActionResult<ResponseDto>> GetAllOrders()
        {
            
            var orders = await orderService.GetAllUserOrders();
            var orderDto = mapper.Map<List<GetUserOrdersDTO>>(orders);

            return Ok(orderDto);
        }
        [HttpGet("user-orders/{userId}")]
        public async Task<ActionResult<ResponseDto>> GetAllUserOrders(int userId)
        {
           
            var orders = await orderService.GetOrdersAsync(userId);
            var orderDto = mapper.Map<List<GetUserOrdersDTO>>(orders);

            return Ok(orderDto);
        }

        [HttpGet("orderdetail/{orderId}")]
        public async Task<ActionResult> GetOrderDetail(int orderId)
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            OrderDetailDTO details = await orderService.GetOrderDetailAsync(orderId,userId);
            var orderDto = mapper.Map<OrderDto>(details.order);

            return Ok(new {order=orderDto,paymentDetails=details.paymentDetails,details.shippingAddress});
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusDto dto)
        {
            try
            {
                var result = await orderService.UpdateOrderStatusAsync(dto.OrderId, dto.NewStatus);

                if (result)
                {
                    var user = await userRepository.GetUserByIdAsync(dto.userId); // Get user by ID

                    if (user != null)
                    {
                        await _emailService.SendEmailAsync(
                            user.Email,
                            "Order Status Update",
                            $"Your order #{dto.OrderId} status has been updated to {dto.NewStatus}."
                        );
                    }
                    return Ok(new { message = "Order status updated and email sent successfully." });
                }
                return BadRequest(new { message = "Order status update failed." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}