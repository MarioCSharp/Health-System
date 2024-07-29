using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Medication;
using Microsoft.EntityFrameworkCore;

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
            var medication = new Medication()
            {
                Name = medicationModel.Name,
                Type = medicationModel.Type,
                EndDate = medicationModel.EndDate,
                StartDate = medicationModel.StartDate,
                HealthIssueId = medicationModel.HealthIssueId,
                Note = medicationModel.Note,
                Dose = medicationModel.Dose,
                UserId = scheduleModel.UserId
            };

            await context.Medications.AddAsync(medication);

            var schedule = new MedicationSchedule()
            {
                Days = scheduleModel.Days,
                MedicationId = medication.Id,
                Rest = scheduleModel.Rest,
                SkipCount = scheduleModel.SkipCount,
                Take = scheduleModel.Take,
                Times = scheduleModel.Times,
                UserId = scheduleModel.UserId
            };

            await context.MedicationSchedules.AddAsync(schedule);

            medication.MedicationScheduleId = schedule.Id;

            await context.SaveChangesAsync();

            return await context.Medications.ContainsAsync(medication) && await context.MedicationSchedules.ContainsAsync(schedule);
        }

        public async Task<List<MedicationDisplayModel>> AllByUser(string userId)
        {
            return await context.Medications
                .Where(x => x.UserId == userId)
                .Select(x => new MedicationDisplayModel()
                {
                    Id = x.Id,
                    Dose = x.Dose,
                    MedicationScheduleId = x.MedicationScheduleId,
                    Name = x.Name, 
                    Type = x.Type
                }).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medication = await context.Medications.FindAsync(id);

            if (medication == null)
            {
                return false;
            }

            var schedule = await context.MedicationSchedules.FindAsync(medication.MedicationScheduleId);

            if (schedule == null)
            {
                return false;
            }

            context.Remove(medication);
            context.Remove(schedule);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<MedicationDetailsModel> DetailsAsync(int id)
        {
            var med = await context.Medications.FindAsync(id);

            if (med is null)
            {
                return new MedicationDetailsModel();    
            }

            var model = new MedicationDetailsModel();

            model.Dose = med.Dose;
            model.Name = med.Name;
            model.StartDate = med.StartDate;
            model.EndDate = med.EndDate;
            model.Note = med.Note;
            med.MedicationScheduleId = med.MedicationScheduleId;
            med.Id = id;

            return model;
        }
    }
}
