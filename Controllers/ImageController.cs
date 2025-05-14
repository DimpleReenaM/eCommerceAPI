using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Entities;
using server.Interface.Service;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public ImageController(IImageService imageService,IMapper mapper)
        {
            this.imageService = imageService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ImageDtoRes>> Upload(IFormFile file)
        {
            Image image = await imageService.SaveImageAsync(file);
            
            return Ok(mapper.Map<ImageDtoRes>(image));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Image>> Delete(int Id)
        {
            await imageService.DeleteImageAsync(Id);
            return Ok();
        }
    }
}
