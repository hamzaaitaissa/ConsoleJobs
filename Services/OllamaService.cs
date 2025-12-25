using Bot.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Bot.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly ILogger<IOllamaService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;
        private readonly string _model;

        public OllamaService(ILogger<IOllamaService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _baseUrl = "http://localhost:11434";
            _model = "mistral";
        }

        public async Task<string> AnalyzeJobAsync(string jobTitle, string company, string description)
        {
            var prompt = $@"Analyze this job posting and provide a brief assessment:
                            Job Title: {jobTitle}
                            Company: {company}
                            Description: {description}

                            Provide a 2-3 sentence analysis of this job opportunity including pros and cons.";

            return await GetLlamaResponseAsync(prompt);
        }


        public async Task<string> FilterJobsAsync(string jobs, string userPreferences)
        {
            var prompt = $@"Based on the user preferences: {userPreferences}

                            Here are the job listings:
                            {jobs}

                            Recommend the top 3 jobs that best match the user's preferences and explain why each one is a good fit.";

            return await GetLlamaResponseAsync(prompt);
        }

        public async Task<string> SummarizeJobAsync(string jobDetails)
        {
            var prompt = $@"Summarize the following job posting in 2-3 bullet points:
                            {jobDetails}";

            return await GetLlamaResponseAsync(prompt);
        }
        private async Task<string> GetLlamaResponseAsync(string prompt)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(500);
                var requestBody = new
                {
                    model = _model,
                    prompt = prompt,
                    stream = false,
                    temperature = 0.7
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync($"{_baseUrl}/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Llama API returned status code: {response.StatusCode}");
                    return "Unable to get AI analysis at this moment.";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                var result = jsonResponse.GetProperty("response").GetString() ?? "";

                return result.Trim();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error connecting to Llama service");
                return "Llama service is unavailable. Please ensure Docker container is running: docker-compose up -d";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
