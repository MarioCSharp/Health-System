namespace HealthSystem.Pharmacy.Models.Cart
{
    public class CartItemModel
    {
        public int Id { get; set; }

        public string? ItemName { get; set; }

        public float ItemPrice { get; set; }

        public int Quantity { get; set; }

        public byte[]? ItemImage { get; set; }
    }
}
