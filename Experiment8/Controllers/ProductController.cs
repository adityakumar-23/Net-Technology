using System.Text;
using Experiment8.Data;
using Experiment8.Models;
using Microsoft.AspNetCore.Mvc;

namespace Experiment8.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        // GET: /Product or /Product/Index
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? category, string? stockStatus)
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync(search, category, stockStatus);
                var categories = await _productRepository.GetCategoriesAsync();

                var model = new ProductFilterViewModel
                {
                    Products = products,
                    SearchTerm = search,
                    SelectedCategory = category,
                    SelectedStockStatus = stockStatus,
                    Categories = categories,
                    TotalCount = products.Count
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product inventory list.");
                TempData["ErrorMessage"] = "Failed to load products from SQL Server: " + ex.Message;
                return View(new ProductFilterViewModel());
            }
        }

        // GET: /Product/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID #{id} was not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID {ProductID}", id);
                TempData["ErrorMessage"] = "Failed to load product details: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Product/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _productRepository.GetCategoriesAsync();
            return View(new Product { Quantity = 10, Price = 100.00m });
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productRepository.GetCategoriesAsync();
                return View(product);
            }

            try
            {
                int newId = await _productRepository.AddProductAsync(product);
                TempData["SuccessMessage"] = $"Product '{product.ProductName}' (ID #{newId}) was added to inventory successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create new product in ADO.NET");
                ModelState.AddModelError(string.Empty, "Database Error: " + ex.Message);
                ViewBag.Categories = await _productRepository.GetCategoriesAsync();
                return View(product);
            }
        }

        // GET: /Product/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID #{id} not found.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Categories = await _productRepository.GetCategoriesAsync();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error opening edit view for ProductID {ProductID}", id);
                TempData["ErrorMessage"] = "Failed to load product for editing: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest("Product ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productRepository.GetCategoriesAsync();
                return View(product);
            }

            try
            {
                bool updated = await _productRepository.UpdateProductAsync(product);
                if (updated)
                {
                    TempData["SuccessMessage"] = $"Product '{product.ProductName}' (ID #{product.ProductID}) was updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = $"Product with ID #{product.ProductID} could not be updated or was not found.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update product ID {ProductID}", product.ProductID);
                ModelState.AddModelError(string.Empty, "Database update error: " + ex.Message);
                ViewBag.Categories = await _productRepository.GetCategoriesAsync();
                return View(product);
            }
        }

        // GET: /Product/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID #{id} not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete view for ProductID {ProductID}", id);
                TempData["ErrorMessage"] = "Failed to load product: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                string productName = product?.ProductName ?? $"ID #{id}";

                bool deleted = await _productRepository.DeleteProductAsync(id);
                if (deleted)
                {
                    TempData["SuccessMessage"] = $"Product '{productName}' (ID #{id}) was permanently deleted from inventory.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete Product ID #{id}. It may have already been removed.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Product ID {ProductID}", id);
                TempData["ErrorMessage"] = "Database delete error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Product/ExportCsv (Demonstrates ADO.NET SqlDataAdapter & DataTable)
        [HttpGet]
        public IActionResult ExportCsv(string? search, string? category)
        {
            try
            {
                var dt = _productRepository.GetProductsDataTable(search, category);
                var sb = new StringBuilder();

                // CSV Header
                sb.AppendLine("ProductID,ProductName,Category,Quantity,Price,TotalValue,SupplierName,CreatedDate");

                // Rows
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    int qty = Convert.ToInt32(row["Quantity"]);
                    decimal price = Convert.ToDecimal(row["Price"]);
                    decimal total = qty * price;
                    string name = $"\"{row["ProductName"].ToString()?.Replace("\"", "\"\"")}\"";
                    string cat = $"\"{row["Category"].ToString()?.Replace("\"", "\"\"")}\"";
                    string supplier = $"\"{row["SupplierName"].ToString()?.Replace("\"", "\"\"")}\"";
                    string date = Convert.ToDateTime(row["CreatedDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    sb.AppendLine($"{row["ProductID"]},{name},{cat},{qty},{price:F2},{total:F2},{supplier},{date}");
                }

                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/csv", $"Inventory_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export products DataTable.");
                TempData["ErrorMessage"] = "Export failed: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

