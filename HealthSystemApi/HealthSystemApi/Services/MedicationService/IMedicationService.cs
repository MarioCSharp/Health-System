using HealthSystemApi.Models.Medication;

namespace HealthSystemApi.Services.MedicationService
{
    public interface IMedicationService
    {
        Task<bool> AddAsync(MedicationAddModel medicationModel, MedicationScheduleAddModel scheduleModel);
        Task<bool> DeleteAsync(int id);
        Task<MedicationDetailsModel> DetailsAsync(int id);
        Task<List<MedicationDisplayModel>> AllByUser(string userId);
    }
}
