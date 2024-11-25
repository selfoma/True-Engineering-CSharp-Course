using System.Net.Http.Json;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class EmployeeHostedService(
    IOptions<ConfigOptions> options,
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
            var employee = employeeService.GetThisEmployee();
            PostEmployeeManagerService(employee!);
        }
        catch (Exception e)
        {
            Logger.Fatal("ExecuteAsync: Background task failed!");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(15);
        } 
        await Task.CompletedTask;
    }

    private void PostEmployeeManagerService(Employee employee)
    {
        httpClientFactory
            .CreateClient()
            .PostAsJsonAsync(options.Value.Services!.BaseUrlOptions!.ManagerUrl + "api/employee", employee);
    }
    
}