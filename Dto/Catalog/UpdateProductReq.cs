namespace server.Dto.Catalog
{
    public class UpdateProductReq
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public int StockQuantity { get; set; }
        public decimal? DiscountPercentage { get; set; } // Discount in percentage (e.g., 10% = 0.10)

        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public bool isActive { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

    }
}
