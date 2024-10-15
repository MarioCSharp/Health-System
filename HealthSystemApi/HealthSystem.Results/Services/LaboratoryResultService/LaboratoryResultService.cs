using HealthSystem.Results.Data;
using HealthSystem.Results.Data.Models;
using HealthSystem.Results.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using QRCoder;

namespace HealthSystem.Results.Services.LaboratoryResultService
{
    /// <summary>
    /// Service for handling laboratory results including file management, result issuing, and fetching doctor-related results.
    /// </summary>
    public class LaboratoryResultService : ILaboratoryResultService
    {
        private readonly ResultsDbContext context;
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboratoryResultService"/> class.
        /// </summary>
        /// <param name="context">Database context for laboratory results.</param>
        public LaboratoryResultService(ResultsDbContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        /// <summary>
        /// Adds a file to an existing laboratory result.
        /// </summary>
        /// <param name="resultId">ID of the laboratory result.</param>
        /// <param name="file">The file to be added.</param>
        /// <returns>Returns true if the file was successfully added; otherwise, false.</returns>
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
                result.File = memoryStream.ToArray(); // Save file as byte array
            }

            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a file associated with a laboratory result based on user login ID and password.
        /// </summary>
        /// <param name="id">User login ID.</param>
        /// <param name="pass">User login password.</param>
        /// <returns>The file as a byte array, or null if not found.</returns>
        public async Task<byte[]> GetFileAsync(string id, string pass)
        {
            var result = await context.LaboratoryResults.FirstOrDefaultAsync(x => x.UserLogingName == id && x.UserLogingPass == pass);

            return result?.File; // Return file or null if not found
        }

        /// <summary>
        /// Retrieves a list of laboratory results for a specific doctor.
        /// </summary>
        /// <param name="doctorUserId">Doctor's user ID.</param>
        /// <returns>A list of laboratory results as display models.</returns>
        public async Task<List<LaboratoryResultDisplayModel>> GetResults(string doctorUserId)
        {
            return await context.LaboratoryResults
                .Where(x => x.DoctorUserId == doctorUserId)
                .Select(x => new LaboratoryResultDisplayModel()
                {
                    Id = x.Id,
                    PatientName = x.PatientName,
                    Date = x.DateAdded.ToString("dd/MM/yyyy HH:mm") // Date formatted for display
                })
                .ToListAsync();
        }

        /// <summary>
        /// Issues a new laboratory result for a patient, generating login credentials for the result.
        /// </summary>
        /// <param name="model">The model containing result details.</param>
        /// <param name="userId">The ID of the doctor issuing the result.</param>
        /// <returns>A tuple containing a success flag, the generated username, and the generated password.</returns>
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

            result.QR = await GenerateQRCode(result.UserLogingName, result.UserLogingPass);

            await context.LaboratoryResults.AddAsync(result);
            await context.SaveChangesAsync();

            return (await context.LaboratoryResults.ContainsAsync(result), result.UserLogingName, result.UserLogingPass);
        }

        /// <summary>
        /// Generates a random username with the specified length.
        /// </summary>
        /// <param name="length">The length of the generated username.</param>
        /// <returns>A randomly generated username.</returns>
        public string GenerateUsername(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var username = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                username.Append(chars[random.Next(chars.Length)]);
            }

            return username.ToString();
        }

        /// <summary>
        /// Generates a random password with the specified length, including special characters.
        /// </summary>
        /// <param name="length">The length of the generated password.</param>
        /// <returns>A randomly generated password with special characters.</returns>
        public string GeneratePassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            var password = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        /// <summary>
        /// Generates QR code for the laboratory result.
        /// </summary>
        /// <param name="id">The ID which the QR code will be generated based on.</param>
        /// <param name="password">The password which the QR code will be generated based on.</param>
        /// <returns>A randomly generated QR code.</returns>
        
        public async Task<byte[]> GenerateQRCode(string id, string password)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"http://localhost:5250/api/LaboratoryResult/GetResultDetails?code={id}-{password}", QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(30);

                return qrCodeImage;
            }
        }
    }
}
