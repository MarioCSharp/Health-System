using HealthSystem.Results.Data;
using HealthSystem.Results.Data.Models;
using HealthSystem.Results.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HealthSystem.Results.Services.LaboratoryResultService
{
    public class LaboratoryResultService : ILaboratoryResultService
    {
        private ResultsDbContext context;
        private Random random;

        public LaboratoryResultService(ResultsDbContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        public async Task<bool> AddFileAsync(int resultId, IFormFile file)
        {
            var result = await context.LaboratoryResults.FindAsync(resultId);

            if (result == null || file == null || file.Length == 0)
            {
                return false;
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                result.File = memoryStream.ToArray();
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<byte[]> GetFileAsync(string id, string pass)
        {
            var result = await context.LaboratoryResults.FirstOrDefaultAsync(x => x.UserLogingName == id && x.UserLogingPass == pass);

            if (result is null)
            {
                return null;
            }

            return result.File;
        }

        public async Task<List<LaboratoryResultDisplayModel>> GetResults(string doctorUserId)
        {
            return await context.LaboratoryResults
                .Where(x => x.DoctorUserId == doctorUserId)
                .Select(x => new LaboratoryResultDisplayModel()
                {
                    Id = x.Id,
                    PatientName = x.PatientName,
                    Date = x.DateAdded.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();
        }

        public async Task<(bool, string, string)> IssueResultAsync(IssueResultModel model, string userId)
        {
            var result = new LaboratoryResult()
            {
                DateAdded = DateTime.Now,
                DoctorUserId = userId,
                PatientName = model.PatientName,
                UserLogingName = GenerateUsername(8),
                UserLogingPass = GeneratePassword(10)
            };

            await context.LaboratoryResults.AddAsync(result);
            await context.SaveChangesAsync();

            return (await context.LaboratoryResults.ContainsAsync(result), result.UserLogingName, result.UserLogingPass);
        }

        public string GenerateUsername(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder username = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                username.Append(chars[random.Next(chars.Length)]);
            }

            return username.ToString();
        }

        public string GeneratePassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            StringBuilder password = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }
    }
}
