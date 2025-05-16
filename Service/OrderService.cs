using AutoMapper;
using server.Dto;
using server.Dto.Order;
using server.Entities;
using server.Interface.Repository;
using server.Interface.Service;
using server.Utils.Enum;

namespace server.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentDetailRepository _paymentRepository;
        private readonly IShippingAddressRepository _shippingAddressRepository;

        public OrderService(
            IMapper mapper,
            ICartService cartService,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IPaymentDetailRepository paymentDetailRepository,
            IShippingAddressRepository shippingAddressRepository
        ){
            this._mapper = mapper;
            this._cartService = cartService;
            this._orderItemRepository = orderItemRepository;
            this._orderRepository = orderRepository;
            this._paymentRepository = paymentDetailRepository;
            this._shippingAddressRepository = shippingAddressRepository;
        }
        public async Task<Order> CreateOrderAsync(int userId, int cartId, AddressDto address)
        {
           ShoppingCart? shoppingCart= await _cartService.FindUserCart(userId) ?? throw new Exception("no cart found for user");
           Order order = new Order(){
                 UserId=userId,
                 OrderDate=DateTime.Now,
                 TotalPriceAfterDiscount=shoppingCart.TotalPriceAfterDiscount,
                 TotalDiscount=shoppingCart.TotalDiscount,
                 TotalPrice=shoppingCart.TotalPrice,
                 Status=OrderStatus.Placed.ToString(),
           };

           await _orderRepository.AddAsync(order);

           ShippingAddress shippingAddress=_mapper.Map<ShippingAddress>(address);
           shippingAddress.OrderId=order.Id;

           await _shippingAddressRepository.AddAsync(shippingAddress);

           foreach (var item in shoppingCart.ShoppingCartItems)
           {
              var orderItem=new OrderItem(){
                     OrderId=order.Id,
                     ProductId=item.ProductId,
                     Quantity=item.Quantity,
                     TotalPriceAfterDiscount=item.TotalPriceAfterDiscount,
                     TotalDiscount=item.TotalDiscount,
                     TotalPrice=item.TotalPrice
                };
              await _orderItemRepository.AddAsync(orderItem);
           }
           
           await _cartService.DeleteCart(cartId);
           return order;

          
        }

        public async Task<OrderDetailDTO> GetOrderDetailAsync(int orderId, int userId)
        {
            Order order= await _orderRepository.GetByIdAsync(orderId)?? throw new Exception("Invalid Order ID");
            if(order.UserId!=userId) throw new Exception("Order do not belongs to you");
            List<OrderItem> orderItems = await _orderItemRepository.GetAllOrderItemByOrderId(orderId);
            order.OrderItems = orderItems;
            ShippingAddress shippingAddress=await _shippingAddressRepository.GetShippingAddressByOrderId(orderId)??throw new Exception("No Shipping Address found for Order ID");
            PaymentDetails paymentDetails = await _paymentRepository.GetPaymentDetailsByOrderId(orderId) ?? throw new Exception("No payment found for Order ID");
            return new OrderDetailDTO(){
                order=_mapper.Map<OrderDto>(order),
                paymentDetails=paymentDetails,
                shippingAddress=shippingAddress
            };
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(int userId)
        {
            return await _orderRepository.GetAllAsyncByUserId(userId);
        }

        public async Task<IEnumerable<Order>> GetAllUserOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Status = newStatus.ToString();
            await _orderRepository.UpdateAsync(order);
            return true;
        }
    }
}