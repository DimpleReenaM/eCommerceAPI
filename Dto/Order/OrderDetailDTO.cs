
using server.Dto.Order;
using server.Entities;

namespace server.Dto
{
    public class OrderDetailDTO
    {
        public OrderDto order { get; set; }
        public PaymentDetails paymentDetails{ get; set; }
        public ShippingAddress shippingAddress{ get; set; }
    }
}