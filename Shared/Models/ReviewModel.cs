using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser Author {get;set; }
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
        public List<Comment>? Comments { get; set; }
    }

    public enum Category
    {
        Book, Movie, Music, Device, Person, Game, Other
    }
}
