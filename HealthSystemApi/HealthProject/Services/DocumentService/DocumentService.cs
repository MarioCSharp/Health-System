using HealthProject.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HealthProject.Services.DocumentService
{
    public class DocumentService : IDocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DocumentService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "https://localhost:7097";

            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> AddAsync(DocumentAddModel model, IFormFile file)
        {
            CheckInternetConnection();
            var form = new MultipartFormDataContent();

            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            form.Add(fileContent, "file", file.FileName);

            form.Add(new StringContent(model.Type), "Type");
            form.Add(new StringContent(model.Title), "Title");
            form.Add(new StringContent(model.Notes), "Notes");
            form.Add(new StringContent(model.HealthIssueId.ToString()), "HealthIssueId");
            form.Add(new StringContent(model.FileName), "FileName");
            form.Add(new StringContent(model.FileExtension), "FileExtension");
            form.Add(new StringContent(model.UserId.ToString()), "UserId");

            var response = await _httpClient.PostAsync($"{_url}/Document/Add", form);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(resultContent);

            return result;
        }

        public async Task<DocumentDetailsModel> DetailsAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Document/Details?id={id}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var healthIssues = System.Text.Json.JsonSerializer.Deserialize<DocumentDetailsModel>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return healthIssues;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new DocumentDetailsModel();
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

            return new DocumentDetailsModel();
        }

        public async Task<List<DocumentViewModel>> GetUserDocuments(string userId)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Document/AllByUser?userId={userId}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var healthIssues = System.Text.Json.JsonSerializer.Deserialize<List<DocumentViewModel>>(jsonResponse, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully created ToDo");
                    return healthIssues;
                }
                else
                {
                    Debug.WriteLine("---> Non Http 2xx response");
                    return new List<DocumentViewModel>();
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

            return new List<DocumentViewModel>();
        }

        public async Task RemoveAsync(int id)
        {
            CheckInternetConnection();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/Document/Remove?id={id}");
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

        private string ToQueryString(DocumentAddModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }
    }
}
