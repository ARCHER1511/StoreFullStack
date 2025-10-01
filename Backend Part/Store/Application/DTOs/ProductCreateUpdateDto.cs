using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class ProductCreateUpdateDto
    {
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } // nullable for update
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
