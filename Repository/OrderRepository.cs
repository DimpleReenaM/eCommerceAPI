
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContex contex;
        public OrderRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }

        public async Task<IEnumerable<Order>> GetAllAsyncByUserId(int userId)
        {
            return await contex.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        }
    }
}