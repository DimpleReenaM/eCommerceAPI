using server.Entities;

namespace server.Interface.Repository
{
    public interface IOrderItemRepository:IGenericRepository<OrderItem>
    {
        Task<List<OrderItem>> GetAllOrderItemByOrderId(int orderId);
    }
}