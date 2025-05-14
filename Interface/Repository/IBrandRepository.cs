using server.Entities;

namespace server.Interface.Repository
{
    public interface IBrandRepository:IGenericRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllIncludingImage();
    }
}
