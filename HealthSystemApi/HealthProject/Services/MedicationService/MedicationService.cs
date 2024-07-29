using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace HealthProject.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MedicationService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(MedicationAddModel model)
        {
            CheckInternetConnection();

            try
            {
                string queryString = ToQueryString(model);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Medication/Add{queryString}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return true;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nUnexpected Exception Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return false;
        }

        public async Task<List<MedicationDisplayModel>> AllByUser(string userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Medication/UserMedicaiton?userId={userId}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medications = JsonSerializer.Deserialize<List<MedicationDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return medications;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nUnexpected Exception Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return new List<MedicationDisplayModel>();
        }

        public async Task<MedicationDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Medication/Details?id={id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medication = JsonSerializer.Deserialize<MedicationDetailsModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return medication;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nUnexpected Exception Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return new MedicationDetailsModel();
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        private string ToQueryString(MedicationAddModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }
    }
}
