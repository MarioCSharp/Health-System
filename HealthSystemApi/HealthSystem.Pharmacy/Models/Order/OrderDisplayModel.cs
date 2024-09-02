using HealthSystem.Pharmacy.Models.Cart;

namespace HealthSystem.Pharmacy.Models.Order
{
    public class OrderDisplayModel
    {
        public string? Location { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
