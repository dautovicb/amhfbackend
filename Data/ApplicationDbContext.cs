using Microsoft.EntityFrameworkCore;
using MentalHealth.Models;

namespace MentalHealth.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.categoryId);

            // modelBuilder.Entity<Comment>().HasOne(p => p.Post).WithMany(b => b.Comments);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.name)
                .IsUnique();
        }
    }
}
