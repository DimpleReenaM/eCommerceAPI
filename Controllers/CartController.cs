using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Entities;
using server.Interface.Service;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN,USER")]

    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;

        public CartController(ICartService cartService,IMapper mapper)
        {
            this.cartService = cartService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetUserCart()
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            ResponseDto responseDto = new ResponseDto();
            ShoppingCart cart = await cartService.FindUserCart(userId);

            if(cart == null){
                return NotFound();
            }
            ShoppingCartResDto cartResData = mapper.Map<ShoppingCartResDto>(cart);
            return Ok(responseDto.success("Successfull", cartResData));
        }
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> AddItemToCart([FromBody] AddItemToCartRequest item)
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            ResponseDto responseDto = new ResponseDto();
            await cartService.AddItemToCart(userId,item.ProductId,item.Quantity);
            return Ok(responseDto.success("SuccessFully Added To Cart"));
        }
        
    }
}
