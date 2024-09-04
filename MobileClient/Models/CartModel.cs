namespace HealthProject.Models
{
    public class CartModel
    {
        public int Id { get; set; }

        public float TotalPrice { get; set; }

        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
