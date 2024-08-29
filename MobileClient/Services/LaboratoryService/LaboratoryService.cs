using HealthProject.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HealthProject.Services.LaboratoryService
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public LaboratoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "https://localhost";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<LaboratoryReturnModel> CheckResult(string userNameId, string pass)
        {
            CheckInternetConnection();

            try
            {
                var token = await SecureStorage.Default.GetAsync("auth_token");

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Authorization token is missing.");
                    return new LaboratoryReturnModel();
                }

                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5250/api/LaboratoryResult/TryGetFile?id={userNameId}&pass={pass}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<byte[]>(responseBody, _jsonSerializerOptions);

                var model = new LaboratoryReturnModel();
                model.File = responseData;

                if (response.IsSuccessStatusCode)
                {
                    return model ?? new LaboratoryReturnModel();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return new LaboratoryReturnModel();
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
