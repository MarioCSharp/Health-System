using HealthProject.Models;

namespace HealthProject.Services.LaboratoryService
{
    public interface ILaboratoryService
    {
        Task<LaboratoryReturnModel> CheckResult(string userNameId, string pass);
    }
}
