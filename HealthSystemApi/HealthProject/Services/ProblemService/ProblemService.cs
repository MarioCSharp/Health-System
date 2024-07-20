using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace HealthProject.Services.ProblemService
{
    public class ProblemService : IProblemService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ProblemService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(ProblemAddModel model, List<int> symptomsIds, string userId)
        {
            CheckInternetConnection();

            try
            {
                var sAM = new SymptomAddModel() { SymptomIds = symptomsIds };

                var queryString = ToQueryString(model, sAM);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Problem/Add{queryString}&userId={userId}");
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

        public async Task DeleteAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Problem/Remove{queryString}");
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

        public async Task<ProblemDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Problem/Details{queryString}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var problem = JsonSerializer.Deserialize<ProblemDetailsModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return problem;
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

            return new ProblemDetailsModel();
        }

        public async Task<List<ProblemDisplayModel>> GetUserProblems(string? userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Problem/UserIssues?userId={userId}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var problem = JsonSerializer.Deserialize<List<ProblemDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return problem;
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

            return new List<ProblemDisplayModel>();
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        private string ToQueryString(ProblemAddModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        private string ToQueryString(SymptomAddModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        public string ToQueryString(ProblemAddModel problemAddModel, SymptomAddModel symptomAddModel)
        {
            var problemQueryString = ToQueryString(problemAddModel);
            var symptomQueryString = ToQueryString(symptomAddModel);

            return "?" + problemQueryString + "&" + symptomQueryString;
        }

        public async Task<List<SymptomCategoryDisplayModel>> GetSymptomsCategories()
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Problem/GetSymptomCategories");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<SymptomCategoryDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return categories;
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

            return new List<SymptomCategoryDisplayModel>();
        }

        public async Task<List<SymptomSubCategoryDisplayModel>> GetSymptomsSubCategories()
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Problem/GetSymptomSubCategories");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var subCategories = JsonSerializer.Deserialize<List<SymptomSubCategoryDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return subCategories;
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

            return new List<SymptomSubCategoryDisplayModel>();
        }
    }
}
