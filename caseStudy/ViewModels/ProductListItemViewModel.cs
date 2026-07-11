using caseStudy.Models;

namespace caseStudy.ViewModels
{
    public class ProductListItemViewModel
    {
        public ProductDto Product { get; set; } = new();
        public PriceDto? LatestPrice { get; set; }
    }
}
