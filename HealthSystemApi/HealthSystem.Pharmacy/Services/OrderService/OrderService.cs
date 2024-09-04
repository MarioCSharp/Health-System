using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Enums;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Cart;
using HealthSystem.Pharmacy.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private PharmacyDbContext context;

        public OrderService(PharmacyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> ChangeStatus(int orderId, string status)
        {
            var order = await context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return false;
            }

            order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), status, true); ;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<OrderDisplayModel>> OrdersInPharmacyAsync(int pharmacyId)
        {
            return await context.Orders
                .Include(x => x.OrderMedications)
                .ThenInclude(x => x.Medication)
                .Where(o => o.PharmacyId == pharmacyId)
                .Select(o => new OrderDisplayModel
                {
                    Id = o.Id,
                    Location = o.Location,
                    Name = o.Name,
                    PhoneNumber = o.PhoneNumber,
                    Status = o.Status.ToString(),
                    TotalPrice = o.OrderMedications.Sum(x => x.Medication.MedicationPrice),
                    CartItems = o.OrderMedications.Select(m => new CartItemModel()
                    {
                        Id = m.OrderId,
                        ItemPrice = m.Medication.MedicationPrice,
                        ItemImage = m.Medication.Image,
                        ItemName = m.Medication.MedicationName,
                        Quantity = m.Medication.MedicationQuantity
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<bool> SubmitOrderAsync(SubmitOrderModel model, string userId)
        {
            var userCart = await context.UserCarts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(c => c.Id == model.CartId);

            if (userCart is null)
            {
                return false;
            }

            var order = new Order()
            {
                UserId = userId,
                PharmacyId = userCart.PharmacyId,
                Location = model.Location,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };

            foreach (var item in userCart.CartItems)
            {
                var medication = await context.Medications.FindAsync(item.MedicationId);

                if (medication is null)
                {
                    return false;
                }

                var medicationId = item.MedicationId;
                var quantity = item.Quantity;

                var orderMedication = new OrderMedication
                {
                    MedicationId = medicationId,
                    Quantity = quantity
                };

                order.OrderMedications.Add(orderMedication);
                medication.MedicationQuantity -= item.Quantity;
            }

            userCart.CartItems.Clear();

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return await context.Orders.ContainsAsync(order);
        }
    }
}
