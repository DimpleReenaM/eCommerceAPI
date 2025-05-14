using server.Dto;
using server.Entities;

namespace server.Interface.Repository
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<ProductPagination> GetAllIncludingChlidEntities(CatalogSpec inData);
        Task<Product?> GetProductByIdIncludingChlidEntities(int productID);
        Task<IEnumerable<Product>> GetSellerProducts1(int sellerId);

    }
}
