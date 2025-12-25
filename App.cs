using Bot.Services;
using Bot.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot
{
    public class App
    {
        private readonly IFetchJobsService _fetchJobs;
        private readonly ILogger<App> _logger;
        private readonly IUserInputService _userInputService;
        private readonly IOllamaService _ollamaService;


        public App(IFetchJobsService fetchJobsService, ILogger<App> logger, IUserInputService userInputService, IOllamaService ollamaService)
        {
            _fetchJobs = fetchJobsService;
            _logger = logger;
            _userInputService = userInputService;
            _ollamaService = ollamaService;
        }

        public async Task RunAsync()
        {
            try
            {
                var criteria = _userInputService.GetSearchCriteria();
                var jobs = await _fetchJobs.FetchAsync(criteria);

                if (!jobs.Any())
                    return;
                Console.WriteLine(new string('=', 80));
                Console.Write("\n ***Would you like AI analysis of these jobs? (y/n) [default: y]: \n");
                var useAi = Console.ReadLine()?.ToLower() ?? "y";

                if (useAi == "y" || useAi == "yes")
                {
                    Console.WriteLine("\n***Analyzing jobs with Ollama AI...\n");

                    Console.WriteLine("\n***Would you like a summary of a job? (y/n) [default: y]:  ");
                    var summary = Console.ReadLine() ?? "n";
                    if(summary == "y" || summary== "yes")
                    {
                        Console.WriteLine("Which one? [default: 1]");
                        if (int.TryParse(Console.ReadLine(), out var jobIndex) && jobIndex > 0 && jobIndex <= jobs.Count())
                        {
                            var selectedJob = jobs.ElementAt(jobIndex - 1);
                            Console.WriteLine("\n Summarizing job details...\n");

                            var jobSummary = await _ollamaService.SummarizeJobAsync(selectedJob.JobDescriptions);

                            Console.WriteLine(new string('=', 80));
                            Console.WriteLine($"DETAILED SUMMARY FOR: {selectedJob.JobTitle}");
                            Console.WriteLine(new string('=', 80));
                            Console.WriteLine(jobSummary);
                            Console.WriteLine(new string('=', 80) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid job number.\n");
                        }

                    }

                    Console.Write("Enter your job preferences for AI filtering (e.g., 'remote, high salary, startup'): ");
                    var preferences = Console.ReadLine() ?? "no specific preferences";

                    var jobsList = string.Join("\n", jobs.Select((j, index) =>
                        $"{index + 1}. {j.JobTitle} at {j.EmployerName}"));

                    var analysis = await _ollamaService.FilterJobsAsync(jobsList, preferences);

                    Console.WriteLine(new string('=', 80));
                    Console.WriteLine("AI RECOMMENDATIONS");
                    Console.WriteLine(new string('=', 80));
                    Console.WriteLine(analysis);
                    Console.WriteLine(new string('=', 80) + "\n");

                    Console.Write("Would you like detailed analysis of a specific job? (y/n): ");
                    var detailedAnalysis = Console.ReadLine()?.ToLower() ?? "n";

                    if (detailedAnalysis == "y" || detailedAnalysis == "yes")
                    {
                        Console.Write($"\nEnter job number to analyze (1-{jobs.Count()}): ");
                        if (int.TryParse(Console.ReadLine(), out var jobIndex) && jobIndex > 0 && jobIndex <= jobs.Count())
                        {
                            var selectedJob = jobs.ElementAt(jobIndex - 1);
                            Console.WriteLine("\nAnalyzing job details...\n");

                            var jobAnalysis = await _ollamaService.AnalyzeJobAsync(
                                selectedJob.JobTitle,
                                selectedJob.EmployerName,
                                selectedJob.JobDescriptions ?? "No description available");

                            Console.WriteLine(new string('=', 80));
                            Console.WriteLine($"DETAILED ANALYSIS FOR: {selectedJob.JobTitle}");
                            Console.WriteLine(new string('=', 80));
                            Console.WriteLine(jobAnalysis);
                            Console.WriteLine(new string('=', 80) + "\n");
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid job number.\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
            }
        }
    }
}
