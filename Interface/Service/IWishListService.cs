using server.Dto;
using server.Entities;

namespace server.Interface.Service
{
    public interface IWishListService
    {
        Task<Wishlist?> GetWishlistIncludeProductAsync(int userId);

        Task AddToWishlistAsync(int userId,int productId);
        Task RemoveFromWishlistAsync(int userId,int productId);
    }
}
