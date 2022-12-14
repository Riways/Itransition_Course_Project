using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public ApplicationUser Author { get; set; }
    }
}
