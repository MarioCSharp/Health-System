using HealthSystem.HealthCare.Data;
using HealthSystem.HealthCare.Data.Models;
using HealthSystem.HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Services.HealthIssueService
{
    /// <summary>
    /// Provides services for managing health issues, including CRUD operations.
    /// </summary>
    public class HealthIssueService : IHealthIssueService
    {
        private readonly HealthCareDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthIssueService"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the health issues data.</param>
        public HealthIssueService(HealthCareDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new health issue to the database.
        /// </summary>
        /// <param name="healthIssueAddModel">The model containing the details of the health issue to add.</param>
        /// <param name="userId">The ID of the user associated with the health issue.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
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

        /// <summary>
        /// Retrieves the details of a specific health issue by its ID.
        /// </summary>
        /// <param name="id">The ID of the health issue to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the details of the health issue.</returns>
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

        /// <summary>
        /// Updates an existing health issue in the database.
        /// </summary>
        /// <param name="healthIssueEditModel">The model containing the updated details of the health issue.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
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

        /// <summary>
        /// Removes a health issue from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the health issue to remove.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        public async Task<bool> RemoveAsync(int id)
        {
            var hI = new HealthIssue() { Id = id };

            context.HealthIssues.Remove(hI);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves a list of health issues associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose health issues are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of health issues displayed with limited description.</returns>
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
