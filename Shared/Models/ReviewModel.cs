using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace totten_romatoes.Shared.Models
{
    public class ReviewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(255, ErrorMessage = "Maximum tile length is 255")]
        [MinLength(2, ErrorMessage = "Minimum title length is 2")]
        public string? Title { get; set; }
        [Required]
        public Category ReviewCategory { get; set; }
        [Required]
        [Range(0, 10)]
        public int AuthorGrade { get; set; }
        [Required(ErrorMessage = "Review body is required")]
        [MaxLength(5000, ErrorMessage = "Review body is too long")]
        [MinLength(1, ErrorMessage = "Review body is too short")]
        public string? ReviewBody { get; set; }
        [Required]
        public DateTime DateOfCreationInUTC { get; set; }
        [Required]
        public long SubjectId { get; set; }
        public SubjectModel Subject { get; set; } = new();
        public long? ReviewImageId { get; set; }
        public ImageModel? ReviewImage { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public List<CommentModel>? Comments { get; set; }
        [NotMapped]
        public int? CommentsAmount { get; set; }
        [MaxLength(Constants.MAX_AMOUNT_OF_TAGS_IN_REVIEW, ErrorMessage = $"Max amount of tags is exceeded")]
        public List<TagModel>? Tags { get; set; }
        public List<LikeModel> Likes { get; set; } = new();
        [JsonIgnore]
        public NpgsqlTsVector? SearchVector { get; set; }
    }

    public enum Category
    {
        Other, Book, Movie, Music, Device, Person, Game
    }

    public enum SortBy
    {
        Newest, Popular
    }

    
}

