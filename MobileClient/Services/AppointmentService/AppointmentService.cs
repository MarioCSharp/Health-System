using HealthProject.Models;
using System.Diagnostics;
using System.Text.Json;

namespace HealthProject.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AppointmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "https://localhost";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<AppointmentModel>> GetUserAppointmentsAsync(string userId)
        {
            CheckInternetConnection();

            try
            {
                string url = $"{_baseAddress}:5046/api/Service/AllByUser?userId={userId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<AppointmentModel>>(responseBody, _jsonSerializerOptions);

                foreach (var app in appointments)
                {
                    url = $"{_baseAddress}:5025/api/Doctor/HasRating?appointmentId={app.Id}";

                    response = await _httpClient.GetAsync(url);

                    responseBody = await response.Content.ReadAsStringAsync();
                    var hasRating = JsonSerializer.Deserialize<bool>(responseBody, _jsonSerializerOptions);

                    app.HasRating = hasRating;

                    var parsed = DateTime.TryParseExact(app.Date, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime date);

                    if (date < DateTime.Now)
                    {
                        app.IsPast = true;
                    }

                    if (app.HasRating || !app.IsPast)
                    {
                        app.IsVisible = false;
                    }
                    else
                    {
                        app.IsVisible = true;
                    }
                }

                return appointments ?? new List<AppointmentModel>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return new List<AppointmentModel>();
        }

        public async Task<List<PrescriptionDisplayModel>> GetUserPrescriptions(string userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5046/api/Appointment/GetUserPrescriptions?userId={userId}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var prescriptions = JsonSerializer.Deserialize<List<PrescriptionDisplayModel>>(responseBody, _jsonSerializerOptions);

                return prescriptions ?? new List<PrescriptionDisplayModel>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return new List<PrescriptionDisplayModel>();
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
