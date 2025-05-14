using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class WishListItemRepository : GenericRepository<WishlistItem>, IWishListItemRepository
    {
        private readonly DataContex contex;

        public WishListItemRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }
    }
}
