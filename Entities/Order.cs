
namespace server.Entities
{
    public class Order : AuditBaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // e.g., Pending, Completed, Cancelled
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}