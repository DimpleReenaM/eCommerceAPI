using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class ProductReviewRepository : GenericRepository<ProductReview>, IProductReviewRepository
    {
        private readonly DataContex contex;
        public ProductReviewRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }

        public async Task<IEnumerable<ProductReview>> GetAllReviewsByProductIdAsync(int productId)
        {
            return await contex.ProductReviews.Where(x=>x.ProductId == productId).ToListAsync();
        }

        public async Task<double> GetAverageRating(int productId)
        {
            return await contex.ProductReviews.Where(x=>x.ProductId==productId).AverageAsync(r=>r.Rating);
        }
    }
}