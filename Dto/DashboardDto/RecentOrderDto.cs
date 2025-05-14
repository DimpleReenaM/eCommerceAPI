namespace server.Dto.DashboardDto
{
    public class RecentOrderDto
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}