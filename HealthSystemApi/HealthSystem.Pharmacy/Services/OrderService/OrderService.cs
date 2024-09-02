using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Order;

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
            throw new NotImplementedException();
        }

        public async Task<bool> SubmitOrderAsync(SubmitOrderModel model)
        {
            throw new NotImplementedException();
        }
    }
}
