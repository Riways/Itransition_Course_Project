using Microsoft.EntityFrameworkCore;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared.Models;
using totten_romatoes.Shared;
using Bogus;
using Duende.IdentityServer.Extensions;

namespace totten_romatoes.Server.Services
{
    public interface IReviewService
    {
        public Task AddReviewToDb(ReviewModel newReview);
        public Task AddCommentToDb(CommentModel newComment);
        public Task AddTagsToDb(List<TagModel> tags);
        public Task<List<ReviewModel>> GetAllReviewsWithoutComments();
        public Task<ReviewModel> GetReviewById(long id);
        public List<TagModel> GetSpecificAmountOFTags(int amount);
        public Task DeleteReviewDromDb(long id);
        public Task<List<ReviewModel>> FullTextSearch(string key);
        public Task GenerateFakeReviews(int amount);
        public Task<List<TagModel>> GetListOfSimilarTagsFromDatabase(string key);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDropboxService _dropboxService ;
        private readonly ISubjectService _subjectService ;

        public ReviewService(ApplicationDbContext dbContext, IDropboxService dropboxService, ISubjectService subjectService)
        {
            _dbContext = dbContext;
            _dropboxService = dropboxService;
            _subjectService = subjectService;
        }

        public async Task AddReviewToDb(ReviewModel newReview)
        {
            if (newReview.ReviewImage != null)
            {
                string imageUrlOnDropbox = await _dropboxService.UploadImageToDropbox(newReview.ReviewImage);
                newReview.ReviewImage.ImageUrl = imageUrlOnDropbox;
            }
            if(newReview.Tags != null)
                ReplaceTagsInListWithExistingInDatabase(newReview.Tags);
            await ReplaceSubjectWithExistingInDatabase(newReview);
            await _dbContext.Reviews!.AddAsync(newReview);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCommentToDb(CommentModel newComment)
        {
            if (!newComment.CommentBody.IsNullOrEmpty())
            {
                await _dbContext.Comments!.AddAsync(newComment);
                _dbContext.SaveChanges();
            }
        }

        public async Task AddTagsToDb(List<TagModel> tags)
        {
            await _dbContext.Tags!.AddRangeAsync(tags);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ReviewModel>> GetAllReviewsWithoutComments()
        {
            var reviews = await _dbContext.Reviews!
                .Include(r => r.Author)
                .Include(r => r.ReviewImage)
                .Include(r => r.Tags)
                .Include(r => r.Subject)
                    .ThenInclude(s => s.Grades)
                .ToListAsync();
            foreach(var review in reviews)
            {
                review.CommentsAmount = await CountCommentsInReview(review.Id);
            }
            return reviews;
        }

        private async Task<int> CountCommentsInReview(long reviewId)
        {
            return await _dbContext.Comments!.Where(c => c.ReviewId == reviewId).CountAsync();
        }

        public async Task<ReviewModel> GetReviewById(long id) {
            var review = await _dbContext.Reviews!
                .Include(r => r.Author)
                .Include(r => r.ReviewImage)
                .Include(r => r.Tags)
                .Include(r => r.Subject)
                    .ThenInclude(s => s.Grades)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync(r => r.Id == id);
            return review;
        }

        public List<TagModel> GetSpecificAmountOFTags(int amount)
        {
            return _dbContext.Tags!.Take(amount).ToList();
        }

        public async Task DeleteReviewDromDb(long id)
        {
            var reviewToDelete = await _dbContext.Reviews!.FirstOrDefaultAsync(r => r.Id == id);
            if(reviewToDelete != null)
            {
                _dbContext.Reviews!.Remove(reviewToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        private void ReplaceTagsInListWithExistingInDatabase(List<TagModel> tags)
        {
            var tagNames = tags.Select(t => t.Name).ToList();
            var existingTags = _dbContext.Tags!.Where(t => tagNames.Contains(t.Name)).ToList();
            tagNames = existingTags?.Select(t => t.Name).ToList();
            tags.RemoveAll(t => tagNames!.Contains(t.Name));
            tags.AddRange(existingTags!);
        }

        private async Task ReplaceSubjectWithExistingInDatabase(ReviewModel review)
        {
            var subjectFromDb = await _dbContext.Subjects!.SingleOrDefaultAsync(s => s.Name == review.Subject.Name);
            if (subjectFromDb != null)
                review.Subject = subjectFromDb;
        }

        public async Task<List<ReviewModel>> FullTextSearch(string key)
        {
            key = key.Replace(" ", " <-> ");
            var reviewBodyAndTitleSearchResult = await _dbContext.Reviews!
                .Where(r => r.SearchVector!.Matches(EF.Functions.ToTsQuery($"{key}:*")))
                .Include(r => r.Subject)
                .Take(Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT)
                .ToListAsync();
            if(reviewBodyAndTitleSearchResult.Count < Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT)
            {
                int reviewsToAdd = Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT - reviewBodyAndTitleSearchResult.Count;
                var commentSearchResult = await _dbContext.Comments!
                    .Where(c => c.SearchVector!.Matches(EF.Functions.ToTsQuery($"{key}:*")))
                    .Include(c => c.Review)
                        .ThenInclude(r => r.Subject)
                    .Select(r => r.Review)
                    .Take(reviewsToAdd)
                    .ToListAsync();
                reviewBodyAndTitleSearchResult.AddRange(commentSearchResult);
            }
            return reviewBodyAndTitleSearchResult;
        }

        public async Task GenerateFakeReviews(int amount)
        {
            List<ReviewModel> fakeReviews = new();
            var commonFaker = new Faker();
            for(int i = 0; i < amount; i++)
            {
                ReviewModel review = new Faker<ReviewModel>()
                    .RuleFor(r => r.AuthorId, f => Constants.FAKER_USER_ID)
                    .RuleFor(r => r.AuthorGrade, f => f.Random.Int(1, 10))
                    .RuleFor(r => r.DateOfCreationInUTC, f => f.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddDays(7)))
                    .RuleFor(r => r.Title, f => f.Lorem.Sentence(f.Random.Int(1, Constants.FAKER_MAX_WORDS_IN_TITLE)))
                    .RuleFor(r => r.Subject, f => new SubjectModel { Name = f.Random.Word() })
                    .RuleFor(r => r.ReviewCategory, f => f.PickRandom<Category>())
                    .RuleFor(r => r.ReviewBody, f => f.Lorem.Sentences(f.Random.Int(Constants.FAKER_MIN_SENTENCES_IN_BODY, Constants.FAKER_MAX_SENTENCES_IN_BODY)))
                    .RuleFor(r => r.ReviewImage, f => new ImageModel { ImageName = f.Random.Word(), ImageType = Constants.IMAGE_FORMAT })
                    .RuleFor(r => r.Tags, f => {
                        List<TagModel> tags = new List<TagModel>();
                        for (int i = 0; i < f.Random.Int(Constants.FAKER_MIN_TAGS_AMOUNT, Constants.FAKER_MAX_TAGS_AMOUNT); i++)
                        {
                            TagModel newTag = new ();
                            newTag.Name = f.Random.Words(f.Random.Int(Constants.FAKER_MIN_WORDS_IN_TAG_AMOUNT, Constants.FAKER_MAX_WORDS_IN_TAG_AMOUNT));
                            tags.Add(newTag);
                        }
                        return tags;
                    });
                using (var client = new HttpClient())
                {
                    var content = await client.GetByteArrayAsync(commonFaker.Image.LoremFlickrUrl(Constants.FAKER_IMAGE_WIDTH, Constants.FAKER_IMAGE_HEIGHT, review.Subject.Name));
                    review.ReviewImage!.ImageData = content;
                }
                await AddReviewToDb(review);
            }
        }

        public async Task<List<TagModel>> GetListOfSimilarTagsFromDatabase(string key)
        {
            return await _dbContext.Tags!.Where(t => EF.Functions.ILike(t.Name, $"%{key}%")).Take(Constants.AMOUNT_OF_TAGS_IN_SEARCH_RESULT).ToListAsync();
        }
    }
}

