namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public decimal DiscountRate { get; set; }

        public void SetProductCode(string code) => ProductCode = code;
    }
}
