using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Dell Laptop",
                Category = "Electronics",
                Price = 70000,
                Description = "Dell laptop for students and professionals."
            },

            new Product
            {
                Id = 2,
                Name = "HP Laptop",
                Category = "Electronics",
                Price = 65000,
                Description = "HP laptop with good performance."
            },

            new Product
            {
                Id = 3,
                Name = "Samsung Mobile",
                Category = "Mobile",
                Price = 25000,
                Description = "Samsung smartphone with excellent camera."
            },

            new Product
            {
                Id = 4,
                Name = "Boat Headphones",
                Category = "Accessories",
                Price = 2000,
                Description = "Wireless headphones with good sound quality."
            }
        };

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}