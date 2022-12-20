using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totten_romatoes.Shared.Models
{
    public class TagModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<ReviewModel> Reviews { get; set; }
    }
}
