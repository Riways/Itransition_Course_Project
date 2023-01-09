using Bogus;
using Duende.IdentityServer.Extensions;
using Markdig.Helpers;
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
        public Task AddLikesToDb(List<LikeModel> likes);
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
        public Task UpdateReview(ReviewModel updatedReview);
        public Task<List<ReviewModel>> FullTextSearch(string key);
        public Task GenerateFakeReviews(int amount);
        public Task<List<TagModel>> GetListOfSimilarTagsFromDatabase(string key);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDropboxService _dropboxService;
        private readonly IUserService _userService;
        private readonly IConfiguration _appConfig;

        public ReviewService(ApplicationDbContext dbContext, IDropboxService dropboxService, IUserService userService, IConfiguration appConfig)
        {
            _dbContext = dbContext;
            _dropboxService = dropboxService;
            _userService = userService;
            _appConfig = appConfig;
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

        public async Task AddLikesToDb(List<LikeModel> likes)
        {
            _dbContext.Likes!.AddRange(likes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ReviewModel>> GetLightweightListOfReviews(string userId)
        {
            List<ReviewModel> reviews = await _dbContext.Reviews!
                .Include(r => r.Subject)
                .Include(r => r.Likes)
                .Where(r => r.AuthorId!.Equals(userId))
                .ToListAsync();
            return reviews;
        }

        public async Task<List<ReviewModel>> GetChunkOfSortedReviews(int page, SortBy sortType)
        {
            int reviewsToSkip = Constants.REVIEWS_ON_HOME_PAGE * page;
            List<ReviewModel> reviews = await _dbContext.Reviews!
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
            foreach (ReviewModel? review in reviews)
            {
                if (!review.Comments.IsNullOrEmpty())
                {
                    review.CommentsAmount = review.Comments!.Count;
                    review.Comments = null;
                }
                ApplicationUser? userWithCountedRating = usersWithCountedRating.SingleOrDefault(r => r.Id == review.AuthorId);
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

        private static Expression<Func<ReviewModel, object>> ChooseSortType(SortBy sortType)
        {
            Expression<Func<ReviewModel, object>> NewestSort = r => r.DateOfCreationInUTC;
            Expression<Func<ReviewModel, object>> PopularSort = r => r.Likes.Count;
            return sortType == SortBy.Popular ? PopularSort : NewestSort;
        }

        public async Task<ReviewModel> GetReviewById(long id)
        {
            ReviewModel? review = await _dbContext.Reviews!
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
            foreach (TagModel tag in tags)
            {
                tag.Reviews = null;
            }
            return tags;
        }

        public async Task DeleteReviewFromDb(long id)
        {
            ReviewModel? reviewToDelete = await _dbContext.Reviews!.FirstOrDefaultAsync(r => r.Id == id);
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
            LikeModel? likeToDelete = _dbContext.Likes!.FirstOrDefault(l => l.Id == id);
            if (likeToDelete != null)
            {
                  _dbContext.Likes!.Remove(likeToDelete);
                  _dbContext.SaveChanges();
            }
        }

        public async Task UpdateReview(ReviewModel updatedReview)
        {
            SubjectModel? subjectFromDb = await _dbContext.Subjects!.Include(s => s.Reviews).SingleOrDefaultAsync(s => s.Name == updatedReview.Subject.Name);
            ReviewModel? existingReview = await _dbContext.Reviews!
                .Include(r => r.Tags)
                .SingleOrDefaultAsync(r => r.Id == updatedReview.Id);
            if (existingReview == null)
            {
                throw new ArgumentException("Attempt to update review that doesn't exist");
            }
            if (subjectFromDb == null || subjectFromDb.Id != existingReview.SubjectId)
            {
                UpdateSubject(subjectFromDb, existingReview, updatedReview);
            }
            UpdateTags(existingReview, updatedReview);
            CopyReviewState(existingReview, updatedReview);
            _dbContext!.Entry(existingReview).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        private void UpdateSubject(SubjectModel subjectFromDb, ReviewModel existingReview, ReviewModel updatedReview)
        {
            SubjectModel subjToSave;
            if (subjectFromDb == null)
            {
                subjToSave = updatedReview.Subject;
                subjToSave.Id = 0;
                subjToSave.Name = updatedReview.Subject.Name;
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

        private void UpdateTags(ReviewModel existingReview, ReviewModel updatedReview)
        {
            List<long> oldTagsIds = existingReview.Tags!.Select(t => t.Id).ToList();
            List<long> newTagsIds = updatedReview.Tags!.Select(t => t.Id).ToList();
            existingReview.Tags!.RemoveAll(t => !newTagsIds.Contains(t.Id));
            updatedReview.Tags!.RemoveAll(t => oldTagsIds.Contains(t.Id));
            _dbContext.Tags!.AttachRange(updatedReview.Tags.Where(t => t.Id != 0 && !oldTagsIds.Contains(t.Id)));
            existingReview.Tags.AddRange(updatedReview.Tags);
        }

        private static void CopyReviewState(ReviewModel to, ReviewModel from)
        {
            to.Title = from.Title;
            to.AuthorGrade = from.AuthorGrade;
            to.ReviewBody = from.ReviewBody;
            to.ReviewCategory = from.ReviewCategory;
            to.DateOfCreationInUTC = from.DateOfCreationInUTC;
        }

        private async Task ReplaceSubjectWithExistingInDatabase(ReviewModel updatedReview)
        {
            SubjectModel? subjectFromDb = await _dbContext.Subjects!.SingleOrDefaultAsync(s => s.Name == updatedReview.Subject.Name);
            if (subjectFromDb != null)
            {
                updatedReview.Subject = subjectFromDb;
                updatedReview.SubjectId = subjectFromDb.Id;
            }
        }

        public async Task<List<ReviewModel>> FullTextSearch(string key)
        {
            key = key.Trim();
            key = key.Replace(" ", " <-> ");
            List<ReviewModel> reviewBodyAndTitleSearchResult = await _dbContext.Reviews!
                .Where(r => r.SearchVector!.Matches(EF.Functions.ToTsQuery($"{key}:*")))
                .Include(r => r.Subject)
                .Distinct()
                .Take(Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT)
                .ToListAsync();
            if (reviewBodyAndTitleSearchResult.Count < Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT)
            {
                int reviewsToAdd = Constants.AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT - reviewBodyAndTitleSearchResult.Count;
                List<ReviewModel?> commentSearchResult = await _dbContext.Comments!
                    .Where(c => c.SearchVector!.Matches(EF.Functions.ToTsQuery($"{key}:*")))
                    .Include(c => c.Review)
                        .ThenInclude(r => r.Subject)
                    .Select(r => r.Review)
                    .Distinct()
                    .Take(reviewsToAdd)
                    .ToListAsync();
                if (!commentSearchResult.IsNullOrEmpty())
                {
                    reviewBodyAndTitleSearchResult.AddRange(commentSearchResult!);
                }
            }
            return reviewBodyAndTitleSearchResult;
        }

        public async Task GenerateFakeReviews(int amount)
        {
            Faker commonFaker = new();
            using HttpClient client = new();
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
                    .RuleFor(r => r.Comments,  f => 
                    {
                        List<CommentModel> comments = new();
                        for (int i = 0; i < f.Random.Int(Constants.FAKER_MIN_COMMENTS_AMOUNT, Constants.FAKER_MAX_COMMENTS_AMOUNT); i++)
                        {
                            CommentModel newComment = new()
                            {
                                AuthorId = Constants.FAKER_USER_ID,
                                DateOfCreationInUTC = f.Date.Between(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow),
                                CommentBody = f.Lorem.Sentences(f.Random.Int(Constants.FAKER_MIN_SENTENCES_IN_COMMENT_AMOUNT, Constants.FAKER_MAX_SENTENCES_IN_COMMENT_AMOUNT))
                            };
                            if(newComment.CommentBody.Length > Constants.MAX_COMMENT_LENGTH)
                            {
                                newComment.CommentBody = newComment.CommentBody.Substring(0, Constants.MAX_COMMENT_LENGTH);
                            }
                            comments.Add(newComment);
                        }
                        return comments;
                    })
                    .RuleFor(r => r.Tags, f =>
                    {
                        List<TagModel> tags = new();
                        for (int i = 0; i < f.Random.Int(Constants.FAKER_MIN_TAGS_AMOUNT, Constants.FAKER_MAX_TAGS_AMOUNT); i++)
                        {
                            var tagName = f.Random.Words(f.Random.Int(Constants.FAKER_MIN_WORDS_IN_TAG_AMOUNT, Constants.FAKER_MAX_WORDS_IN_TAG_AMOUNT));
                            if (tagName.Length > Constants.MAX_TAG_LENGTH)
                                tagName = tagName.Substring(0, Constants.MAX_TAG_LENGTH);
                            TagModel newTag = new()
                            {
                                Name = tagName
                            };
                            if (_dbContext.Tags!.Any(t => t.Name.Equals(tagName)))
                            {
                                newTag = _dbContext.Tags!.Single(t => t.Name.Equals(tagName));
                            }
                            tags.Add(newTag);
                        }
                        return tags;
                    });
                byte[] content = await client.GetByteArrayAsync(commonFaker.Image.LoremFlickrUrl(Constants.FAKER_IMAGE_WIDTH, Constants.FAKER_IMAGE_HEIGHT, review.Subject.Name));
                review.ReviewImage!.ImageData = content;
                await AddReviewToDb(review);
                await GenerateLikesToReview(commonFaker.Random.Int(Constants.FAKER_MIN_LIKES_AMOUNT, Constants.FAKER_MAX_LIKES_AMOUNT), review, commonFaker);
            }
        }

        private async Task GenerateLikesToReview(int amount, ReviewModel review, Faker faker)
        {
            List<LikeModel> likes = new();
            for (int z = 0; z < amount; z++)
            { 
                string username = faker.Random.Uuid().ToString();
                username = username.Substring(0, 25);
                if(!await _userService.IsUsernameAvailable(username))
                {
                    continue;
                }
                string email = $"{username}@fake.bo";
                string newUserId = await _userService.CreateUser(username, email, _appConfig["bogus_password"]);
                LikeModel newLike = new()
                {
                    FromUserId = newUserId,
                    ToUserId = Constants.FAKER_USER_ID,
                    ReviewId = review.Id
                };
                likes.Add(newLike);
            }
            await AddLikesToDb(likes);
        }

        public async Task<List<TagModel>> GetListOfSimilarTagsFromDatabase(string key)
        {
            return await _dbContext.Tags!.AsNoTracking().Where(t => EF.Functions.ILike(t.Name, $"%{key}%")).Take(Constants.AMOUNT_OF_TAGS_IN_SEARCH_RESULT).ToListAsync();
        }
    }
}