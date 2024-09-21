using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Medication;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace HealthSystem.Pharmacy.Services.MedicationService
{
    /// <summary>
    /// Service responsible for handling operations related to medications in the pharmacy system.
    /// </summary>
    public class MedicationService : IMedicationService
    {
        private readonly PharmacyDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PharmacyDbContext"/> used for database operations.</param>
        public MedicationService(PharmacyDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new medication to the pharmacy database.
        /// </summary>
        /// <param name="model">The <see cref="MedicationAddModel"/> containing the details of the medication to add.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> AddAsync(MedicationAddModel model)
        {
            var medication = new Medication()
            {
                PharmacyId = model.PharmacyId,
                MedicationName = model.MedicationName,
                MedicationPrice = model.MedicationPrice,
                MedicationQuantity = model.MedicationQuantity
            };

            using (var stream = new MemoryStream())
            {
                await model.Image.CopyToAsync(stream);
                medication.Image = stream.ToArray();
            }

            await context.Medications.AddAsync(medication);
            await context.SaveChangesAsync();

            return await context.Medications.ContainsAsync(medication);
        }

        /// <summary>
        /// Adds a specified quantity to an existing medication.
        /// </summary>
        /// <param name="medicationId">The ID of the medication to update.</param>
        /// <param name="quantity">The quantity to add to the medication.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> AddQuantityAsync(int medicationId, int quantity)
        {
            var medication = await context.Medications.FindAsync(medicationId);

            if (medication is null)
            {
                return false;
            }

            medication.MedicationQuantity += quantity;
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves all medications for a specific pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy.</param>
        /// <returns>A list of <see cref="MedicationDisplayModel"/> representing the medications in the pharmacy.</returns>
        public async Task<List<MedicationDisplayModel>> AllInPharmacyAsync(int pharmacyId)
        {
            return await context.Medications
                .Where(x => x.PharmacyId == pharmacyId)
                .Select(x => new MedicationDisplayModel()
                {
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.MedicationName,
                    Price = x.MedicationPrice,
                    Quantity = x.MedicationQuantity
                }).ToListAsync();
        }

        /// <summary>
        /// Deletes a medication from the pharmacy database.
        /// </summary>
        /// <param name="medicationId">The ID of the medication to delete.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAsync(int medicationId)
        {
            var medication = await context.Medications.FindAsync(medicationId);

            if (medication is null)
            {
                return false;
            }

            context.Medications.Remove(medication);
            await context.SaveChangesAsync();

            return !await context.Medications.ContainsAsync(medication);
        }

        /// <summary>
        /// Edits the details of an existing medication.
        /// </summary>
        /// <param name="model">The <see cref="MedicationEditModel"/> containing the updated details of the medication.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> EditAsync(MedicationEditModel model)
        {
            var medication = await context.Medications.FindAsync(model.Id);

            if (medication is null)
            {
                return false;
            }

            medication.MedicationPrice = model.Price;
            medication.MedicationName = model.Name;
            medication.MedicationQuantity = model.Quantity;

            if (model.Image is not null)
            {
                using (var stream = new MemoryStream())
                {
                    await model.Image.CopyToAsync(stream);
                    medication.Image = stream.ToArray();
                }
            }

            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves medications based on the user's role and ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The role of the user (e.g., Pharmacist or PharmacyOwner).</param>
        /// <returns>A list of <see cref="MedicationDisplayModel"/> representing the medications accessible to the user.</returns>
        public async Task<List<MedicationDisplayModel>> GetMedications(string userId, string role)
        {
            var pharmacyId = 0;

            if (role == "Pharmacist")
            {
                var pharmacist = await context.Pharmacists.FirstOrDefaultAsync(x => x.UserId == userId);
                pharmacyId = pharmacist?.PharmacyId ?? 0;
            }
            else
            {
                var pharmacy = await context.Pharmacies.FirstOrDefaultAsync(x => x.OwnerUserId == userId);
                pharmacyId = pharmacy?.Id ?? 0;
            }

            return await context.Medications
                .Where(x => x.PharmacyId == pharmacyId)
                .Select(x => new MedicationDisplayModel()
                {
                    Id = x.Id,
                    Name = x.MedicationName,
                    Image = x.Image,
                    Price = x.MedicationPrice,
                    Quantity = x.MedicationQuantity
                }).ToListAsync();
        }
    }
}
