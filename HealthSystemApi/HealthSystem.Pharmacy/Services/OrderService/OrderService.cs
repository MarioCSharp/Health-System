using Azure.Core;
using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Enums;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Cart;
using HealthSystem.Pharmacy.Models.Order;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace HealthSystem.Pharmacy.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private PharmacyDbContext context;
        private HttpClient httpClient;

        public OrderService(PharmacyDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
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

        public async Task<bool> GetOrderByEGNAsync(string egn, int cartId, string token)
        {
            var cart = await context.UserCarts.FindAsync(cartId);

            if (cart is null)
            {
                throw new Exception("No user cart found!");
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://results/api/Recipe/GeLastRecipe?EGN={egn}");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var recipeResponse = await httpClient.SendAsync(request);

            if (!recipeResponse.IsSuccessStatusCode)
            {
                var responseContent = await recipeResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed fetching the doctor! Status: {recipeResponse.StatusCode}, Response: {responseContent}");
            }

            var fileStream = await recipeResponse.Content.ReadAsStreamAsync();
            var contentType = recipeResponse.Content.Headers.ContentType?.ToString();
            var fileName = recipeResponse.Content.Headers.ContentDisposition?.FileName ?? "downloadedFile";

            using (var reader = new StreamReader(fileStream))
            {
                string line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var splitLine = line.Split('-');

                    var medicationName = splitLine[0];

                    var meds = await context.Medications
                        .Include(x => x.Pharmacy)
                        .Where(x => x.MedicationName == medicationName && x.PharmacyId == cart.Id)
                        .ToListAsync();

                    if (meds is null)
                    {
                        continue;
                    }

                    foreach (var medication in meds)
                    {
                        var cartItem = await context.CartItems
                        .FirstOrDefaultAsync(x => x.MedicationId == medication.Id && x.UserCartId == cartId);

                        if (cartItem is null)
                        {
                            var newCartItem = new CartItem()
                            {
                                MedicationId = medication.Id,
                                Quantity = 1,
                                UserCartId = cartId
                            };

                            await context.CartItems.AddAsync(newCartItem);
                            await context.SaveChangesAsync();

                            return await context.CartItems.ContainsAsync(newCartItem);
                        }
                    }
                }
            }

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
                    TotalPrice = o.OrderMedications.Sum(x => x.Medication.MedicationPrice * x.Quantity),
                    CartItems = o.OrderMedications.Select(m => new CartItemModel()
                    {
                        Id = m.OrderId,
                        ItemPrice = m.Medication.MedicationPrice,
                        ItemImage = m.Medication.Image,
                        ItemName = m.Medication.MedicationName,
                        Quantity = m.Quantity
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

    public class RecipeDisplayModel
    {
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
