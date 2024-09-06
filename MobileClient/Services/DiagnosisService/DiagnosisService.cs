using HealthProject.Models;
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

        public async Task<PredictionModel> GetPrediction(List<string> symptoms)
        {
            CheckInternetConnection();

            try
            {
                var requestBody = new { symptoms = symptoms };
                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseAddress}:8000/predict", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var predictions = JsonConvert.DeserializeObject<List<PredictionResponse>>(responseContent);

                    var sb = new StringBuilder();

                    var count = 1;

                    var neededDoctors = new Dictionary<string, int>();

                    foreach (var prediction in predictions)
                    {
                        if (prediction.Probability < 0.2)
                        {
                            continue;
                        }

                        sb.AppendLine($"---Прогноза {count++}---");
                        sb.AppendLine($"Диагноза: {prediction.Diagnosis}\n Предпазване: {prediction.Prevention}\n Точност: {prediction.Probability * 100}%");

                        if (!neededDoctors.ContainsKey(prediction.DoctorSpecialization))
                        {
                            neededDoctors[prediction.DoctorSpecialization] = 1;
                        }
                        else
                        {
                            neededDoctors[prediction.DoctorSpecialization]++; 
                        }
                    }

                    var recommendedDoctors = new List<DoctorModel>();

                    foreach (var doc in neededDoctors)
                    {
                        response = await _httpClient.GetAsync($"{_baseAddress}:5025/api/Doctor/GetTopDoctorsWithSpecialization?specialization={doc.Key}&top={doc.Value}");

                        if (!response.IsSuccessStatusCode)
                        {
                            continue;
                        }

                        var responsesDoctors = JsonConvert.DeserializeObject<List<DoctorModel>>(responseContent);

                        recommendedDoctors.AddRange(responsesDoctors);
                    }

                    return new PredictionModel()
                    {
                        Prediction = sb.ToString(),
                        RecommendedDoctors = recommendedDoctors
                    };
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

            return new PredictionModel();
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
            public string? DoctorSpecialization { get; set; }
        }
    }
}
