using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class ImageRepository:GenericRepository<Image>, IImageRepository
    {
        private readonly DataContex contex;

        public ImageRepository(DataContex contex):base(contex)
        {
            this.contex = contex;
        }
    }
}
