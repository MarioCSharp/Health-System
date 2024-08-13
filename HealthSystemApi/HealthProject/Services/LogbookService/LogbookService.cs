using HealthProject.Models;
using Newtonsoft.Json;
using System.Diagnostics;
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
            CheckInternetConnection();

            var form = new MultipartFormDataContent();

            for (int i = 0; i < model.Values.Count; i++)
            {
                form.Add(new StringContent(model.Values[i].ToString()), $"Values[{i}]");
            }

            for (int i = 0; i < model.Factors.Count; i++)
            {
                form.Add(new StringContent(model.Factors[i]), $"Factors[{i}]");
            }

            form.Add(new StringContent(model.Type ?? ""), "Type");
            form.Add(new StringContent(model.HealthIssueId.ToString()), "HealthIssueId");
            form.Add(new StringContent(model.Note ?? ""), "Note");
            form.Add(new StringContent(model.UserId ?? ""), "UserId");
            form.Add(new StringContent(model.Date.ToString("dd/MM/yyyy HH:mm")), "Date");

            var response = await _httpClient.PostAsync($"{_url}/Logbook/Add", form);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(resultContent);

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            CheckInternetConnection();

            var response = await _httpClient.GetAsync($"{_url}/Logbook/Remove?id={id}");
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(resultContent);

            return result;
        }

        public async Task<bool> EditAsync(LogAddModel model)
        {
            CheckInternetConnection();

            var form = new MultipartFormDataContent();

            for (int i = 0; i < model.Values.Count; i++)
            {
                form.Add(new StringContent(model.Values.ToString()), $"Values[{i}]");
            }

            for (int i = 0; i < model.Factors.Count; i++)
            {
                form.Add(new StringContent(model.Factors.ToString()), $"Factors[{i}]");
            }

            form.Add(new StringContent(model.Type), "Type");
            form.Add(new StringContent(model.HealthIssueId.ToString()), "HealthIssueId");
            form.Add(new StringContent(model.HealthIssueId.ToString()), "HealthIssueId");
            form.Add(new StringContent(model.Note), "Note");
            form.Add(new StringContent(model.UserId), "UserId");
            form.Add(new StringContent(model.Date.ToString("dd/MM/yyyy mm:HH")), "Date");

            var response = await _httpClient.PostAsync($"{_url}/Logbook/Edit", form);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(resultContent);

            return result;
        }

        public async Task<List<LogDisplayModel>> GetByUser(string userId)
        {
            CheckInternetConnection();

            var response = await _httpClient.GetAsync($"{_url}/Logbook/AllByUser?userId={userId}");
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<LogDisplayModel>>(resultContent);

            return result ?? new List<LogDisplayModel>();
        }

        public async Task<LogAddModel> GetEditAsync(int id)
        {
            CheckInternetConnection();

            var response = await _httpClient.GetAsync($"{_url}/Logbook/Edit?id={id}");
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LogAddModel>(resultContent);

            return result ?? new LogAddModel();
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }
    }
}
