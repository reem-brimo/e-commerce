using E_Commerce.Services.Interfaces;
using E_Commerce.SharedKernal.OperationResults;
using System.Net;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;

        public GeminiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperationResult<HttpStatusCode, string>> ChatAsync(string prompt)
        {
            var result = new OperationResult<HttpStatusCode, string>();
            var requestPayload = new
            {
                contents = new[]
               {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestPayload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("", content);

            if (!response.IsSuccessStatusCode)
            {
                result.AddError("response not given");
                result.EnumResult = response.StatusCode;
                return result;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var returnedObject = JsonSerializer.Deserialize<ApiResponse>(responseBody, options);

            result.Result = returnedObject!.Candidates[0].Content.Parts[0].Text;
            result.EnumResult = response.StatusCode;
            return result;
        }
    }

    //Define response models
    public class Part
    {
        public string Text { get; set; }
    }

    public class Content
    {
        public List<Part> Parts { get; set; }
    }

    public class Candidate
    {
        public Content Content { get; set; }
    }

    public class ApiResponse
    {
        public List<Candidate> Candidates { get; set; }

    }
}
