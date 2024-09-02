using HealthSystem.Pharmacy.Models.Order;

namespace HealthSystem.Pharmacy.Services.OrderService
{
    public interface IOrderService
    {
        Task<bool> SubmitOrderAsync(SubmitOrderModel model);

        Task<List<OrderDisplayModel>> OrdersInPharmacyAsync(int pharmacyId);
    }
}
