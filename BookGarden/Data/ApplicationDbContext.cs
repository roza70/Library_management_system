using Microsoft.EntityFrameworkCore;
using BookGarden.Models;

namespace BookGarden.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryIdentification = 1, Name = "Action", Description = "Action genre books" },
                new Category { CategoryIdentification = 2, Name = "Sci-Fi", Description = "Science fiction books" },
                new Category { CategoryIdentification = 3, Name = "History", Description = "History genre books" }
            );
        }
    }
}