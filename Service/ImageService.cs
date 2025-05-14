using server.Entities;
using server.Interface.Repository;
using server.Interface.Service;

namespace server.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment environment;
        private readonly IImageRepository imageRepository;

        public ImageService(IWebHostEnvironment environment,IImageRepository imageRepository)
        {
            this.environment = environment;
            this.imageRepository = imageRepository;
        }
        public async Task DeleteImageAsync(int Id)
        {
            Image? image = await imageRepository.GetByIdAsync(Id);
            if (image == null) throw new ArgumentNullException($"No Image fount with id {Id}");
            var contentPath = environment.ContentRootPath;
            var imagePath = Path.Combine(contentPath, "Uploads");
            var fileNameWithPath = Path.Combine(imagePath, image.ImageUrl);

            if (Directory.Exists(fileNameWithPath))
            {
                Directory.Delete(fileNameWithPath);
            }
            await imageRepository.DeleteAsync(image);
        }

        public async Task<Image> SaveImageAsync(IFormFile file)
        {
            if(file == null) throw new ArgumentNullException("File is empty");

            var contentPath = environment.ContentRootPath;
            var imagePath = Path.Combine(contentPath, "Uploads");

            if (!Directory.Exists(imagePath)) 
            {
                Directory.CreateDirectory(imagePath);
            }

            var imageName = file.FileName;
            var imageExt = Path.GetExtension(imageName);
            var fileName = $"{Guid.NewGuid().ToString()}{imageExt}";

            var fileNameWithPath = Path.Combine(imagePath, fileName);

            using var fileStream = new FileStream(fileNameWithPath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            Image image = new Image()
            {
                ImageUrl = fileName,
                ImageName = imageName,
                ImageNameExt = imageExt
            };

             return await imageRepository.AddAsync(image);
        }
    }
}
