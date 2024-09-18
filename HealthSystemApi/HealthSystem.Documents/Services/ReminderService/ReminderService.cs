using HealthSystem.Documents.Data;
using HealthSystem.Documents.Data.Models;
using HealthSystem.Documents.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Services.ReminderService
{
    /// <summary>
    /// Provides methods for managing reminders, including adding, retrieving, and removing reminders.
    /// </summary>
    public class ReminderService : IReminderService
    {
        private readonly DocumentsDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="DocumentsDbContext"/> used to interact with the database.</param>
        public ReminderService(DocumentsDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new reminder to the database.
        /// </summary>
        /// <param name="model">The model containing the reminder details.</param>
        /// <param name="userId">The user ID of the reminder owner.</param>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation, with a value indicating whether the addition was successful.</returns>
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

        /// <summary>
        /// Retrieves all reminders associated with a specific user.
        /// </summary>
        /// <param name="userId">The user ID whose reminders are to be retrieved.</param>
        /// <returns>A <see cref="Task{List{ReminderDetailsModel}}"/> representing the asynchronous operation, with a list of reminders.</returns>
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

        /// <summary>
        /// Removes a reminder from the database.
        /// </summary>
        /// <param name="id">The ID of the reminder to remove.</param>
        /// <param name="userId">The user ID of the reminder owner.</param>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation, with a value indicating whether the removal was successful.</returns>
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
