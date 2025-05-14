using server.Dto;
using server.Entities;

namespace server.Interface.Service
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId,int cartId,AddressDto address);
        Task<IEnumerable<Order>> GetOrdersAsync(int userId);
        Task<OrderDetailDTO> GetOrderDetailAsync(int orderId,int userId);
    }
}