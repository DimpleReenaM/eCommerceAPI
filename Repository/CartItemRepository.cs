using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class CartItemRepository : GenericRepository<ShoppingCartItem>, ICartItemRepository
    {
        private readonly DataContex contex;
        public CartItemRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }

        public async Task<List<ShoppingCartItem>> GetAllByCartId(int cartId)
        {
            return await contex.ShopcartItems
                        .Include(x => x.Product)
                        .ThenInclude(x => x.Thumbnail)
                        .Where(x => x.ShoppingCartId == cartId)
                        .ToListAsync();
        }
    }
}