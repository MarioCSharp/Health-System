namespace HealthProject.Models
{
    public class PharmacyProductDisplayModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public byte[]? Image { get; set; }
        public int Quantity { get; set; }
        public int UserEnteredQuantity { get; set; }
        public (PharmacyProductDisplayModel product, int quantity) AddToCartParameter => (this, UserEnteredQuantity);
    }
}
