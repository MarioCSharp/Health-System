using HealthProject.Models;
using System.Text.Json;

namespace HealthProject.Services.LogbookService
{
    public class LogbookService : ILogbookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public LogbookService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(LogAddModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditAsync(LogAddModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LogDisplayModel>> GetByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<LogAddModel> GetEditAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
