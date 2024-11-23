using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using Microsoft.Extensions.Hosting;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class ManagerHostedService(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IHttpClientFactory httpClientFactory,
    IManagerService managerService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = httpClientFactory.CreateClient();
        
        
        return Task.CompletedTask;
    }
}