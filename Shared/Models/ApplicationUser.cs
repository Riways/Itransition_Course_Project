
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace totten_romatoes.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Username is too long")]
        [MinLength(2, ErrorMessage = "Username is too short")]
        public override string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Email is too long")]
        [MinLength(5, ErrorMessage = "Email is too short")]
        [EmailAddress]
        public override string Email { get; set; } = string.Empty;

        [NotMapped]
        public long Rating { get; set; }
    }
}
