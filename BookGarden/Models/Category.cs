using System.ComponentModel.DataAnnotations;

namespace BookGarden.Models
{
    public class Category
    {
        [Key]
        public int CategoryIdentification { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}