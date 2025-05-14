
using server.Entities;

namespace server.Interface.Repository
{
    public interface IProductReviewRepository:IGenericRepository<ProductReview>
    {
        Task<IEnumerable<ProductReview>> GetAllReviewsByProductIdAsync(int productId);
        Task<double> GetAverageRating(int productId);

    }
}