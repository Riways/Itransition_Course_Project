using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totten_romatoes.Shared.Models
{
    public class LikeModel
    {
        public long Id { get; set; }
        [Required]
        public string FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }
        [Required]
        public string ToUserId { get; set; }
        public ApplicationUser ToUser { get; set; }
        public long ReviewId { get; set; }
        public ReviewModel Review { get; set; }
    }
}
