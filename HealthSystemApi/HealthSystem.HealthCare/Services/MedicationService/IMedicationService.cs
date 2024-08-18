using HealthSystem.HealthCare.Models;

namespace HealthSystem.HealthCare.Services.MedicationService
{
    public interface IMedicationService
    {
        Task<bool> AddAsync(MedicationAddModel medicationModel);
        Task<bool> DeleteAsync(int id);
        Task<MedicationDetailsModel> DetailsAsync(int id);
        Task<List<MedicationDisplayModel>> AllByUser(string userId);
        Task<List<MedicationScheduleModel>> GetUserScheduleAsync(string userId);
    }
}
