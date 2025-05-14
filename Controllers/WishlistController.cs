using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Entities;
using server.Interface.Repository;
using server.Interface.Service;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN,USER")]
    public class WishlistController : ControllerBase
    {

        private readonly IWishListService wishListService;
        private readonly IMapper mapper;

        public WishlistController(IWishListService wishListService,IMapper mapper)
        {
            this.wishListService = wishListService;
            this.mapper = mapper;
        }

        // GET: api/Wishlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishListItemResDto>>> GetWishlist()
        {
            //string? email = User?.Identity?.Name;
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }

            var wishlist = await wishListService.GetWishlistIncludeProductAsync(userId);

            if (wishlist == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ICollection<WishListItemResDto>>(wishlist.WishlistItems));
        }

        // POST: api/Wishlist/Add
        [HttpGet("Add/{ProductId}")]
        public async Task<ActionResult<ResponseDto>> AddToWishlist(int ProductId)
        {
            if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }

            ResponseDto res = new ResponseDto();

            await wishListService.AddToWishlistAsync(userId,ProductId);

            return Ok(res.success("Product added to wishlist."));
        }

        // DELETE: api/Wishlist/Remove/{userId}/{productId}
        [HttpDelete("Remove/{productId}")]
        public async Task<ActionResult<ResponseDto>> RemoveFromWishlist(int productId)
        {
             if (!Int32.TryParse(User.FindFirst("UserId")?.Value, out int userId))
            {
                return Unauthorized();
            }
            ResponseDto res = new ResponseDto();

            await wishListService.RemoveFromWishlistAsync(userId, productId);

            return Ok(res.success("Product removed from wishlist."));
        }
    }
}
