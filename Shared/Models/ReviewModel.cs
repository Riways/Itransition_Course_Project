using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace totten_romatoes.Shared.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string SubjectOfReview { get; set; }
        [Required]
        public Category ReviewCategory { get; set; }
        [Required]
        [Range(0,10)]
        public int AuthorGrade { get; set; }
        [Required]
        public string ReviewBody { get; set; }
        public string? AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public List<CommentModel>? Comments { get; set; }


        public ReviewModel(  string subjectOfReview, Category reviewCategory, int authorGrade, string reviewBody)
        {
            SubjectOfReview = subjectOfReview;
            ReviewCategory = reviewCategory;
            AuthorGrade = authorGrade;
            ReviewBody = reviewBody;
        }

        public ReviewModel()
        {
        }
    }

    public enum Category
    {
        Other, Book, Movie, Music, Device, Person, Game
    }
}
