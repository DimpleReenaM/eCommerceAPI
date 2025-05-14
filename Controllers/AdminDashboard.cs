using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto.DashboardDto;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboard : ControllerBase
    {
        private readonly DataContex _context;

        public AdminDashboard(DataContex context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var totalRevenue = await _context.Orders.SumAsync(o => o.TotalPrice);
            var totalOrders = await _context.Orders.CountAsync();
            var activeUsers = await _context.users.Where(u => u.Role == "Seller").CountAsync();
            var newSellers = await _context.users
              .Where(u => u.Role == "Seller" && u.CreatedDate >= DateTime.UtcNow.AddDays(-30))
              .CountAsync();


            var recentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new RecentOrderDto
                {
                    Id = o.Id,
                    Customer = o.User.UserName,
                    Date = o.OrderDate.ToString("MMM dd, yyyy"),
                    Amount = o.TotalPrice,
                    Status = o.Status
                })
                .ToListAsync();

            var recentSellers = await _context.users
                .Where(u => u.Role == "Seller")
                .OrderByDescending(u => u.CreatedDate)
                .Take(5)
                .Select(u => new RecentSellerDto
                {
                    Name = u.UserName,
                    StoreName = u.BusinessName// You can change this if you have a field
                })
                .ToListAsync();



            var dashboardDto = new DashboardDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                ActiveUsers = activeUsers,
                NewSellers = newSellers,
                RecentOrders = recentOrders,
                RecentSellers = recentSellers
            };

            return Ok(dashboardDto);
        }
    }
}
