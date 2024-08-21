using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace HealthProject.Services.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HospitalService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "https://localhost";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<HospitalModel>> All()
        {
            CheckInternetConnection();

            try
            {
                var response = await _httpClient.GetAsync($"{_baseAddress}:5025/api/Hospital/All");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var wrapper = JsonSerializer.Deserialize<HospitalWrapper>(jsonResponse, _jsonSerializerOptions);

                return wrapper?.Hospitals ?? new List<HospitalModel>();

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General exception: {ex.Message}");
            }

            return new List<HospitalModel>();
        }

        public async Task<HospitalDetailsModel> Details(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5025/api/Hospital/Details?id={id}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var hospital = JsonSerializer.Deserialize<HospitalDetailsModel>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return hospital ?? new HospitalDetailsModel();
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

            return new HospitalDetailsModel();
        }

        private string ToQueryString(AddHospitalModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        public class HospitalWrapper
        {
            public List<HospitalModel> Hospitals { get; set; } = new List<HospitalModel>();
        }
    }
}
