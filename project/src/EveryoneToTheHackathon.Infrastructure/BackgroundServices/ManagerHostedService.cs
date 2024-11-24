using System.Net.Http.Json;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class ManagerHostedService(
    IOptions<ConfigOptions> options,
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IHttpClientFactory httpClientFactory,
    IManagerService managerService) : BackgroundService
{
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ManagerHostedService));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await backgroundTaskQueue.DequeueAsync(stoppingToken);
        var dreamTeams = managerService.ManageTeams();
        var response = await httpClientFactory
            .CreateClient()
            .PostAsJsonAsync(
                options.Value.Services!.BaseUrlOptions!.DirectorUrl + "api/teams", 
                dreamTeams, 
                stoppingToken
            );
        if (!response.IsSuccessStatusCode)
        {
            Logger.Fatal("ExecuteAsync: Got bad response.");
            Logger.Fatal($"Response: { await response.Content.ReadAsStringAsync(stoppingToken) }");
            Environment.Exit(15);
        }
        await Task.CompletedTask;
    }
    
}