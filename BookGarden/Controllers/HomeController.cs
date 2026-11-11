
using BookGarden.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookGarden.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(
            int? id,
            CancellationToken cancellationToken)
        {
            if (id is null or 0)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(
                id.Value,
                cancellationToken);

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
