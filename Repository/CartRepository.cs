using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class CartRepository : GenericRepository<ShoppingCart>, ICartRepository
    {
        private readonly DataContex contex;

        public CartRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }

        public async Task<ShoppingCart?> FindCartByUserId(int userId)
        {
            return await contex.Shopcarts
                        .Where(x => x.UserId == userId)
                        .FirstOrDefaultAsync();
        }
    }
}