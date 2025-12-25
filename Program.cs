// See https://aka.ms/new-console-template for more information
using Bot;
using Bot.Services;
using Bot.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var host = Host.CreateDefaultBuilder(args)

    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient();
        services.AddTransient<App>();
        services.AddTransient<IFetchJobsService, FetchJobsService>();
        services.AddTransient<IUserInputService, UserInputService>();
        services.AddTransient<IOllamaService, OllamaService>();
    }).ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .UseEnvironment("Development")
    .Build();



await host.Services.GetRequiredService<App>().RunAsync();
