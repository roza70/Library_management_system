using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookGarden.Models;

public class Product
{
    [Key]
    public int ProductIdentification { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    [Required]
    [Display(Name = "List Price")]
    [Range(1, 10000)]
    public double ListPrice { get; set; }

    [Required]
    [Display(Name = "Price for 50+")]
    [Range(1, 10000)]
    public double Price50 { get; set; }

    [Required]
    [Display(Name = "Price for 100+")]
    [Range(1, 10000)]
    public double Price100 { get; set; }

    public int CategoryIdentification { get; set; }
    [ForeignKey("CategoryIdentification")]

    public Category? Category { get; set; }

    [Display(Name = "Product Image")]
    public string ImageUrl { get; set; } = string.Empty;
}
