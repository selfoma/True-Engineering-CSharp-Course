using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Messages;
using log4net;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class DirectorHostedService(
    IOptions<ConfigOptions> options,
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IBus bus, 
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
            await backgroundTaskQueue.DequeueAsync(stoppingToken);
            await directorService.FinishHackathon();
        }
        directorService.PrintHackathonInfoById();
        directorService.ShowOverallResult();
        await Task.CompletedTask;
    }

    private void NotifyEmployeeHackathonStarted(Hackathon hackathon, CancellationToken stoppingToken)
    {
        var message = new HackathonStarted(hackathon.HackathonId);
        bus.Publish(message, stoppingToken);
        Logger.Info("Publish done!");
    }
}