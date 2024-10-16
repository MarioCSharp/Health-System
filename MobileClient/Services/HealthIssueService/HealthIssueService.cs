﻿using HealthProject.Models;
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

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "https://localhost";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(HealthIssueAddModel model, string userId)
        {
            CheckInternetConnection();

            try
            {
                model.StartDate = model.IssueStartDate.ToString("yyyy-MM-dd");
                model.EndDate = model.IssueEndDate.ToString("yyyy-MM-dd");
                var name = model.Name;
                var description = model.Description;

                string queryString = $"?startDate={model.StartDate}&endDate={model.EndDate}&name={Uri.EscapeDataString(name)}&description={Uri.EscapeDataString(description)}&userId={Uri.EscapeDataString(userId)}";


                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/HealthIssue/Add{queryString}");
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

        public async Task<List<HealthIssueDisplayModel>> AllByUser(string userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/HealthIssue/UserIssues?userId={userId}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var healthIssues = JsonSerializer.Deserialize<List<HealthIssueDisplayModel>>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return healthIssues ?? new List<HealthIssueDisplayModel>();
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

            return new List<HealthIssueDisplayModel>();
        }

        public async Task<HealthIssueDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/HealthIssue/Details{queryString}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var hI = JsonSerializer.Deserialize<HealthIssueDetailsModel>(responseBody, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    return hI ?? new HealthIssueDetailsModel();
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

            return new HealthIssueDetailsModel();
        }

        public async Task Remove(int id)
        {
            CheckInternetConnection();

            try
            {
                string queryString = $"?id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}:5115/api/HealthIssue/Remove{queryString}");
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
