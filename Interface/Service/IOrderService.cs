using server.Dto;
using server.Entities;
using server.Utils.Enum;

namespace server.Interface.Service
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId,int cartId,AddressDto address);
        Task<IEnumerable<Order>> GetOrdersAsync(int userId);
        Task<OrderDetailDTO> GetOrderDetailAsync(int orderId,int userId);

        Task<IEnumerable<Order>> GetAllUserOrders();
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);

    }
}