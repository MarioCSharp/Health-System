using HealthSystem.Pharmacy.Models.Cart;

namespace HealthSystem.Pharmacy.Models.Order
{
    public class OrderDisplayModel
    {
        public int Id { get; set; }

        public string? Location { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Status { get; set; }

        public float TotalPrice { get; set; }

        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
