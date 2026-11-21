using System.Data;
using Experiment8.Models;

namespace Experiment8.Data
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync(string? search = null, string? category = null, string? stockStatus = null);
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<DashboardViewModel> GetDashboardMetricsAsync();
        Task<List<string>> GetCategoriesAsync();
        DataTable GetProductsDataTable(string? search = null, string? category = null);
    }
}

