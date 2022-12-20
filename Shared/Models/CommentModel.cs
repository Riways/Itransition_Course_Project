using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class CommentModel
    {
        public long Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public ApplicationUser Author { get; set; }
    }
}
