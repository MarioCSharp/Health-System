using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HealthProject.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task Register(RegisterModel registerModel)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }

            try
            {
                string queryString = ToQueryString(registerModel);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Authentication/Register{queryString}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

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
        private string ToQueryString(RegisterModel registerModel)
        {
            var properties = from p in registerModel.GetType().GetProperties()
                             where p.GetValue(registerModel, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(registerModel, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }
    }
}
