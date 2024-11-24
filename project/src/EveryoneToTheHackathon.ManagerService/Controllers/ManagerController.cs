using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.ManagerService.Controllers;

[ApiController]
[Route("/api")]
public class ManagerController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IManagerService managerService) : ControllerBase
{

    private readonly List<Employee> _employeesList = new();

    [HttpPost("employee")]
    public IActionResult HandleEmployeeRequest([FromBody] Employee? employee)
    {
        if (employee is null)
        {
            return BadRequest("Employee is null.");
        }
        _employeesList.Add(employee);
        if (_employeesList.Count != 10)
        {
            managerService.AddAndSplitEmployees(_employeesList);
            backgroundTaskQueue.EnqueueAsync(new("Build teams: [Assignee].ManagerService"));
        }
        return Ok($"Got it: [E].{employee.EmployeeId} - [ROLE].{employee.Role}.");
    }
    
}