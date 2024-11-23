using System.Net.Http.Json;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class DirectorHostedService(
    IOptions<ConfigOptions> options,
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IHttpClientFactory httpClientFactory,
    IDirectorService directorService) : BackgroundService
{
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = httpClientFactory.CreateClient();

        var hackathon = directorService.StartHackathon();
        NotifyEmployeeHackathonStarted(hackathon, client);

        while (true)
        {
            
        }
        
        return Task.CompletedTask;
    }

    private async void NotifyEmployeeHackathonStarted(Hackathon hackathon, HttpClient client)
    {
        var servicesUrl = new List<string>
        {
            "https://teamlead-1/api/employee/notify",
            "https://teamlead-2/api/employee/notify",
            "https://teamlead-3/api/employee/notify",
            "https://teamlead-4/api/employee/notify",
            "https://teamlead-5/api/employee/notify",
            "https://junior-1/api/employee/notify",
            "https://junior-2/api/employee/notify",
            "https://junior-3/api/employee/notify",
            "https://junior-4/api/employee/notify",
            "https://junior-5/api/employee/notify",
        };

        var tasks = servicesUrl.Select(async url =>
        {
            try
            {
                var response = await client.PostAsJsonAsync(url, new { HackathonId = hackathon.HackathonId });
                if (!response.IsSuccessStatusCode)
                {
                    // TODO: HANDLE IT
                } 
            }
            catch (Exception e)
            {
                // TODO: HANDLE IT
            }
        });
        await Task.WhenAll(tasks);
    }

}