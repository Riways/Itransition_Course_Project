using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace totten_romatoes.Shared.Models
{
    [Table("Tags")]
    public class TagModel
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(Constants.MAX_TAG_LENGTH, ErrorMessage = "Tag is too long")]
        [MinLength(Constants.MIN_TAG_LENGTH, ErrorMessage = "Tag is too short")]
        public string Name { get; set; } = string.Empty;
        public List<ReviewModel>? Reviews { get; set; }

        public TagModel(string name)
        {
            Name = name;
        }

        public TagModel()
        {
        }


    }
}
