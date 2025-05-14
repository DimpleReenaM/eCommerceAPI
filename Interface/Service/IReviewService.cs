using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dto;
using server.Entities;

namespace server.Interface.Service
{
    public interface IReviewService
    {
        Task<ProductReview> CreateReview(ReviewDTO review,int userId);
        Task<IEnumerable<ProductReview>> GetReviews(int productId);

    }
}