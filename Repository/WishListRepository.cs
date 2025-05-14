using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class WishListRepository : GenericRepository<Wishlist>, IWishListRepository
    {
        private readonly DataContex context;

        public WishListRepository(DataContex context) : base(context)
        {
            this.context = context;
        }

        public async Task<Wishlist?> GetWishlistByUserIdAsync(int userId)
        {
            return await context.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<Wishlist?> GetWishlistByUserIdIncludeProductAsync(int userId)
        {
            return await context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Product)
                .ThenInclude(p=>p.Thumbnail)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
