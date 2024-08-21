using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace HealthProject.Services.DiagnosisService
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DiagnosisService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "https://localhost";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<string> GetPrediction(List<string> symptoms)
        {
            CheckInternetConnection();

            try
            {
                var requestBody = new { symptoms = symptoms };
                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseAddress}:8080/predict", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var predictions = JsonConvert.DeserializeObject<List<PredictionResponse>>(responseContent);

                    var sb = new StringBuilder();

                    foreach (var prediction in predictions)
                    {
                        sb.AppendLine($"Диагноза: {prediction.Diagnosis}, Предпазване: {prediction.Prevention}, Точност: {prediction.Probability * 100}%");
                    }

                    return sb.ToString();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            return string.Empty;
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        public class PredictionResponse
        {
            public string? Diagnosis { get; set; }
            public string? Prevention { get; set; }
            public float Probability { get; set; }
        }
    }
}
