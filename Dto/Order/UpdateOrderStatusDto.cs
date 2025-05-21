using server.Utils.Enum;

namespace server.Dto.Order
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }

        public int userId { get; set; }
    }
}
