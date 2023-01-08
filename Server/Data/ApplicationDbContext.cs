using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ReviewModel>(reviewModel =>
            {
                reviewModel.Property(e => e.ReviewCategory)
                 .HasConversion(
                     v => v.ToString(),
                     v => (Category)Enum.Parse(typeof(Category), v));
                reviewModel.Property(e => e.AuthorId)
                 .IsRequired();
            });
            modelBuilder.Entity<ReviewModel>().HasGeneratedTsVectorColumn(
                    r => r.SearchVector,
                    "english",
                    r => new { r.ReviewBody, r.Title })
                    .HasIndex(r => r.SearchVector)
                    .HasMethod("GIN");
            modelBuilder.Entity<CommentModel>().HasGeneratedTsVectorColumn(
                    r => r.SearchVector,
                    "english",
                    r => new { r.CommentBody })
                    .HasIndex(r => r.SearchVector)
                    .HasMethod("GIN");
            modelBuilder.Entity<TagModel>().HasAlternateKey(t => t.Name);
            modelBuilder.Entity<SubjectModel>().HasAlternateKey(g => g.Name);
            modelBuilder.Entity<GradeModel>().HasAlternateKey(g => new { g.SubjectId, g.AuthorId });
            modelBuilder.Entity<LikeModel>().HasAlternateKey(l => new { l.FromUserId, l.ReviewId });
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex").IsUnique();
        }

        public DbSet<ReviewModel>? Reviews { get; set; }
        public DbSet<CommentModel>? Comments { get; set; }
        public DbSet<GradeModel>? Grades { get; set; }
        public DbSet<ImageModel>? Images { get; set; }
        public DbSet<TagModel>? Tags { get; set; }
        public DbSet<LikeModel>? Likes { get; set; }
        public DbSet<SubjectModel>? Subjects { get; set; }
    }
}