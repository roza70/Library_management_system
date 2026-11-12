using BookGarden.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookGarden.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryIdentification = 1,
                    Name = "Action",
                    Description = "Action genre books",
                    DisplayOrder = 1
                },
                new Category
                {
                    CategoryIdentification = 2,
                    Name = "Sci-Fi",
                    Description = "Science fiction books",
                    DisplayOrder = 2
                },
                new Category
                {
                    CategoryIdentification = 3,
                    Name = "History",
                    Description = "History genre books",
                    DisplayOrder = 3
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductIdentification = 1,
                    Title = "Store of the Jungle",
                    Author = "Alba Solver",
                    Description = "A thrilling adventure deep in the heart of an uncharted jungle, where ancient secrets and hidden treasures await those brave enough to seek them.",
                    ISBN = "JNG7777770001",
                    ListPrice = 45,
                    Price50 = 35,
                    Price100 = 30,
                    CategoryIdentification = 1
                },
                new Product
                {
                    ProductIdentification = 2,
                    Title = "Money and Time",
                    Author = "Bob Roarman",
                    Description = "An insightful exploration of wealth, ambition, and the relentless passage of time. A story that challenges what it truly means to be rich.",
                    ISBN = "MNT8888880001",
                    ListPrice = 55,
                    Price50 = 45,
                    Price100 = 40,
                    CategoryIdentification = 3
                },
                new Product
                {
                    ProductIdentification = 3,
                    Title = "Secret of the Lake",
                    Author = "Jessica Alban",
                    Description = "A mysterious tale set around a remote lake where strange occurrences lead a young detective to uncover a decades-old secret buried beneath the water.",
                    ISBN = "LKE9999990001",
                    ListPrice = 35,
                    Price50 = 28,
                    Price100 = 25,
                    CategoryIdentification = 1
                },
                new Product
                {
                    ProductIdentification = 4,
                    Title = "Moon and Planets",
                    Author = "Kristen Lober",
                    Description = "A captivating journey through the cosmos, blending science and imagination as humanity reaches beyond the stars for the very first time.",
                    ISBN = "SPC1010100001",
                    ListPrice = 65,
                    Price50 = 55,
                    Price100 = 50,
                    CategoryIdentification = 2
                },
                new Product
                {
                    ProductIdentification = 5,
                    Title = "Forest and Dawn",
                    Author = "Laura Goldberg",
                    Description = "At the edge of an ancient forest, dawn reveals more than just light. A poetic novel about renewal, loss, and the quiet strength found in nature.",
                    ISBN = "FRD1111110001",
                    ListPrice = 30,
                    Price50 = 24,
                    Price100 = 20,
                    CategoryIdentification = 3
                },
                new Product
                {
                    ProductIdentification = 6,
                    Title = "The Whispering Grove",
                    Author = "Elara Vance",
                    Description = "Deep within a forgotten grove, the trees hold memories of those who came before. A hauntingly beautiful story of legacy and belonging.",
                    ISBN = "WGR1212120001",
                    ListPrice = 42,
                    Price50 = 34,
                    Price100 = 30,
                    CategoryIdentification = 1
                },
                new Product
                {
                    ProductIdentification = 7,
                    Title = "The Forgotten Cipher",
                    Author = "Anya Ravenwood",
                    Description = "A brilliant cryptographer stumbles upon an ancient code that could rewrite history. But cracking it means confronting a conspiracy centuries in the making.",
                    ISBN = "CIP1313130001",
                    ListPrice = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryIdentification = 2
                },
                new Product
                {
                    ProductIdentification = 8,
                    Title = "The Silent Orchard",
                    Author = "Elara Vance",
                    Description = "When silence falls over a once-thriving orchard, one woman returns to her childhood home to uncover the truth behind its abandonment and her family's past.",
                    ISBN = "ORC1414140001",
                    ListPrice = 38,
                    Price50 = 30,
                    Price100 = 26,
                    CategoryIdentification = 3
                });
        }
    }
}
