namespace server.Entities
{
    public class Product: AuditBaseEntity
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }

        public decimal? DiscountPercentage { get; set; } // Discount in percentage (e.g., 10% = 0.10)
        public decimal? DiscountAmount { get; set; } // Discount in fixed amount (e.g., $10)
        public decimal NewPrice
        {
            get
            {
                // Apply percentage discount if available
                if (DiscountPercentage.HasValue && DiscountPercentage.Value > 0)
                {
                    return OriginalPrice - (OriginalPrice * DiscountPercentage.Value / 100);
                }

                // Apply amount discount if available
                if (DiscountAmount.HasValue && DiscountAmount.Value > 0)
                {
                    return OriginalPrice - DiscountAmount.Value;
                }

                // If no discount, return the original price
                return OriginalPrice;
            }
        }

        public bool IsOnDiscount
        {
            get
            {
                return DiscountPercentage.HasValue && DiscountPercentage.Value > 0 ||
                       DiscountAmount.HasValue && DiscountAmount.Value > 0;
            }
        }


        public int StockQuantity { get; set; }
        public bool InStock
        {
            get { return StockQuantity > 0 ? true : false; }
        }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }

        public bool IsFeatured { get; set; } = false;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<ProductReview> ProductReviews { get; set; }

        public int? ThumbnailId { get; set; }
        public Image? Thumbnail { get; set; }
    }
}
