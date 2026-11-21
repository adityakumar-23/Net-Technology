using Experiment8.Data;
using Microsoft.AspNetCore.Mvc;

namespace Experiment8.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IProductRepository productRepository, ILogger<DashboardController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("Dashboard")]
        [Route("Dashboard/Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _productRepository.GetDashboardMetricsAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load dashboard metrics.");
                TempData["ErrorMessage"] = "Could not load dashboard data from SQL Server. Please verify database connection.";
                return View(new Models.DashboardViewModel());
            }
        }
    }
}

