using server.Entities;

namespace server.Interface.Service
{
    public interface IImageService
    {
        Task<Image> SaveImageAsync(IFormFile file);
        Task DeleteImageAsync(int Id);
    }
}
