using HealthSystem.Documents.Data;
using HealthSystem.Documents.Data.Models;
using HealthSystem.Documents.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Services.ReminderService
{
    public class ReminderService : IReminderService
    {
        private DocumentsDbContext context;

        public ReminderService(DocumentsDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(ReminderAddModel model, string userId)
        {
            var reminder = new Reminder()
            {
                Name = model.Name,
                Type = model.Type,
                DateAdded = DateTime.Now,
                RemindTime = model.RemindTime,
                UserId = userId
            };

            await context.Reminders.AddAsync(reminder);
            await context.SaveChangesAsync();

            return await context.Reminders.ContainsAsync(reminder);
        }

        public async Task<List<ReminderDetailsModel>> AllByUserAsync(string userId)
        {
            return await context.Reminders
                .Where(x => x.UserId == userId)
                .Select(x => new ReminderDetailsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    DateAdded = x.DateAdded.ToString("dd/MM/yyyy HH:mm"),
                    ReminderDate = x.RemindTime.ToString("dd/MM/yyyy HH:mm")
                }).ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id, string userId)
        {
            var reminder = await context.Reminders.FindAsync(id);

            if (reminder is null || reminder.UserId != userId)
            {
                return false;
            }

            context.Reminders.Remove(reminder);
            await context.SaveChangesAsync();

            return !await context.Reminders.ContainsAsync(reminder);    
        }
    }
}
