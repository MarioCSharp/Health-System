﻿using HealthProject.Models;

namespace HealthProject.Services.MedicationService
{
    public interface IMedicationService
    {
        Task<List<MedicationDisplayModel>> AllByUser(string userId);
        Task<bool> AddAsync(MedicationAddModel model);
        Task<MedicationDetailsModel> DetailsAsync(int id);
    }
}
