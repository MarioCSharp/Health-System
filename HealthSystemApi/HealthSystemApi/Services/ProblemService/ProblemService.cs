using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Problem;
using HealthSystemApi.Models.Symptom;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.ProblemService
{
    public class ProblemService : IProblemService
    {
        private ApplicationDbContext context;

        public ProblemService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(ProblemAddModel problemAddModel, List<int> symptoms, string? userId)
        {
            var problem = new Problem()
            {
                Notes = problemAddModel.Notes,
                Date = problemAddModel.Date,
                UserId = userId
            };

            if (problemAddModel.HealthIssueId != 0)
            {
                problem.HealthIssueId = problemAddModel.HealthIssueId;
            }

            foreach (var id in symptoms) 
            {
                var symptom = await context.Symptoms.FindAsync(id);
                problem.Symptoms.Add(symptom);
            }

            await context.Problems.AddAsync(problem);
            await context.SaveChangesAsync();

            return await context.Problems.ContainsAsync(problem);
        }

        public async Task<ProblemDetailsModel> DetailsAsync(int id)
        {
            var problem = await context.Problems
                .Include(p => p.Symptoms)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (problem == null)
            {
                return new ProblemDetailsModel();
            }

            return new ProblemDetailsModel()
            {
                Date = problem.Date,
                Notes = problem.Notes,
                HealthIssueId = problem.HealthIssueId,
                Symptoms = problem.Symptoms.Select(s => new SymptomModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            };
        }

        public async Task<bool> EditAsync(ProblemEditModel problemEditModel, List<int> symptoms)
        {
            var problem = await context.Problems
                .Include(p => p.Symptoms)
                .FirstOrDefaultAsync(p => p.Id == problemEditModel.Id);

            if (problem == null) return false;

            problem.Notes = problemEditModel.Notes;
            problem.Symptoms.Clear();

            foreach (var id in symptoms)
            {
                var symptom = await context.Symptoms.FindAsync(id);
                problem.Symptoms.Add(symptom);
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var problem = await context.Problems.FindAsync(id);

            if (problem is null) return false;

            context.Problems.Remove(problem);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProblemDisplayModel>> UserProblemsAsync(string? userId)
        {
            var problems = await context.Problems.Where(x => x.UserId == userId).Include(x => x.Symptoms).ToListAsync();

            return problems.Select(x => new ProblemDisplayModel
            {
                Notes = x.Notes,
                Date = x.Date,
                Symptoms = x.Symptoms.Select(y => new SymptomModel()
                    {
                        Id = y.Id,
                        Name = y.Name
                    }).ToList()
            }).ToList();
        }
    }
}
