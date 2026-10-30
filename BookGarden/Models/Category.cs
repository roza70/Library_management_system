using System.ComponentModel.DataAnnotations;

namespace BookGarden.Models
{
    public class Category
    {
        [Key]
        public int CategoryIdentification { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description{ get; set; } = string.Empty;
        [Range(0,100,ErrorMessage ="Range must be between 0 and 100!!")]
        public string DisplayOrder{ get; set; } = string.Empty;
    }
}