using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Medication;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private PharmacyDbContext context;

        public MedicationService(PharmacyDbContext context)
        {
            this.context = context;
        }

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

        public async Task<List<MedicationDisplayModel>> AllInPharmacyAsync(int pharmacyId)
        {
            return await context.Medications
                .Where(x => x.PharmacyId == pharmacyId)
                .Select(x => new MedicationDisplayModel()
                {
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.MedicationName,
                    Price = x.MedicationPrice
                }).ToListAsync();
        }

        public async Task<bool> EditAsync(MedicationEditModel model)
        {
            var medication = await context.Medications.FindAsync(model.Id);

            if (medication is null)
            {
                return false;
            }

            medication.MedicationPrice = model.Price;
            medication.MedicationName = model.Name;

            using (var stream = new MemoryStream())
            {
                await model.Image.CopyToAsync(stream);
                medication.Image = stream.ToArray();
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
