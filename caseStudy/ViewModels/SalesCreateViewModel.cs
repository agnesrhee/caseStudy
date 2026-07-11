using System.ComponentModel.DataAnnotations;

namespace caseStudy.ViewModels
{
    public class SalesCreateViewModel
    {
        [Required]
        [Display(Name = "Seller Name")]
        public string Username { get; set; } = string.Empty;

        public IReadOnlyList<string> Categories { get; set; } = [];

        public List<SalesLineItemInputModel> Items { get; set; } =
        [
            new SalesLineItemInputModel()
        ];
    }

    public class SalesLineItemInputModel
    {
        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Select a product.")]
        public int ProductId { get; set; }

        [Range(1, 1000000, ErrorMessage = "Quantity must be at least one.")]
        public int Quantity { get; set; } = 1;
    }
}
