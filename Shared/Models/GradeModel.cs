using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class GradeModel
    {
        public long Id { get; set; }
        [Required]
        [Range(1, 5)]
        public int Value { get; set; }
        [Required]
        public long SubjectId { get; set; }
        public SubjectModel? Subject { get; set; }
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }
    }
}
