using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class BrandRepository:GenericRepository<Brand>,IBrandRepository
    {
        private readonly DataContex contex;

        public BrandRepository(DataContex contex):base(contex)
        {
            this.contex = contex;
        }

        public async Task<IEnumerable<Brand>> GetAllIncludingImage()
        {
            return await contex.brands
                .Include(b=>b.Image)
                .ToListAsync();
        }
    }
}
