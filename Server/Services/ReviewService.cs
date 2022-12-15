using totten_romatoes.Client.Pages.Components.Reviews;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IReviewService
    {
        public void AddReviewToDb(ReviewModel newReview);
        public List<ReviewModel> GetAllReviews();
        public ReviewModel GetReviewById(int id);
        public void AddCommentToDb(CommentModel newComment);
        public void AddGradeToDb(GradeModel newGrade);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _dbContext;

        public ReviewService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddReviewToDb(ReviewModel newReview)
        {
            _dbContext.Reviews.Add(newReview);
            _dbContext.SaveChanges();
        }

        public List<ReviewModel> GetAllReviews()
        {
            return _dbContext.Reviews.ToList();
        }

        public ReviewModel GetReviewById(int id)
        {
            return _dbContext.Reviews.Single(r => r.Id == id);
        }

        public void AddCommentToDb(CommentModel newComment)
        {
            _dbContext.Comments.Add(newComment);
            _dbContext.SaveChanges();
        }

        public void AddGradeToDb(GradeModel newGrade)
        {
            _dbContext.Grades.Add(newGrade);
            _dbContext.SaveChanges();
        }
    }
}
