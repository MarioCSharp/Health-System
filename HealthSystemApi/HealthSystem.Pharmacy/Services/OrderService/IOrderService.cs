using HealthSystem.Pharmacy.Models.Order;

namespace HealthSystem.Pharmacy.Services.OrderService
{
    public interface IOrderService
    {
        Task<bool> SubmitOrderAsync(SubmitOrderModel model, string userId);

        Task<List<OrderDisplayModel>> OrdersInPharmacyAsync(int pharmacyId);

        Task<bool> GetOrderByEGNAsync(string egn, int cartId, string token);

        Task<bool> ChangeStatus(int orderId, string status);
    }
}
