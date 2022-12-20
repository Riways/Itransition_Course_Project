using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace totten_romatoes.Shared.Models
{
    public class ReviewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Subject of review is required")]
        [MaxLength(255, ErrorMessage = "Maximum subject length is 255")]
        [MinLength(2, ErrorMessage = "Minimum subject length is 2")]
        public string SubjectOfReview { get; set; }
        [Required]
        public Category ReviewCategory { get; set; }
        [Required]
        [Range(0,10)]
        public int AuthorGrade { get; set; }
        [Required(ErrorMessage = "Review body is required")]
        [MaxLength(5000, ErrorMessage = "Review body is too long")]
        [MinLength(1, ErrorMessage = "Review body is too short")]
        public string ReviewBody { get; set; }
        [Required]
        public DateTime DateOfCreationInUTC { get; set; }
        public long? ImageId { get; set; }
        public ImageModel? ReviewImage { get; set; }
        public string? AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public List<CommentModel>? Comments { get; set; }
        public List<TagModel>? Tags { get; set; }
        public List<GradeModel>? Grades { get; set; }


        public ReviewModel(string authorId,  string subjectOfReview, Category reviewCategory, int authorGrade, string reviewBody, DateTime dateOfCreationInUTC)
        {
            AuthorId = authorId;
            SubjectOfReview = subjectOfReview;
            ReviewCategory = reviewCategory;
            AuthorGrade = authorGrade;
            ReviewBody = reviewBody;
            DateOfCreationInUTC = dateOfCreationInUTC;
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

