using totten_romatoes.Server.Data;
using totten_romatoes.Shared;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IImageService
    {
        public Task<ImageModel> SaveImageToDb(ImageModel imageModel);
    }
    public class ImageService : IImageService
    {
        private ApplicationDbContext _dbContext { get; set; }

        public ImageService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ImageModel> SaveImageToDb(ImageModel imageModel)
        {
            if (imageModel.ImageType != Constants.IMAGE_FORMAT || imageModel.ImageData.Length > Constants.MAX_IMAGE_SIZE)
                throw new Exception("Image format or size is not proper");
            await _dbContext.Images!.AddAsync(imageModel);
            await _dbContext.SaveChangesAsync();
            return imageModel;
        }
    }
}
