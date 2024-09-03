using HealthSystem.Pharmacy.Data;
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

        public async Task<List<OrderDisplayModel>> OrdersInPharmacyAsync(int pharmacyId)
        {
            return await context.Orders
                .Include(x => x.OrderMedications)
                .ThenInclude(x => x.Medication)
                .Where(o => o.PharmacyId == pharmacyId)
                .Select(o => new OrderDisplayModel
                {
                    Location = o.Location,
                    Name = o.Name,
                    PhoneNumber = o.PhoneNumber,
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
                PharmacyId = userCart.PharmacyId
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

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return await context.Orders.ContainsAsync(order);
        }
    }
}
