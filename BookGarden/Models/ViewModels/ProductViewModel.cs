using BookGarden.Models;
using Microsoft.AspNetCore.Http;

namespace BookGarden.Models.ViewModels;

public class ProductViewModel
{
    public Product Product { get; set; } = new();

    public IFormFile? ImageFile { get; set; }
}