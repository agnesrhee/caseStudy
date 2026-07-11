using System.ComponentModel.DataAnnotations;

namespace caseStudy.ViewModels
{
    public class SalesEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Seller Name")]
        public string Username { get; set; } = string.Empty;

        public IReadOnlyList<string> Categories { get; set; } = [];

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Select a product.")]
        public int ProductId { get; set; }

        [Range(1, 1000000, ErrorMessage = "Quantity must be at least one.")]
        public int Quantity { get; set; } = 1;
    }
}
