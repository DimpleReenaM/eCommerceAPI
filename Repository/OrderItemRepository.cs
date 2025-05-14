using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly DataContex contex;

        public OrderItemRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }

        public async Task<List<OrderItem>> GetAllOrderItemByOrderId(int orderId)
        {
            return await contex.OrderItems
            .Include(o=>o.Product)
            .ThenInclude(p=>p.Thumbnail)
            .Where(o=>o.OrderId == orderId)
            .ToListAsync();
        }
    }
}