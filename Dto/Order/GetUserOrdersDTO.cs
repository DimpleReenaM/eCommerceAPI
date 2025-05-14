
namespace server.Dto
{
    public class GetUserOrdersDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public string Status { get; set; } // e.g., Pending, Completed, Cancelled

    }
}