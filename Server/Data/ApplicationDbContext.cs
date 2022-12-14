using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                 .Entity<ReviewModel>()
                 .Property(e => e.ReviewCategory)
                 .HasConversion(
                     v => v.ToString(),
                     v => (Category)Enum.Parse(typeof(Category), v)); 
        }

        public DbSet<ReviewModel>? Reviews { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Grade>? Grades { get; set; }
    }
}