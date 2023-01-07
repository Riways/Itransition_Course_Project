using Bogus;
using Duende.IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IReviewService
    {
        public Task AddReviewToDb(ReviewModel newReview);
        public Task AddCommentToDb(CommentModel newComment);
        public Task AddTagsToDb(List<TagModel> tags);
        public void AddLikeToDb(LikeModel like);
        public Task<List<ReviewModel>> GetLightweightListOfReviews(string userId);
        public Task<List<ReviewModel>> GetChunkOfSortedReviews(int page, SortBy sortType);
        public Task<ReviewModel> GetReviewById(long id);
        public Task<int> GetAmountOfReviews();
        public Task<int> GetAmountOfCommentsInReview(long id);
        public Task<List<CommentModel>> GetCommentsFromReview(long id);
        public List<TagModel> GetDefaultAmountOfTags();
        public Task DeleteReviewFromDb(long id);
        public Task DeleteMuiltipleReviews(IEnumerable<long> ids);
        public void DeleteLikeDromDb(long id);
        public Task UpdateReview(ReviewModel review);
        public Task<List<ReviewModel>> FullTextSearch(string key);
        public Task GenerateFakeReviews(int amount);
        public Task<List<TagModel>> GetListOfSimilarTagsFromDatabase(string key);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDropboxService _dropboxService;

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
            if (newReview.Tags != null)
            {
                _dbContext.Tags!.AttachRange(newReview.Tags.Where(t => t.Id != 0));
            }
            await ReplaceSubjectWithExistingInDatabase(newReview);
            await _dbContext.Reviews!.AddAsync(newReview);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCommentToDb(CommentModel newComment)
        {
            if (!newComment.CommentBody.IsNullOrEmpty())
            {
                await _dbContext.Comments!.AddAsync(newComment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddTagsToDb(List<TagModel> tags)
        {
            await _dbContext.Tags!.AddRangeAsync(tags);
            await _dbContext.SaveChangesAsync();
        }

        public void AddLikeToDb(LikeModel like)
        {
            if (!_dbContext.Likes!.Any(l => l.FromUserId == like.FromUserId && l.ReviewId == like.ReviewId))
            {
                _dbContext.Likes!.Add(like);
                _dbContext.SaveChanges();
            }
        }

        public async Task<List<ReviewModel>> GetLightweightListOfReviews(string userId)
        {
            var reviews = await _dbContext.Reviews!
                .Include(r => r.Subject)
                .Include(r => r.Likes)
                .Where(r => r.AuthorId!.Equals(userId))
                .ToListAsync();
            return reviews;
        }

        public async Task<List<ReviewModel>> GetChunkOfSortedReviews(int page, SortBy sortType)
        {
            int reviewsToSkip = Constants.REVIEWS_ON_HOME_PAGE * page;
            var reviews = await _dbContext.Reviews!
                .Include(r => r.Author)
                .Include(r => r.ReviewImage)
                .Include(r => r.Tags)
                .Include(r => r.Subject)
                    .ThenInclude(s => s.Grades)
                .Include(r => r.Likes)
                .Include(r => r.Comments)
                .OrderByDescending(ChooseSortType(sortType))
                .Skip(reviewsToSkip)
                .Take(Constants.REVIEWS_ON_HOME_PAGE)
                .ToListAsync();
            List<ApplicationUser> usersWithCountedRating = new();
            foreach (var review in reviews)
            {
                if (!review.Comments.IsNullOrEmpty())
                {
                    review.CommentsAmount = review.Comments!.Count();
                    review.Comments = null;
                }
                var userWithCountedRating = usersWithCountedRating.SingleOrDefault(r => r.Id == review.AuthorId);
                if (userWithCountedRating == null)
                {
                    review.Author!.Rating = CountUserRating(review.Author.Id);
                    usersWithCountedRating.Add(review.Author);
                }
                else
                {
                    review.Author!.Rating = userWithCountedRating.Rating;
                }
            }
            return reviews;
        }

        private Expression<Func<ReviewModel, Object>> ChooseSortType(SortBy sortType)
        {
            Expression<Func<ReviewModel, Object>> NewestSort = r => r.DateOfCreationInUTC;
            Expression<Func<ReviewModel, Object>> PopularSort = r => r.Likes.Count;
            if (sortType == SortBy.Popular)
                return PopularSort;
            else
                return NewestSort;
        }

        public async Task<ReviewModel> GetReviewById(long id)
        {
            var review = await _dbContext.Reviews!
                .AsNoTracking()
                .Include(r => r.Author)
                .Include(r => r.ReviewImage)
                .Include(r => r.Tags)
                .Include(r => r.Likes)
                .Include(r => r.Subject)
                    .ThenInclude(s => s.Grades)
                .Include(r => r.Comments!)
                    .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync(r => r.Id == id);
            if (review != null && !review.Comments.IsNullOrEmpty())
            {
                review.Comments = review.Comments!.OrderBy(c => c.DateOfCreationInUTC).ToList();
                review.Author!.Rating = CountUserRating(review.Author.Id);
            }
            return review;
        }

        private int CountUserRating(string userId)
        {
            return _dbContext.Likes!.Where(l => l.ToUserId == userId).Count();
        }

        public async Task<int> GetAmountOfReviews()
        {
            return await _dbContext.Reviews!.CountAsync();
        }

        public async Task<int> GetAmountOfCommentsInReview(long id)
        {
            return await _dbContext.Comments!.Where(c => c.ReviewId == id).CountAsync();
        }

        public async Task<List<CommentModel>> GetCommentsFromReview(long id)
        {
            return await _dbContext.Comments!.Include(c => c.Author).Where(c => c.ReviewId == id).ToListAsync();
        }

        public List<TagModel> GetDefaultAmountOfTags()
        {
            List<TagModel> tags = _dbContext.Tags!
                .Include(t => t.Reviews)
                .OrderByDescending(t => t.Reviews!.Count)
                .Take(Constants.AMOUNT_OF_TAGS_IN_CLOUD).ToList();
            foreach (var tag in tags)
            {
                tag.Reviews = null;
            }
            return tags;
        }

        public async Task DeleteReviewFromDb(long id)
        {
            var reviewToDelete = await _dbContext.Reviews!.FirstOrDefaultAsync(r => r.Id == id);
            if (reviewToDelete != null)
            {
                _dbContext.Reviews!.Remove(reviewToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteMuiltipleReviews(IEnumerable<long> ids)
        {
            IEnumerable<ReviewModel> reviewsToDelete = await _dbContext.Reviews!.Where(r => ids.Contains(r.Id)).ToListAsync();
            if (reviewsToDelete != null)
            {
                _dbContext.Reviews!.RemoveRange(reviewsToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public void DeleteLikeDromDb(long id)
        {
            var likeToDelete = _dbContext.Likes!.FirstOrDefault(l => l.Id == id);
            if (likeToDelete != null)
            {
                _dbContext.Likes!.Remove(likeToDelete);
                _dbContext.SaveChanges();
            }
        }

        public async Task UpdateReview(ReviewModel review)
        {
            var subjectFromDb = await _dbContext.Subjects!.Include(s => s.Reviews).SingleOrDefaultAsync(s => s.Name == review.Subject.Name);
            var existingReview = await _dbContext.Reviews!
                .Include(r => r.Tags)
                .SingleOrDefaultAsync(r => r.Id == review.Id);
            if (existingReview != null)
            {
                //subj
                if (subjectFromDb == null || subjectFromDb.Id != existingReview.SubjectId)
                {
                    SubjectModel subjToSave;
                    if (subjectFromDb == null)
                    {
                        subjToSave = review.Subject;
                        subjToSave.Id = 0;
                        subjToSave.Name = review.Subject.Name;
                        subjToSave.Reviews = new List<ReviewModel> { existingReview };
                        _dbContext!.Entry(subjToSave).State = EntityState.Added;
                    }
                    else
                    {
                        subjToSave = subjectFromDb;
                    }
                    existingReview.SubjectId = subjToSave.Id;
                    existingReview.Subject = subjToSave;
                }
                //tag
                List<long> oldTagsIds = existingReview.Tags!.Select(t => t.Id).ToList();
                List<long> newTagsIds = review.Tags!.Select(t => t.Id).ToList();
                existingReview.Tags.RemoveAll(t => !newTagsIds.Contains(t.Id));
                review.Tags.RemoveAll(t => oldTagsIds.Contains(t.Id));
                _dbContext.Tags!.AttachRange(review.Tags.Where(t => t.Id != 0 && !oldTagsIds.Contains(t.Id)));
                //other
                existingReview.Tags.AddRange(review.Tags);
                existingReview.Title = review.Title;
                existingReview.AuthorGrade = review.AuthorGrade;
                existingReview.ReviewBody = review.ReviewBody;
                existingReview.ReviewCategory = review.ReviewCategory;
                existingReview.DateOfCreationInUTC = review.DateOfCreationInUTC;
            }
            _dbContext!.Entry(existingReview).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        private async Task ReplaceSubjectWithExistingInDatabase(ReviewModel review)
        {
            var subjectFromDb = await _dbContext.Subjects!.SingleOrDefaultAsync(s => s.Name == review.Subject.Name);
            if (subjectFromDb != null)
            {
                review.Subject = subjectFromDb;
                review.SubjectId = subjectFromDb.Id;
            }
        }

        public async Task<List<ReviewModel>> FullTextSearch(string key)
        {
            key = key.Trim();
            key = key.Replace(" ", " <-> ");
            var reviewBodyAndTitleSearchResult = await _dbContext.Reviews!
                .Where(r => r.SearchVector!.Matches(EF.Functions.ToTsQuery($"{key}:*")))
                .Include(r => r.Subject)
                .Distinct()
                .Take(Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT)
                .ToListAsync();
            if (reviewBodyAndTitleSearchResult.Count < Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT)
            {
                int reviewsToAdd = Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT - reviewBodyAndTitleSearchResult.Count;
                var commentSearchResult = await _dbContext.Comments!
                    .Where(c => c.SearchVector!.Matches(EF.Functions.ToTsQuery($"{key}:*")))
                    .Include(c => c.Review)
                        .ThenInclude(r => r.Subject)
                    .Select(r => r.Review)
                    .Distinct()
                    .Take(reviewsToAdd)
                    .ToListAsync();
                if (!commentSearchResult.IsNullOrEmpty())
                    reviewBodyAndTitleSearchResult.AddRange(commentSearchResult!);
            }
            return reviewBodyAndTitleSearchResult;
        }

        public async Task GenerateFakeReviews(int amount)
        {
            List<ReviewModel> fakeReviews = new();
            var commonFaker = new Faker();
            using (var client = new HttpClient())
            {
                for (int i = 0; i < amount; i++)
                {
                    ReviewModel review = new Faker<ReviewModel>()
                        .RuleFor(r => r.AuthorId, f => Constants.FAKER_USER_ID)
                        .RuleFor(r => r.AuthorGrade, f => f.Random.Int(1, 10))
                        .RuleFor(r => r.DateOfCreationInUTC, f => f.Date.Between(DateTime.UtcNow.AddDays(-14), DateTime.UtcNow.AddDays(-7)))
                        .RuleFor(r => r.Title, f => f.Lorem.Sentence(f.Random.Int(1, Constants.FAKER_MAX_WORDS_IN_TITLE)))
                        .RuleFor(r => r.Subject, f => new SubjectModel { Name = f.Random.Word() })
                        .RuleFor(r => r.ReviewCategory, f => f.PickRandom<Category>())
                        .RuleFor(r => r.ReviewBody, f => f.Lorem.Sentences(f.Random.Int(Constants.FAKER_MIN_SENTENCES_IN_BODY, Constants.FAKER_MAX_SENTENCES_IN_BODY)))
                        .RuleFor(r => r.ReviewImage, f => new ImageModel { ImageName = f.Random.Word(), ImageType = Constants.IMAGE_FORMAT })
                        .RuleFor(r => r.Comments, f =>
                        {
                            List<CommentModel> comments = new();
                            for (int i = 0; i < f.Random.Int(Constants.FAKER_MIN_COMMENTS_AMOUNT, Constants.FAKER_MAX_COMMENTS_AMOUNT); i++)
                            {
                                CommentModel newComment = new();
                                newComment.AuthorId = Constants.FAKER_USER_ID;
                                newComment.DateOfCreationInUTC = f.Date.Between(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                                newComment.CommentBody = f.Lorem.Sentences(f.Random.Int(Constants.FAKER_MIN_SENTENCES_IN_COMMENT_AMOUNT, Constants.FAKER_MAX_SENTENCES_IN_COMMENT_AMOUNT));
                                comments.Add(newComment);
                            }
                            return comments;
                        })
                        .RuleFor(r => r.Tags, f =>
                        {
                            List<TagModel> tags = new();
                            for (int i = 0; i < f.Random.Int(Constants.FAKER_MIN_TAGS_AMOUNT, Constants.FAKER_MAX_TAGS_AMOUNT); i++)
                            {
                                TagModel newTag = new();
                                newTag.Name = f.Random.Words(f.Random.Int(Constants.FAKER_MIN_WORDS_IN_TAG_AMOUNT, Constants.FAKER_MAX_WORDS_IN_TAG_AMOUNT));
                                tags.Add(newTag);
                            }
                            return tags;
                        });
                    var content = await client.GetByteArrayAsync(commonFaker.Image.LoremFlickrUrl(Constants.FAKER_IMAGE_WIDTH, Constants.FAKER_IMAGE_HEIGHT, review.Subject.Name));
                    review.ReviewImage!.ImageData = content;
                    await AddReviewToDb(review);
                }
            }
        }

        public async Task<List<TagModel>> GetListOfSimilarTagsFromDatabase(string key)
        {
            return await _dbContext.Tags!.AsNoTracking().Where(t => EF.Functions.ILike(t.Name, $"%{key}%")).Take(Constants.AMOUNT_OF_TAGS_IN_SEARCH_RESULT).ToListAsync();
        }
    }
}