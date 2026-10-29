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
                new Category { CategoryIdentification = 1, Name = "Action", Description = "Action genre books" , DisplayOrder = "1" },
                new Category { CategoryIdentification = 2, Name = "Sci-Fi", Description = "Science fiction books" , DisplayOrder = "2" },
                new Category { CategoryIdentification = 3, Name = "History", Description = "History genre books" , DisplayOrder = "3" }
            );
        }
    }
}