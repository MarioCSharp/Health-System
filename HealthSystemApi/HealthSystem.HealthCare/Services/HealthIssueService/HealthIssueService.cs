using HealthSystem.HealthCare.Data;
using HealthSystem.HealthCare.Data.Models;
using HealthSystem.HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Services.HealthIssueService
{
    public class HealthIssueService : IHealthIssueService
    {
        private HealthCareDbContext context;

        public HealthIssueService(HealthCareDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(HealthIssueAddModel healthIssueAddModel, string userId)
        {
            var hI = new HealthIssue()
            {
                Name = healthIssueAddModel.Name,
                Description = healthIssueAddModel.Description,
                IssueStartDate = healthIssueAddModel.IssueStartDate,
                IssueEndDate = healthIssueAddModel.IssueEndDate,
                UserId = userId
            };

            await context.HealthIssues.AddAsync(hI);
            await context.SaveChangesAsync();

            return await context.HealthIssues.ContainsAsync(hI);
        }

        public async Task<HealthIssueDetailsModel> DetailsAsync(int id)
        {
            var hI = await context.HealthIssues.FindAsync(id);

            if (hI == null)
                return new HealthIssueDetailsModel();

            return new HealthIssueDetailsModel
            {
                Id = id,
                Name = hI.Name,
                Description = hI.Description,
                IssueStartDate = hI.IssueStartDate,
                IssueEndDate = hI.IssueEndDate
            };
        }

        public async Task<bool> EditAsync(HealthIssueEditModel healthIssueEditModel)
        {
            var hI = await context.HealthIssues.FindAsync(healthIssueEditModel.Id);

            if (hI == null) return false;

            hI.Name = healthIssueEditModel.Name;
            hI.Description = healthIssueEditModel.Description;
            hI.IssueStartDate = healthIssueEditModel.IssueStartDate;
            hI.IssueEndDate = healthIssueEditModel.IssueEndDate;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var hI = await context.HealthIssues.FindAsync(id);

            if (hI == null) return false;

            context.HealthIssues.Remove(hI);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<HealthIssueDisplayModel>> UserIssuesAsync(string userId)
        {
            return await context.HealthIssues
                    .Where(x => x.UserId == userId)
                    .Select(x => new HealthIssueDisplayModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortDescription = string.IsNullOrEmpty(x.Description) ? "" : x.Description.Substring(0, 20)
                    })
                    .ToListAsync();
        }
    }
}
