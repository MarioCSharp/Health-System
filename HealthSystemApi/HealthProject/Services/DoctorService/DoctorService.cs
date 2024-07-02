using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace HealthProject.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DoctorService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddDoctorAsync(AddDoctorModel doctorModel)
        {
            CheckInternetConnection();

            try
            {
                string queryString = ToQueryString(doctorModel);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Doctor/Add{queryString}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return true;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return false;
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

        public async Task<List<DoctorModel>> AllAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Doctor/All{queryString}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var doctors = JsonSerializer.Deserialize<List<DoctorModel>>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return doctors;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new List<DoctorModel>();
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

            return new List<DoctorModel>();
        }

        public async Task<DoctorDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Doctor/Details{queryString}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var doctor = JsonSerializer.Deserialize<DoctorDetailsModel>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return doctor;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new DoctorDetailsModel();
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

            return new DoctorDetailsModel();
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        private string ToQueryString(AddDoctorModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }
    }
}
