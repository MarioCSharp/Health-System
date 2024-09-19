using HealthSystem.HealthCare.Data;
using HealthSystem.HealthCare.Data.Models;
using HealthSystem.HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Services.LogbookService
{
    /// <summary>
    /// Provides services for managing log entries, including CRUD operations.
    /// </summary>
    public class LogbookService : ILogbookService
    {
        private readonly HealthCareDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogbookService"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the log entries data.</param>
        public LogbookService(HealthCareDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new log entry to the database.
        /// </summary>
        /// <param name="model">The model containing the details of the log entry to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> AddAsync(LogAddModel model)
        {
            var log = new Log
            {
                Type = model.Type,
                Date = DateTime.Now,
                Factors = model.Factors,
                Note = model.Note,
                UserId = model.UserId,
                Values = model.Values
            };

            await context.Logs.AddAsync(log);
            await context.SaveChangesAsync();

            return await context.Logs.ContainsAsync(log);
        }

        /// <summary>
        /// Retrieves a list of log entries associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose log entries are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of log entries displayed with formatted date and values.</returns>
        public async Task<List<LogDisplayModel>> AllByUserAsync(string userId)
        {
            return await context.Logs
                    .Where(x => x.UserId == userId)
                    .Select(x => new LogDisplayModel
                    {
                        Id = x.Id,
                        Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                        Type = x.Type,
                        Values = x.Values
                    }).ToListAsync();
        }

        /// <summary>
        /// Deletes a log entry from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the log entry to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var log = new Log() { Id = id };

            context.Remove(log);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates an existing log entry in the database.
        /// </summary>
        /// <param name="model">The model containing the updated details of the log entry.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> EditAsync(LogAddModel model)
        {
            var log = await context.Logs.FindAsync(model.Id);

            if (log is null)
            {
                return false;
            }

            log.Factors = model.Factors;
            log.Type = model.Type;
            log.Values = model.Values;
            log.Note = model.Note;

            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves the details of a log entry for editing by its ID.
        /// </summary>
        /// <param name="id">The ID of the log entry to retrieve for editing.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the log entry details for editing.</returns>
        public async Task<LogAddModel> GetEditAsync(int id)
        {
            var log = await context.Logs.FindAsync(id);

            if (log is null)
            {
                return new LogAddModel();
            }

            return new LogAddModel()
            {
                Id = id,
                Date = log.Date,
                Type = log.Type,
                Note = log.Note,
                Factors = log.Factors,
                Values = log.Values
            };
        }
    }
}
