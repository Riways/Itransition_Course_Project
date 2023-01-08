using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class LikeModel
    {
        public long Id { get; set; }
        [Required]
        public string FromUserId { get; set; } = string.Empty;
        public ApplicationUser? FromUser { get; set; }
        [Required]
        public string ToUserId { get; set; } = string.Empty;
        public ApplicationUser? ToUser { get; set; }
        [Required]
        public long ReviewId { get; set; }
        public ReviewModel? Review { get; set; }
    }
}
