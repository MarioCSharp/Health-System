using HealthSystem.HealthCare.Data;
using HealthSystem.HealthCare.Data.Models;
using HealthSystem.HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Services.MedicationService
{
    /// <summary>
    /// Service to manage medications and schedules for a user.
    /// </summary>
    public class MedicationService : IMedicationService
    {
        private HealthCareDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationService"/> class with the provided DbContext.
        /// </summary>
        /// <param name="context">The database context to be used by the service.</param>
        public MedicationService(HealthCareDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new medication and associated schedule to the database.
        /// </summary>
        /// <param name="medicationModel">The model containing the medication and schedule details.</param>
        /// <returns>Returns <c>true</c> if the medication and schedule were successfully added, otherwise <c>false</c>.</returns>
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
                Note = medicationModel.Note,
                Dose = medicationModel.Dose,
                UserId = medicationModel.UserId,
                MedicationScheduleId = schedule.Id
            };

            await context.Medications.AddAsync(medication);
            await context.SaveChangesAsync();

            return await context.Medications.ContainsAsync(medication) && await context.MedicationSchedules.ContainsAsync(schedule);
        }

        /// <summary>
        /// Retrieves all medications associated with a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of medication display models associated with the user.</returns>
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

        /// <summary>
        /// Deletes a medication and its associated schedule from the database.
        /// </summary>
        /// <param name="id">The ID of the medication to delete.</param>
        /// <returns>Returns <c>true</c> if the medication was successfully deleted, otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var medication = await context.Medications.FindAsync(id);

            if (medication == null)
            {
                return false;
            }

            var schedule = new MedicationSchedule() { Id = medication.MedicationScheduleId };

            context.Remove(medication);
            context.Remove(schedule);

            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves detailed information about a medication by its ID.
        /// </summary>
        /// <param name="id">The ID of the medication.</param>
        /// <returns>A <see cref="MedicationDetailsModel"/> containing detailed information about the medication.</returns>
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

        /// <summary>
        /// Retrieves the medication schedule for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of medication schedule models for the user.</returns>
        public async Task<List<MedicationScheduleModel>> GetUserScheduleAsync(string userId)
        {
            return await context.Medications
                .Where(medication => medication.UserId == userId)
                .Select(medication => new MedicationScheduleModel()
                {
                    Id = medication.Id,
                    Name = medication.Name,
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

        /// <summary>
        /// Retrieves valid medications for a user (current or future medications).
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of valid medication display models for the user.</returns>
        public async Task<List<MedicationDisplayModel>> GetUsersValidMedications(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new List<MedicationDisplayModel>();
            }

            return await context.Medications
                .Where(x => x.UserId == userId && x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now)
                .Take(3)
                .Select(x => new MedicationDisplayModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Dose = x.Dose,
                    Type = x.Type
                }).ToListAsync();
        }
    }
}
