using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Entities;
using server.Interface.Repository;
using System;
using System.Linq;

namespace server.Repository
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        private readonly DataContex contex;

        public ProductRepository(DataContex contex):base(contex) 
        {
            this.contex = contex;
        }

        public async Task<ProductPagination> GetAllIncludingChlidEntities(CatalogSpec inData)
        {
            IQueryable<Product> productQuery = contex.products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Thumbnail)
                .AsQueryable();

            // Filter by search
            if (!string.IsNullOrEmpty(inData.Search))
            {
                productQuery = productQuery.Where(p => p.Name.Contains(inData.Search));
            }

            // Filter by price
            if (inData.MinPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.OriginalPrice >= inData.MinPrice);
            }
            if (inData.MaxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.OriginalPrice <= inData.MaxPrice);
            }

            // Filter by stock
            if (inData.InStock.HasValue)
            {
                if (inData.InStock == true)
                {
                    productQuery = productQuery.Where(p => p.StockQuantity > 0);
                }
                else
                {
                    productQuery = productQuery.Where(p => p.StockQuantity == 0);
                }
            }

            // Filter by categories
            if (inData.CategoryIds != null && inData.CategoryIds.Any())
            {
                productQuery = productQuery.Where(p => inData.CategoryIds.Contains(p.CategoryId));
            }

            // Filter by brands
            if (inData.BrandIds != null && inData.BrandIds.Any())
            {
                productQuery = productQuery.Where(p => inData.BrandIds.Contains(p.BrandId));
            }

            

            // Apply sorting
            switch (inData.Sort?.ToLower())
            {
                case "price_htl":
                    productQuery = productQuery.OrderByDescending(p => p.OriginalPrice);
                    break;
                case "price_lth":
                    productQuery = productQuery.OrderBy(p => p.OriginalPrice);
                    break;
                case "featured":
                    productQuery = productQuery.OrderByDescending(p => p.IsFeatured);
                    break;
                case "newest":
                    productQuery = productQuery.OrderByDescending(p => p.CreatedDate);
                    break;
                default:
                    productQuery = productQuery.OrderBy(p => p.Id);
                    break;
            }

            // Apply pagination
            var filteredProducts = await productQuery
                .Skip((inData.PageIndex - 1) * inData.PageSize)
                .Take(inData.PageSize)
                .ToListAsync();

            // Only get stats for the filtered set
            var totalFilteredCount = await productQuery.CountAsync();
            var minFilteredPrice = await productQuery.MinAsync(p => p.OriginalPrice);
            var maxFilteredPrice = await productQuery.MaxAsync(p => p.OriginalPrice);

            return new ProductPagination()
            {
                PageIndex = inData.PageIndex,
                PageSize = inData.PageSize,
                Data = filteredProducts,
                Count = totalFilteredCount,
                MinPrice = minFilteredPrice,
                MaxPrice = maxFilteredPrice
            };
        }

        public async Task<Product?> GetProductByIdIncludingChlidEntities(int productID)
        {
            return await contex.products
                .Include(p=>p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.Thumbnail)
                .Where(p=>p.Id == productID)
                .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetSellerProducts1(int sellerId)
        {
            return await contex.products.Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Thumbnail)
                .Where(p => p.CreatedBy == sellerId)
                .ToListAsync();
        }

    }
}
