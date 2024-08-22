using HealthProject.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Plugin.LocalNotification;

namespace HealthProject.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MedicationService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(MedicationAddModel model)
        {
            CheckInternetConnection();

            var form = new MultipartFormDataContent();

            for (int i = 0; i < model.Days.Count; i++)
            {
                var stringContent = new StringContent(model.Days[i].ToString());
                form.Add(stringContent, $"Days[{i}]");
            }

            for (int i = 0; i < model.Times.Count; i++)
            {
                var stringContent = new StringContent(model.Times[i].ToString());
                form.Add(stringContent, $"Times[{i}]");
            }

            form.Add(new StringContent(model.Type ?? string.Empty), "Type");
            form.Add(new StringContent(model.Name ?? string.Empty), "Name");
            form.Add(new StringContent(model.Dose.ToString()), "Dose");
            form.Add(new StringContent(model.Note ?? string.Empty), "Note");
            form.Add(new StringContent(model.StartDate.ToString("o")), "StartDate");
            form.Add(new StringContent(model.EndDate.ToString("o")), "EndDate");
            form.Add(new StringContent(model.Take.ToString()), "Take");
            form.Add(new StringContent(model.SkipCount.ToString()), "SkipCount");
            form.Add(new StringContent(model.Rest.ToString()), "Rest");
            form.Add(new StringContent(model.HealthIssueId.ToString()), "HealthIssueId");
            form.Add(new StringContent(model.UserId ?? string.Empty), "UserId");

            var response = await _httpClient.PostAsync($"{_baseAddress}:5115/api/Medication/Add", form);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(resultContent);

            return result;
        }

        public async Task<List<MedicationDisplayModel>> AllByUser(string userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/Medication/UserMedicaiton?userId={userId}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medications = System.Text.Json.JsonSerializer.Deserialize<List<MedicationDisplayModel>>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return medications ?? new List<MedicationDisplayModel>();
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

            return new List<MedicationDisplayModel>();
        }

        public async Task DeleteAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/Medication/Remove?id={id}");
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

        public async Task<MedicationDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/Medication/Details?id={id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var medication = System.Text.Json.JsonSerializer.Deserialize<MedicationDetailsModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return medication ?? new MedicationDetailsModel();
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

            return new MedicationDetailsModel();
        }

        public async Task<List<MedicationScheduleModel>> SchedulesAsync(string userId)
        {
            CheckInternetConnection();

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/Medication/UserSchedule?userId={userId}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var medication = System.Text.Json.JsonSerializer.Deserialize<List<MedicationScheduleModel>>(responseBody, _jsonSerializerOptions);

            return medication ?? new List<MedicationScheduleModel>();
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("--- No internet access");
            }
        }

        private string ToQueryString(MedicationAddModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        private void ScheduleNotifications(MedicationAddModel model)
        {
            if (model.Type == "Daily")
            {
                foreach (var time in model.Times)
                {
                    ScheduleDailyNotification(model, time);
                }
            }
            else if (model.Type == "SpecificDayOfWeek")
            {
                foreach (var day in model.Days)
                {
                    foreach (var time in model.Times)
                    {
                        ScheduleWeeklyNotification(model, day, time);
                    }
                }
            }
            else if (model.Type == "TakeXThenRestY")
            {
                ScheduleTakeXThenRestYNotification(model);
            }
            else if (model.Type == "EveryXDays")
            {
                ScheduleEveryXDaysNotification(model);
            }
        }

        private void ScheduleDailyNotification(MedicationAddModel model, TimeSpan time)
        {
            var notificationTime = DateTime.Today.Add(time);
            if (notificationTime <= model.EndDate)
            {
                var notificationId = GenerateUniqueNotificationId(model, time);

                var notification = new NotificationRequest
                {
                    NotificationId = notificationId,
                    Title = "Напомняне за лекарство",
                    Description = $"Време е да вземеш лекарство: {model.Name} с доза {model.Dose} {model.Type}",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = notificationTime,
                        RepeatType = NotificationRepeat.Daily,
                        NotifyRepeatInterval = model.EndDate.Subtract(DateTime.Today).TotalDays > 0
                            ? TimeSpan.FromDays(1)
                            : (TimeSpan?)null
                    },
                    ReturningData = $"{model.UserId}|{model.Name}|{time}"
                };

                LocalNotificationCenter.Current.Show(notification);
            }
        }

        private void ScheduleWeeklyNotification(MedicationAddModel model, DayOfWeek day, TimeSpan time)
        {
            var notifyTime = DateTime.Today.AddDays(((int)day - (int)DateTime.Today.DayOfWeek + 7) % 7).Add(time);
            if (notifyTime <= model.EndDate)
            {
                var notificationId = GenerateUniqueNotificationId(model, time, day);

                var notification = new NotificationRequest
                {
                    NotificationId = notificationId,
                    Title = "Напомняне за лекарство",
                    Description = $"Време е да вземеш лекарство: {model.Name} с доза {model.Dose} {model.Type}",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = notifyTime,
                        RepeatType = NotificationRepeat.Weekly,
                        NotifyRepeatInterval = model.EndDate.Subtract(notifyTime).TotalDays > 0
                            ? TimeSpan.FromDays(7)
                            : (TimeSpan?)null
                    },
                    ReturningData = $"{model.UserId}|{model.Name}|{day}|{time}"
                };

                LocalNotificationCenter.Current.Show(notification);
            }
        }

        private void ScheduleTakeXThenRestYNotification(MedicationAddModel model)
        {
            var startDate = model.StartDate;
            var notifyDate = startDate;

            for (int i = 0; i < model.Take; i++)
            {
                if (notifyDate > model.EndDate) break;

                foreach (var time in model.Times)
                {
                    var notificationId = GenerateUniqueNotificationId(model, time, null, notifyDate);

                    var notification = new NotificationRequest
                    {
                        NotificationId = notificationId,
                        Title = "Напомняне за лекарство",
                        Description = $"Време е да вземеш лекарство: {model.Name} с доза {model.Dose} {model.Type}",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = notifyDate.Add(time),
                            RepeatType = NotificationRepeat.No
                        },
                        ReturningData = $"{model.UserId}|{model.Name}|{notifyDate}|{time}"
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }
                notifyDate = notifyDate.AddDays(1);
            }

            notifyDate = notifyDate.AddDays(model.Rest);
        }

        private void ScheduleEveryXDaysNotification(MedicationAddModel model)
        {
            var notifyDate = model.StartDate;

            while (notifyDate <= model.EndDate)
            {
                foreach (var time in model.Times)
                {
                    if (notifyDate > model.EndDate) break;

                    var notificationId = GenerateUniqueNotificationId(model, time, null, notifyDate);

                    var notification = new NotificationRequest
                    {
                        NotificationId = notificationId,
                        Title = "Напомняне за лекарство",
                        Description = $"Време е да вземеш лекарство: {model.Name} с доза {model.Dose} {model.Type}",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = notifyDate.Add(time),
                            RepeatType = NotificationRepeat.No
                        },
                        ReturningData = $"{model.UserId}|{model.Name}|{notifyDate}|{time}"
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }

                notifyDate = notifyDate.AddDays(model.SkipCount);
            }
        }

        private int GenerateUniqueNotificationId(MedicationAddModel model, TimeSpan time, DayOfWeek? day = null, DateTime? notifyDate = null)
        {
            var idString = $"{model.UserId}|{model.Name}|{time}";

            if (day != null)
                idString += $"|{day}";

            if (notifyDate != null)
                idString += $"|{notifyDate.Value.Date}";

            return idString.GetHashCode();
        }
    }
}
