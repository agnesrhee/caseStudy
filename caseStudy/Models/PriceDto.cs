namespace caseStudy.Models
{
    public class PriceDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
    }
}
