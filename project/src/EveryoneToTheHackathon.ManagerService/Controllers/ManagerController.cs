using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.ManagerService.Controllers;

[ApiController]
[Route("/api/manager-service")]
public class ManagerController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IManagerService managerService) : ControllerBase
{

    private readonly List<Employee> _employeesList = new();

    [HttpPost("collect-employees")]
    public IActionResult s([FromBody] Employee employee)
    {
        _employeesList.Add(employee);
        if (_employeesList.Count == 10)
        {
            managerService.AddAndSplitEmployees(_employeesList);
            backgroundTaskQueue.EnqueueAsync(new BaseTaskModel("Build teams."));
        }
        return Ok("Got it");
    }
    
}