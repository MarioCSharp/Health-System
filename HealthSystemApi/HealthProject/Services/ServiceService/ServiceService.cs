using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace HealthProject.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ServiceService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task AddAsync(ServiceAddModel model)
        {
            CheckInternetConnection();

            try
            {
                string queryString = ToQueryString(model);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Service/Add{queryString}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
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
        }

        private string ToQueryString(ServiceAddModel model)
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

        public async Task<List<ServiceModel>> AllByIdAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Service/AllById{queryString}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var services = JsonSerializer.Deserialize<List<ServiceModel>>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return services;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new List<ServiceModel>();
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

            return new List<ServiceModel>();
        }

        public async Task<ServiceDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Service/Details{queryString}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var service = JsonSerializer.Deserialize<ServiceDetailsModel>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return service;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new ServiceDetailsModel();
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

            return new ServiceDetailsModel();
        }

        public async Task<List<string>> AvailableHoursAsync(DateTime date, int serviceId)
        {
            CheckInternetConnection();

            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                string queryString = $"?date={formattedDate}&serviceId={serviceId}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Service/AvailableHours{queryString}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var service = JsonSerializer.Deserialize<List<string>>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return service;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new List<string>();
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

            return new List<string>();
        }
    }
}
