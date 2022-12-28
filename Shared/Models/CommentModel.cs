using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace totten_romatoes.Shared.Models
{
    public class CommentModel
    {
        public long Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        [JsonIgnore]
        public NpgsqlTsVector? SearchVector { get; set; }
    }
}
