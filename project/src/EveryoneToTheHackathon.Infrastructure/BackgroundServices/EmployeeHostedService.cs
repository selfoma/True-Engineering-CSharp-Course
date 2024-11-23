using System.Net.Http.Json;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using log4net;
using Microsoft.Extensions.Hosting;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class EmployeeHostedService(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IHttpClientFactory httpClientFactory,
    IEmployeeService employeeService) : BackgroundService
{
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(EmployeeHostedService));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await backgroundTaskQueue.DequeueAsync(stoppingToken);
            var type = EmployeeRoleExtensions.GetRole(Environment.GetEnvironmentVariable("type"));
            var id = Convert.ToInt32(Environment.GetEnvironmentVariable("id"));
            var employee = employeeService.GetByIdAndRoleCurrentHackathon(id, type);
            PostEmployeeManagerService(employee!);
        }
        catch (OperationCanceledException e)
        {
            Logger.Fatal("ExecuteAsync: background task failed!");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(15);
        } 
        await Task.CompletedTask;
    }

    private void PostEmployeeManagerService(Employee employee)
    {
        var client = httpClientFactory.CreateClient();
        var url = "https://manager-service/api/manager/collect-participants";
        client.PostAsJsonAsync(url, employee);
    }
    
}