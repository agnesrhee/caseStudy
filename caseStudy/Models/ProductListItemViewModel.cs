namespace caseStudy.Models
{
    public class ProductListItemViewModel
    {
        public ProductDto Product { get; set; } = new();
        public PriceDto? LatestPrice { get; set; }
    }
}
