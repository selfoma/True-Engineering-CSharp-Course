using System.Net.Http.Json;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class DirectorHostedService(
    IOptions<ConfigOptions> options,
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IHttpClientFactory httpClientFactory,
    IDirectorService directorService) : BackgroundService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(DirectorHostedService));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for (int i = 0; i < options.Value.Hackathon!.TimesToHold; i++)
        {
            var hackathon = directorService.StartHackathon();
            NotifyEmployeeHackathonStarted(hackathon, stoppingToken);

            await backgroundTaskQueue.DequeueAsync(stoppingToken);
            directorService.PrintHackathonInfoById();
            directorService.ShowOverallResult();
        }
        await Task.CompletedTask;
    }

    private async void NotifyEmployeeHackathonStarted(Hackathon hackathon, CancellationToken stoppingToken)
    {
        var client = httpClientFactory.CreateClient();
        var tasks = options
            .Value
            .Services
            !.BaseUrl
            !.EmployeeUrls
            .Select(async url => { 
                try 
                {
                    var response = await client
                        .PostAsJsonAsync(url + "/api/notify", new { hackathon.HackathonId }, stoppingToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.Fatal("NotifyEmployeeHackathonStarted: Got bad response.");
                        Logger.Fatal($"Response: { await response.Content.ReadAsStringAsync(stoppingToken) }");
                        Environment.Exit(15);
                    }
                    else
                    {
                        Logger.Info($"Success POST: { url }");
                    }
                }
                catch (Exception e)
                {
                    Logger.Fatal("NotifyEmployeeHackathonStarted: Exception thrown");
                    Logger.Fatal($"URL: { url }");
                    Logger.Fatal("Exception:", e);
                    Environment.Exit(15);
                }
            });
        await Task.WhenAll(tasks);
    }
}