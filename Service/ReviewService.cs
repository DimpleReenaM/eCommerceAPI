using server.Dto;
using server.Entities;
using server.Interface.Repository;
using server.Interface.Service;

namespace server.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IProductReviewRepository repository;
        private readonly IProductRepository productRepository;

        public ReviewService(IProductReviewRepository repository,IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this.repository = repository;
        }
        public async Task<ProductReview> CreateReview(ReviewDTO review, int userId)
        {
            var productExists = await productRepository.GetByIdAsync(review.ProductId);

            if (productExists is null)
            {
                throw new Exception("Product not found.");
            }
            ProductReview review1 = new ProductReview()
            {
                ProductId = review.ProductId,
                UserId = userId,
                Review = review.Review,
                Rating = review.Rating,
            };
            ProductReview savedReview = await repository.AddAsync(review1);
            productExists.AverageRating = await repository.GetAverageRating(review.ProductId);
            await productRepository.UpdateAsync(productExists);
            return savedReview;
        }

        public async Task<IEnumerable<ProductReview>> GetReviews(int productId)
        {
            return await repository.GetAllReviewsByProductIdAsync(productId);
        }
    }
}