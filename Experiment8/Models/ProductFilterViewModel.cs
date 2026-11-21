namespace Experiment8.Models
{
    public class ProductFilterViewModel
    {
        public List<Product> Products { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SelectedCategory { get; set; }
        public string? SelectedStockStatus { get; set; }
        public List<string> Categories { get; set; } = new();
        public int TotalCount { get; set; }
    }
}

