using HealthSystem.Pharmacy.Data;

namespace HealthSystem.Pharmacy.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private PharmacyDbContext context;

        public OrderService(PharmacyDbContext context)
        {
            this.context = context;
        }
    }
}
