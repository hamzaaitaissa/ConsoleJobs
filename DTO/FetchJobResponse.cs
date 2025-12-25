using System.Text.Json.Serialization;

namespace Bot.DTO
{
    public class FetchJobResponse()
    {
        [JsonPropertyName("data")]
        public List<InsideData> Data { get; set; }

    }

    public class InsideData
    {
        [JsonPropertyName("job_id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("employer_name")]
        public string EmployerName { get; set; } = string.Empty;
        [JsonPropertyName("employer_website")]
        public string? EmployerWebsite { get; set; }
        [JsonPropertyName("employer_linkedin")]
        public string? LinkedIn { get; set; }
        [JsonPropertyName("job_publisher")]
        public string? Publisher { get; set; }
        [JsonPropertyName("job_employment_type_text")]
        public string? EmployementType { get; set; }
        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; } = string.Empty;
        [JsonPropertyName("job_apply_link")]
        public string ApplyLink { get; set; } = string.Empty;
        [JsonPropertyName("apply_options")]
        public List<ApplyOptions> ApplyOptions { get; set; }
        [JsonPropertyName("job_description")]
        public string JobDescriptions { get; set; }

    }


    public class ApplyOptions
    {
        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }
        [JsonPropertyName("apply_link")]
        public string ApplyLink { get; set; }
    }
}
