﻿namespace HealthSystem.Pharmacy.Models.Cart
{
    public class CartDisplayModel
    {
        public int Id { get; set; }

        public float TotalPrice { get; set; }

        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
