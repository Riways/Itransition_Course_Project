using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Shared.Models
{
    public class GradeModel
    {
        public long Id { get; set; }
        [Required]
        public ApplicationUser Author { get; set; }
        [Required]
        [Range(0,5)]
        public int Value { get; set; }
        [Required]
        public long ReviewId { get; set; }
        [Required]
        public ReviewModel Review { get; set; }
    }
}
