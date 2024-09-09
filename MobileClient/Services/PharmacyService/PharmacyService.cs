using HealthProject.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HealthProject.Services.PharmacyService
{
    public class PharmacyService : IPharmacyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public PharmacyService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddToCart(int medcicationId, int userCartId, int quantity)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}:5171/api/Cart/AddToCart");

                var form = new MultipartFormDataContent();
                var token = await SecureStorage.GetAsync("auth_token");

                form.Add(new StringContent(medcicationId.ToString()), "MedicationId");
                form.Add(new StringContent(userCartId.ToString()), "UserCartId");
                form.Add(new StringContent(quantity.ToString()), "Quantity");

                message.Content = form;
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<bool>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return result;
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

        public async Task<List<PharmacyDisplayModel>> GetAll()
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5171/api/Pharmacy/All");

                var token = await SecureStorage.GetAsync("auth_token");

                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var pharmacies = JsonSerializer.Deserialize<List<PharmacyDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return pharmacies ?? new List<PharmacyDisplayModel>();
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

            return new List<PharmacyDisplayModel>();
        }

        public async Task<List<PharmacyProductDisplayModel>> GetAllProducts(int pharmacyId)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5171/api/Medication/AllInPharmacy?pharmacyId={pharmacyId}");

                var token = await SecureStorage.GetAsync("auth_token");

                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medications = JsonSerializer.Deserialize<List<PharmacyProductDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return medications ?? new List<PharmacyProductDisplayModel>();
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

            return new List<PharmacyProductDisplayModel>();
        }

        public async Task<bool> GetMedicationsByEGNAsync(string EGN, int cartId)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5171/api/Order/GetOrderByEGN?EGN={EGN}&userCartId={cartId}");

                var token = await SecureStorage.GetAsync("auth_token");

                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<bool>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return result;
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

        public async Task<CartModel> GetUserCart(int pharmacyId)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5171/api/Cart/GetUserCart?pharmacyId={pharmacyId}");

                var token = await SecureStorage.GetAsync("auth_token");

                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medications = JsonSerializer.Deserialize<CartModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return medications ?? new CartModel();
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

            return new CartModel();
        }

        public async Task<bool> RemoveFromCart(int cartItemId)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5171/api/Cart/RemoveFromCart?cartItemId={cartItemId}");

                var token = await SecureStorage.GetAsync("auth_token");

                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medications = JsonSerializer.Deserialize<bool>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return medications;
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

        public async Task<bool> SubmitOrder(SubmitOrderModel model)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}:5171/api/Order/SubmitOrder");

                var form = new MultipartFormDataContent();
                var token = await SecureStorage.GetAsync("auth_token");

                form.Add(new StringContent(model.Name ?? "Unknown"), "Name");
                form.Add(new StringContent(model.Location ?? "Unknown"), "Location");
                form.Add(new StringContent(model.PhoneNumber ?? "Unknown"), "PhoneNumber");
                form.Add(new StringContent(model.CartId.ToString() ?? "Unknown"), "CartId");

                message.Content = form;
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<bool>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return result;
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

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }
    }
}
