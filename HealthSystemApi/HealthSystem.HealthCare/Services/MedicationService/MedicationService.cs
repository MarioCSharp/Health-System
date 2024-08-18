using HealthSystem.HealthCare.Data;
using HealthSystem.HealthCare.Data.Models;
using HealthSystem.HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private HealthCareDbContext context;

        public MedicationService(HealthCareDbContext context)
        {
            this.context = context;   
        }

        public async Task<bool> AddAsync(MedicationAddModel medicationModel)
        {

            var schedule = new MedicationSchedule()
            {
                Days = medicationModel.Days,
                Rest = medicationModel.Rest,
                SkipCount = medicationModel.SkipCount,
                Take = medicationModel.Take,
                Times = medicationModel.Times,
                UserId = medicationModel.UserId
            };

            await context.MedicationSchedules.AddAsync(schedule);
            await context.SaveChangesAsync();

            var medication = new Medication()
            {
                Name = medicationModel.Name,
                Type = medicationModel.Type,
                EndDate = medicationModel.EndDate,
                StartDate = medicationModel.StartDate,
                HealthIssueId = medicationModel.HealthIssueId,
                Note = medicationModel.Note,
                Dose = medicationModel.Dose,
                UserId = medicationModel.UserId,
                MedicationScheduleId = schedule.Id
            };

            await context.Medications.AddAsync(medication);
            await context.SaveChangesAsync();

            return await context.Medications.ContainsAsync(medication) && await context.MedicationSchedules.ContainsAsync(schedule);
        }

        public async Task<List<MedicationDisplayModel>> AllByUser(string userId)
        {
            return await context.Medications
                .Include(x => x.MedicationSchedule)
                .Where(x => x.MedicationSchedule.UserId == userId)
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

        public async Task<List<MedicationScheduleModel>> GetUserScheduleAsync(string userId)
        {
            return await context.Medications
                .Where(medication => medication.UserId == userId)
                .Select(medication => new MedicationScheduleModel()
                {
                    Id = medication.Id,
                    Name = medication.Name,
                    HealthIssueName = medication.HealthIssue.Name ?? string.Empty,
                    Dose = medication.Dose,
                    Type = medication.Type,
                    Note = medication.Note,
                    StartDate = medication.StartDate,
                    EndDate = medication.EndDate,
                    Times = medication.MedicationSchedule.Times,
                    SkipCount = medication.MedicationSchedule.SkipCount,
                    Take = medication.MedicationSchedule.Take,
                    Rest = medication.MedicationSchedule.Rest,
                    Days = medication.MedicationSchedule.Days
                }).ToListAsync();
        }
    }
}
