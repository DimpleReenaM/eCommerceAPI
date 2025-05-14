using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class CategoryRepository:GenericRepository<Category>,ICategoryRepository
    {
        private readonly DataContex contex;

        public CategoryRepository(DataContex contex):base(contex)
        {
            this.contex = contex;
        }

        public async Task<IEnumerable<Category>> GetAllIncludingImage()
        {
            return await contex.categories
                .Include(b => b.Image)
                .ToListAsync();
        }
    }
}
