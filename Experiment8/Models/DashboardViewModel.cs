namespace Experiment8.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
        public int LowStockProductsCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public List<Product> RecentProducts { get; set; } = new();
        public List<Product> LowStockProducts { get; set; } = new();
        public Dictionary<string, int> CategoryCounts { get; set; } = new();
    }
}

