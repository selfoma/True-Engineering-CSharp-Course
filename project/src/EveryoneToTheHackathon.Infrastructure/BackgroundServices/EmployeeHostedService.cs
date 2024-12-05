using System.Net.Http.Json;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Dtos;
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
            PostEmployeeManagerService(employee!, stoppingToken);
        }
        catch (Exception e)
        {
            Logger.Fatal("ExecuteAsync: Background task failed!");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(15);
        } 
        await Task.CompletedTask;
    }

    private async void PostEmployeeManagerService(Employee employee, CancellationToken stoppingToken)
    {
        try
        {
            var response = await httpClientFactory
                .CreateClient()
                .PostAsJsonAsync(
                    options.Value.Services!.BaseUrl!.ManagerUrl + "/api/employee", 
                    new
                    {
                        employee.EmployeeId,
                        employee.FullName,
                        employee.Role,
                        employee.HackathonEmployeeWishListMappings.Last().HackathonId,
                        WishListDtos = employee
                            .HackathonEmployeeWishListMappings
                            .Last()
                            .WishLists
                            .Select(w => WishListDto.ToDto(w))
                            .ToList()
                    }, 
                    stoppingToken);
            if (!response.IsSuccessStatusCode)
            {
                Logger.Fatal($"PostEmployeeManagerService: Got bad response");
                Logger.Fatal($"Response: { await response.Content.ReadAsStringAsync(stoppingToken) }");
                Environment.Exit(15);
            }
            else
            {
                Logger.Info($"Success POST: [ID].{employee.EmployeeId} - [ROLE].{employee.Role}");
            }
        }
        catch (Exception e)
        {
            Logger.Fatal("PostEmployeeManagerService: Exception thrown");
            Logger.Fatal("Exception:", e);
            Environment.Exit(15);
        }
    }
}