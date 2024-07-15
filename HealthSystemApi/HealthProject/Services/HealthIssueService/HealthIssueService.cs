using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace HealthProject.Services.HealthIssueService
{
    public class HealthIssueService : IHealthIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HealthIssueService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(HealthIssueAddModel model)
        {
            CheckInternetConnection();

            try
            {
                model.StartDate = model.IssueStartDate.ToString("yyyy-MM-dd");
                model.EndDate = model.IssueEndDate.ToString("yyyy-MM-dd");
                var name = model.Name;
                var description = model.Description;

                string queryString = $"?startDate={model.StartDate}&endDate={model.EndDate}&name={Uri.EscapeDataString(name)}&description={Uri.EscapeDataString(description)}";


                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/HealthIssue/Add{queryString}");
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

        public async Task<List<HealthIssueDisplayModel>> AllByUser()
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/HealthIssue/UserIssues");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var healthIssues = JsonSerializer.Deserialize<List<HealthIssueDisplayModel>>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return healthIssues;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new List<HealthIssueDisplayModel>();
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

            return new List<HealthIssueDisplayModel>();
        }

        public async Task<HealthIssueDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/HealthIssue/Details{queryString}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var hI = JsonSerializer.Deserialize<HealthIssueDetailsModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return hI;
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

            return new HealthIssueDetailsModel();
        }

        public async Task Remove(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/HealthIssue/Remove{queryString}");
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

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        private string ToQueryString(HealthIssueAddModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }
    }
}
