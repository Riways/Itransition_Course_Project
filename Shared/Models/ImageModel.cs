using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totten_romatoes.Shared.Models
{
    public class ImageModel
    {
        public long Id { get; set; }
        [Required]
        public string ImageType { get; set; }
        [Required]
        public string ImageName { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public byte[] ImageData { get; set; }
    }
}
