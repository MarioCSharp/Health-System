using HealthProject.Models;
using System.Diagnostics;
using System.Net;
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

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";

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

                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5098/api/Problem/Add{queryString}&userId={userId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return true;
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

            return false;
        }

        public async Task DeleteAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5098/api/Problem/Remove?id={id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }

        public async Task<ProblemDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5098/api/Problem/Details?id={id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var problem = JsonSerializer.Deserialize<ProblemDetailsModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return problem ?? new ProblemDetailsModel();
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

            return new ProblemDetailsModel();
        }

        public async Task<List<ProblemDisplayModel>> GetUserProblems(string? userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5098/api/Problem/UserIssues?userId={userId}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var problem = JsonSerializer.Deserialize<List<ProblemDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return problem ?? new List<ProblemDisplayModel>();
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

            return new List<ProblemDisplayModel>();
        }

        public async Task<List<SymptomCategoryDisplayModel>> GetSymptomsCategories()
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5098/api/Problem/GetSymptomCategories");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<SymptomCategoryDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return categories ?? new List<SymptomCategoryDisplayModel>();
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

            return new List<SymptomCategoryDisplayModel>();
        }

        public async Task<List<SymptomSubCategoryDisplayModel>> GetSymptomsSubCategories()
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5098/api/Problem/GetSymptomSubCategories");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var subCategories = JsonSerializer.Deserialize<List<SymptomSubCategoryDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return subCategories ?? new List<SymptomSubCategoryDisplayModel>();
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

            return new List<SymptomSubCategoryDisplayModel>();
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        private string ToQueryString(object model)
        {
            var properties = from p in model.GetType().GetProperties()
                             let value = p.GetValue(model, null)
                             where value != null
                             from param in (value is System.Collections.IEnumerable && !(value is string)
                                            ? (value as System.Collections.IEnumerable).Cast<object>()
                                            : new List<object> { value })
                             select p.Name + "=" + WebUtility.UrlEncode(param.ToString());

            return string.Join("&", properties.ToArray());
        }

        public string ToQueryString(ProblemAddModel problemAddModel, SymptomAddModel symptomAddModel)
        {
            var problemQueryString = ToQueryString(problemAddModel);
            var symptomQueryString = ToQueryString(symptomAddModel);

            var combinedQueryString = problemQueryString;
            if (!string.IsNullOrEmpty(symptomQueryString))
            {
                combinedQueryString += "&" + symptomQueryString;
            }

            if (!string.IsNullOrEmpty(combinedQueryString))
            {
                combinedQueryString = "?" + combinedQueryString;
            }

            Console.WriteLine($"Combined Query String: {combinedQueryString}");
            return combinedQueryString;
        }
    }
}
