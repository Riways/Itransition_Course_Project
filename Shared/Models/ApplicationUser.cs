
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace totten_romatoes.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(25)]
        [MinLength(2)]
        public override string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(5)]
        [EmailAddress]
        public override string Email { get; set; }
    }
}
