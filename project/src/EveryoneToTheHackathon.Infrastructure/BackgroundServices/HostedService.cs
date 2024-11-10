using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices;

public class HostedService(
    IOptions<ConfigOptions> options,
    IDirectorService directorService, 
    IManagerService managerService, 
    IEmployeeService employeeService) : BackgroundService
{
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for (int i = 0; i < options.Value.Hackathon.TimesToHold; i++)
        {
            var hackathon = directorService.StartHackathon();
            employeeService.HandleParticipantsList(hackathon.HackathonId);
            employeeService.PrepareWishLists();
            var dreamTeams = managerService
                .ManageTeams(employeeService.Juniors, employeeService.TeamLeads); 
            directorService.FinishHackathon(dreamTeams);
        }
        directorService.PrintHackathonInfoById();
        directorService.ShowOverallResult();
        return Task.CompletedTask;
    }
    
}