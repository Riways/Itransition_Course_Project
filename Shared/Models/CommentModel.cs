using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace totten_romatoes.Shared.Models
{
    public class CommentModel
    {
        public long Id { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string CommentBody { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfCreationInUTC { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public long ReviewId { get; set; }
        public ReviewModel? Review { get; set; }
        [JsonIgnore]
        public NpgsqlTsVector? SearchVector { get; set; }
    }
}
