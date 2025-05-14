namespace server.Dto.DashboardDto
{
    public class DashboardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int ActiveUsers { get; set; }
        public int NewSellers { get; set; }

        public List<RecentOrderDto> RecentOrders { get; set; }
        public List<RecentSellerDto> RecentSellers { get; set; }
    }
}
