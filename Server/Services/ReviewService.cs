using Microsoft.EntityFrameworkCore;
using totten_romatoes.Client.Pages.Components.Reviews;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IReviewService
    {
        public Task AddReviewToDb(ReviewModel newReview);
        public void AddCommentToDb(CommentModel newComment);
        public Task AddTagsToDb(List<TagModel> tags);
        public List<ReviewModel> GetAllReviews();
        public ReviewModel GetReviewById(long id);
        public List<TagModel> GetSpecificAmountOFTags(int amount);
        public Task DeleteReviewDromDb(long id);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDropboxService _dropboxService ;

        public ReviewService(ApplicationDbContext dbContext, IDropboxService dropboxService)
        {
            _dbContext = dbContext;
            _dropboxService = dropboxService;
        }

        public async Task AddReviewToDb(ReviewModel newReview)
        {
            if (newReview.ReviewImage != null)
            {
                string imageUrlOnDropbox = await _dropboxService.UploadImageToDropbox(newReview.ReviewImage);
                newReview.ReviewImage.ImageUrl = imageUrlOnDropbox;
            }
            if(newReview.Tags != null)
                await ReplaceTagsInListWithExistingInDatabase(newReview.Tags);
            await _dbContext.Reviews.AddAsync(newReview);
            await _dbContext.SaveChangesAsync();
        }

        public void AddCommentToDb(CommentModel newComment)
        {
            _dbContext.Comments.Add(newComment);
            _dbContext.SaveChanges();
        }

        public async Task AddTagsToDb(List<TagModel> tags)
        {
            await _dbContext.Tags.AddRangeAsync(tags);
            await _dbContext.SaveChangesAsync();
        }

        public List<ReviewModel> GetAllReviews()
        {
            return _dbContext.Reviews.Include(r => r.Author)
                .Include(r => r.ReviewImage)
                .Include(r => r.Tags)
                .Include(r => r.Subject)
                    .ThenInclude(s => s.Grades)
                .ToList();
        }

        public ReviewModel GetReviewById(long id)
        {
            return _dbContext.Reviews.Single(r => r.Id == id);
        }

        public List<TagModel> GetSpecificAmountOFTags(int amount)
        {
            return _dbContext.Tags.Take(amount).ToList();
        }

        private async Task ReplaceTagsInListWithExistingInDatabase(List<TagModel> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == tags[i].Name);
                if (existingTag != null)
                {
                    tags[i] = existingTag;
                }
            }
        }

        public async Task DeleteReviewDromDb(long id)
        {
            ReviewModel reviewToDelete = new();
            reviewToDelete.Id = id;
            _dbContext.Attach<ReviewModel>(reviewToDelete);
            _dbContext.Reviews.Remove(reviewToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}

