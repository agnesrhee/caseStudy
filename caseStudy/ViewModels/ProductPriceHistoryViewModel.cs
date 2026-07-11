using caseStudy.Models;

namespace caseStudy.ViewModels
{
    public class ProductPriceHistoryViewModel
    {
        public ProductDto Product { get; set; } = new();
        public IReadOnlyList<PriceChangeViewModel> PriceChanges { get; set; } = [];
        public int RawPriceCount { get; set; }
    }

    public class PriceChangeViewModel
    {
        public DateTime DateChanged { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
    }
}
