using Bot.DTO;
using Bot.Models;
using Bot.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bot.Services
{
    public class FetchJobsService : IFetchJobsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FetchJobsService> _logger;
        private readonly IConfiguration _configuration;

        public FetchJobsService(IHttpClientFactory httpClientFactory, ILogger<FetchJobsService> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<InsideData>> FetchAsync(JobSearchCriteria jobSearchCriteria)
        {
            try
            {
                var apiKey = _configuration.GetValue<string>("RapidApi:ApiKey");
                var client = _httpClientFactory.CreateClient();
                var keywords = Uri.EscapeDataString(jobSearchCriteria.Keywords);
                var dateFilter = jobSearchCriteria.DatePosted == "any" ? "" : $"date_posted={jobSearchCriteria.DatePosted}";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://jsearch.p.rapidapi.com/search?query={keywords}&page=1&num_pages=1&country={jobSearchCriteria.Country}{dateFilter}"),
                    Headers =
                        {
                            { "x-rapidapi-key", apiKey },
                            { "x-rapidapi-host", "jsearch.p.rapidapi.com" },
                        },
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                //serializing data
                var content = await response.Content.ReadAsStringAsync();
                var serializedJson = JsonSerializer.Deserialize<FetchJobResponse>(content);
                if (serializedJson?.Data == null || !serializedJson.Data.Any())
                {
                    Console.WriteLine("\nNo jobs found matching your criteria.\n");
                    return Enumerable.Empty<InsideData>();
                }
                Console.WriteLine($"****> Found{serializedJson.Data.Count} Job(s)");
                for (int i = 0; i < serializedJson.Data.Count(); i++)
                {
                    var job = serializedJson.Data[i];
                    Console.WriteLine($"\n[{i + 1}] {job.JobTitle}");
                    Console.WriteLine($"     Company: {job.EmployerName}");
                    Console.WriteLine($"     Location: {jobSearchCriteria.Country}");
                    Console.WriteLine($"     Type: {job.EmployementType}");
                    Console.WriteLine($"     Apply: {job.ApplyLink}");
                    Console.WriteLine($"     LinkedIn: {job.LinkedIn}");
                    Console.WriteLine($"     Website: {job.EmployerWebsite}");
                }
                return serializedJson.Data;

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching jobs");
                Console.WriteLine($"\nError: Failed to fetch jobs. {ex.Message}\n");
                return Enumerable.Empty<InsideData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured");
                return Enumerable.Empty<InsideData>();
            }
        }
    }
}
