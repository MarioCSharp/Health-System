using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Enums;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Cart;
using HealthSystem.Pharmacy.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Services.OrderService
{
    /// <summary>
    /// Service responsible for handling operations related to orders in the pharmacy system.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly PharmacyDbContext context;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PharmacyDbContext"/> used for database operations.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        public OrderService(PharmacyDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Changes the status of a specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to change the status of.</param>
        /// <param name="status">The new status to assign to the order.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> ChangeStatus(int orderId, string status)
        {
            var order = await context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return false;
            }

            order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), status, true);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves an order based on the provided EGN (personal identification number) and adds the medications from the prescription to the user's cart.
        /// </summary>
        /// <param name="egn">The EGN of the patient.</param>
        /// <param name="cartId">The ID of the user's cart.</param>
        /// <param name="token">The authorization token for making external API requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
            using (var reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var splitLine = line.Split('-');
                    var medicationName = splitLine[0];

                    var meds = await context.Medications
                        .Include(x => x.Pharmacy)
                        .Where(x => x.MedicationName == medicationName && x.PharmacyId == cart.PharmacyId)
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

        /// <summary>
        /// Retrieves a list of orders for a specific pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy.</param>
        /// <returns>A list of <see cref="OrderDisplayModel"/> representing the orders in the pharmacy.</returns>
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

        /// <summary>
        /// Submits a new order based on the user's cart and details provided in the submission model.
        /// </summary>
        /// <param name="model">The <see cref="SubmitOrderModel"/> containing the order details.</param>
        /// <param name="userId">The ID of the user placing the order.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
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

                var orderMedication = new OrderMedication
                {
                    MedicationId = item.MedicationId,
                    Quantity = item.Quantity
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

    /// <summary>
    /// Model for displaying recipe information.
    /// </summary>
    public class RecipeDisplayModel
    {
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
