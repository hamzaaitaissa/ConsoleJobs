using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Services.Interfaces
{
    public interface IOllamaService
    {
        Task<string> AnalyzeJobAsync(string jobTitle, string company, string description);
        Task<string> FilterJobsAsync(string jobs, string userPreferences);
        Task<string> SummarizeJobAsync(string jobDetails);
    }
}
