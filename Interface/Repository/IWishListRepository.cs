using server.Entities;

namespace server.Interface.Repository
{
    public interface IWishListRepository:IGenericRepository<Wishlist>
    {
        Task<Wishlist?> GetWishlistByUserIdIncludeProductAsync(int userId);

        Task<Wishlist?> GetWishlistByUserIdAsync(int userId);
    }
}
