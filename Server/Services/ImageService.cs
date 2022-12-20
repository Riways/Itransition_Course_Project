using totten_romatoes.Server.Data;
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
            await _dbContext.Images.AddAsync(imageModel);
            await _dbContext.SaveChangesAsync();
            return imageModel;
        }
    }
}
