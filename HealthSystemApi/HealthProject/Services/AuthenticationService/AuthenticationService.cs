using HealthProject.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthProject.Services.AuthenticationService
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

        public async Task<AuthenticationModel> IsAuthenticated()
        {
            CheckInternetConnection();

            var token = await SecureStorage.Default.GetAsync("auth_token");

            if (token is null)
            {
                return new AuthenticationModel() { IsAuthenticated = false };
            }

            var response = await _httpClient
                .GetFromJsonAsync<AuthenticationModel>
                    ($"{_url}/Authentication/IsAuthenticated?token={token}",
                     _jsonSerializerOptions);

            if (!response.IsAuthenticated)
            {
                SecureStorage.Default.Remove("auth_token");
            }

            return response;
        }

        public async Task Login(LoginModel loginModel)
        {
            CheckInternetConnection();

            try
            {
                string queryString = ToQueryString(loginModel);

                HttpResponseMessage response = await _httpClient
                    .GetAsync($"{_url}/Authentication/Login{queryString}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<TokenModel>(responseBody, _jsonSerializerOptions);

                await SecureStorage.Default.SetAsync("auth_token", token.Token);
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

        public async Task Register(RegisterModel registerModel)
        {
            CheckInternetConnection();

            try
            {
                string queryString = ToQueryString(registerModel);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Authentication/Register{queryString}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<TokenModel>(responseBody, _jsonSerializerOptions);

                await SecureStorage.Default.SetAsync("auth_token", token.Token);
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

        public async Task<bool> IsAdmin()
        {
            CheckInternetConnection();

            try
            {
                var token = await SecureStorage.Default.GetAsync("auth_token");

                if (token is null)
                {
                    return false;
                }

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Authentication/IsAdmin{token}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var isAdmin = JsonSerializer.Deserialize<bool>(responseBody, _jsonSerializerOptions);

                return isAdmin;
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

        public async Task Logout()
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");

            if (token is null)
            {
                return;
            }

            SecureStorage.Default.Remove("auth_token");
        }

        private string ToQueryString(RegisterModel registerModel)
        {
            var properties = from p in registerModel.GetType().GetProperties()
                             where p.GetValue(registerModel, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(registerModel, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        private string ToQueryString(LoginModel registerModel)
        {
            var properties = from p in registerModel.GetType().GetProperties()
                             where p.GetValue(registerModel, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(registerModel, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }
    }
}
