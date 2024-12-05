using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using EveryoneToTheHackathon.Infrastructure.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.ManagerService.Controllers;

[ApiController]
[Route("api")]
public class ManagerController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IManagerService managerService) : ControllerBase
{
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ManagerController));

    private static readonly List<EmployeeResponseDto> EmployeeResponses = new();

    [HttpPost("employee")]
    public IActionResult HandleEmployeeRequest([FromBody] EmployeeResponseDto? employeeResponseDto)
    {
        if (employeeResponseDto is null)
        {
            Logger.Warn("EmployeeResponseDto is null!");
            return BadRequest("Request is null.");
        }
        Logger.Info($"HandleEmployeeRequest: Employee: [ID].{ employeeResponseDto.EmployeeId } - [R].{ employeeResponseDto.Role }.");
        EmployeeResponses.Add(employeeResponseDto);
        if (EmployeeResponses.Count == 10)
        {
            managerService.SplitResponses(EmployeeResponses);
            backgroundTaskQueue.EnqueueAsync(new("Build teams: [Assignee].ManagerService."));
            EmployeeResponses.Clear();
        }
        return Ok($"Got it.");
    }
}