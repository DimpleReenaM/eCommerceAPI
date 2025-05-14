using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Entities;
using server.Interface.Repository;
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
                .Include(p=>p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.Thumbnail)
                .AsQueryable();

            if (!string.IsNullOrEmpty(inData.Search))
            {
                productQuery= productQuery.Where(p=>p.Name.Contains(inData.Search));
            }

            if (inData.MinPrice.HasValue) 
            {
                productQuery = productQuery.Where(p => p.OriginalPrice >= inData.MinPrice);
            }
            if (inData.MaxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.OriginalPrice <= inData.MaxPrice);
            }
            if (inData.InStock.HasValue)
            {
                if (inData.InStock==true)
                {
                    productQuery = productQuery.Where(p => p.StockQuantity>0);
                }
                else
                {
                    productQuery = productQuery.Where(p => p.StockQuantity == 0);
                }
                
            }
            if (inData.CategoryIds!=null && inData.CategoryIds.Length>0)
            {
                productQuery = productQuery.Where(p => inData.CategoryIds.Contains(p.CategoryId) );
            }
            if (inData.BrandIds!=null && inData.BrandIds.Length>0)
            {
                productQuery = productQuery.Where(p => inData.BrandIds.Contains(p.BrandId));
            }

            if (!string.IsNullOrEmpty(inData.Sort))
            {
                if (inData.Sort.ToLower() == "price_htl")
                {
                   productQuery = productQuery.OrderByDescending(p=>p.OriginalPrice);
                }
                if (inData.Sort.ToLower() == "price_lth")
                {
                    productQuery = productQuery.OrderBy(p => p.OriginalPrice);
                }
                if (inData.Sort.ToLower() == "featured")
                {
                    productQuery = productQuery.OrderByDescending(p => p.IsFeatured);                    
                }
                if (inData.Sort.ToLower() == "rating")
                {
                    productQuery = productQuery.OrderBy(p => p.AverageRating);
                }
                if (inData.Sort.ToLower() == "newest")
                {
                    productQuery = productQuery.OrderByDescending(p => p.CreatedDate);
                }
            }


            return new ProductPagination()
            {
                PageIndex = inData.PageIndex,
                PageSize = inData.PageSize,               
                Data = await productQuery
                            .Skip((inData.PageIndex - 1) * inData.PageSize)
                            .Take(inData.PageSize)
                            .ToListAsync(),
                Count = await contex.products.CountAsync(),
                MaxPrice = await contex.products.MaxAsync(p => p.OriginalPrice),
                MinPrice = await contex.products.MinAsync(p => p.OriginalPrice),
            };


            //return await productQuery
            //            .Skip((inData.pageIndex-1)* inData.pageSize)
            //            .Take(inData.pageSize)
            //            .ToListAsync();
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
