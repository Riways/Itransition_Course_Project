using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace totten_romatoes.Shared.Models
{
    public class ImageModel
    {
        public long Id { get; set; }
        [Required]
        public string ImageType { get; set; } = string.Empty;
        [Required]
        public string ImageName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        [NotMapped]
        [MaxLength(Constants.MAX_IMAGE_SIZE)]
        public byte[]? ImageData { get; set; }
    }
}
