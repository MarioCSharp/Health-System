using HealthProject.Models;
using Plugin.LocalNotification;
using System.Diagnostics;
using System.Text.Json;

namespace HealthProject.Services.ReminderService
{
    public class ReminderService : IReminderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ReminderService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(ReminderAddModel model)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}:5256/api/Reminder/Add");

                var token = await SecureStorage.Default.GetAsync("auth_token");

                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var form = new MultipartFormDataContent();

                form.Add(new StringContent(model.Type ?? string.Empty), "Type");
                form.Add(new StringContent(model.Name ?? string.Empty), "Name");
                form.Add(new StringContent(model.RemindTime.ToString("o")), "RemindTime");

                message.Content = form;

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var notification = new NotificationRequest
                    {
                        Description = model.Name,
                        Title = model.Type,
                        NotificationId = 1, 
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = model.RemindTime
                        }
                    };

                    await LocalNotificationCenter.Current.Show(notification);

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

        public async Task<List<ReminderDisplayModel>> AllByUser()
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5256/api/Reminder/AllByUser");

                var token = await SecureStorage.Default.GetAsync("auth_token");

                if (string.IsNullOrEmpty(token))
                {
                    return new List<ReminderDisplayModel>();
                }

                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var reminders = JsonSerializer.Deserialize<List<ReminderDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return reminders ?? new List<ReminderDisplayModel>();
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

            return new List<ReminderDisplayModel>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}:5256/api/Reminder/Remove?id={id}");

                var token = await SecureStorage.Default.GetAsync("auth_token");

                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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
