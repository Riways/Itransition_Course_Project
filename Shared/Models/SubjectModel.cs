using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class SubjectModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Subject name is required")]
        [MaxLength(40, ErrorMessage = "Subject name is too long")]
        [MinLength(2, ErrorMessage = "Subject name is too short")]
        public string Name { get; set; } = string.Empty;
        public List<GradeModel>? Grades { get; set; }
        public List<ReviewModel>? Reviews { get; set; }
    }
}
