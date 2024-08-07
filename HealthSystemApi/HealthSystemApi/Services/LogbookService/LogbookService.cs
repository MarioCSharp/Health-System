using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Logbook;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HealthSystemApi.Services.LogbookService
{
    public class LogbookService : ILogbookService
    {
        private ApplicationDbContext context;

        public LogbookService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(LogAddModel model)
        {
            var log = new Log
            {
                Type = model.Type,
                Date = model.Date,
                Factors = model.Factors,
                HealthIssueId = model.HealthIssueId,
                Note = model.Note,
                UserId = model.UserId,
                Values = model.Values
            };

            await context.Logs.AddAsync(log);
            await context.SaveChangesAsync();

            return await context.Logs.ContainsAsync(log);
        }

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

        public async Task<bool> DeleteAsync(int id)
        {
            var log = await context.Logs.FindAsync(id);

            if (log is null)
            {
                return false;   
            }

            context.Remove(log);
            await context.SaveChangesAsync();

            return await context.Logs.ContainsAsync(log);
        }

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
            log.HealthIssueId = model.HealthIssueId;

            await context.SaveChangesAsync();

            return true;
        }

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
                HealthIssueId = log.HealthIssueId,
                Values = log.Values
            };
        }
    }
}
