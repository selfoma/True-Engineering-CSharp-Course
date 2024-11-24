using System.Net.Http.Json;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
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
        var hackathon = directorService.StartHackathon();
        NotifyEmployeeHackathonStarted(hackathon, stoppingToken);

        await backgroundTaskQueue.DequeueAsync(stoppingToken);
        directorService.ShowOverallResult();
        
        await Task.CompletedTask;
    }

    private async void NotifyEmployeeHackathonStarted(Hackathon hackathon, CancellationToken stoppingToken)
    {
        var tasks = options
            .Value
            .Services
            !.BaseUrlOptions
            !.EmployeeUrls
            .Select(async url => { 
                try 
                {
                    var response = await httpClientFactory
                        .CreateClient()
                        .PostAsJsonAsync(url + "api/notify", new { hackathon.HackathonId }, stoppingToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.Fatal("NotifyEmployeeHackathonStarted: Got bad response.");
                        Logger.Fatal($"Response: { await response.Content.ReadAsStringAsync(stoppingToken) }");
                        Environment.Exit(15);
                    } 
                }
                catch (Exception e)
                {
                    Logger.Fatal("NotifyEmployeeHackathonStarted: Exception thrown");
                    Logger.Fatal("Exception:", e);
                    Environment.Exit(15);
                }
            });
        await Task.WhenAll(tasks);
    }

}