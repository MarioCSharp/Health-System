using HealthSystem.Identity.Identity.Models;

namespace HealthSystem.Identity.Services.AccountService
{
    public interface IAccountService
    {
        Task<List<AccountDisplayModel>> GetAccountsAsync();
    }
}
