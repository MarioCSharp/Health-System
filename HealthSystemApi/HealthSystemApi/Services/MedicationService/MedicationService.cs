using HealthSystemApi.Data;
using HealthSystemApi.Models.Medication;

namespace HealthSystemApi.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private ApplicationDbContext context;

        public MedicationService(ApplicationDbContext context)
        {
            this.context = context;   
        }

        public async Task<bool> AddAsync(MedicationAddModel medicationModel, MedicationScheduleAddModel scheduleModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MedicationDetailsModel> DetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
