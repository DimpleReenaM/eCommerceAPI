using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interface.Service;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,USER")]

    public class CartItemController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartItemController(ICartService cartService)
        {
            this.cartService = cartService;
            
        }
        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult<ResponseDto>> DeleteCartItem(int cartItemId)
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            ResponseDto responseDto = new ResponseDto();
            await cartService.RemoveCartItem(userId,cartItemId);
            return Ok(responseDto.success("Item removed successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> UpdateCartItem([FromBody] UpdateCartItemRequest item)
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            ResponseDto responseDto = new ResponseDto();
            await cartService.UpdateCartItem(userId,item.CartItemId,item.Quantity);
            return Ok(responseDto);
        }
    }
}