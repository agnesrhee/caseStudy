using System.ComponentModel.DataAnnotations;

namespace caseStudy.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        public int ProductId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ProductSku { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ProductCategory { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; } = 1;

        public bool IsRecurring { get; set; }

        public DateTime SoldAt { get; set; } = DateTime.UtcNow;
    }
}
