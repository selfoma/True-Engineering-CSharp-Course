using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.EmployeeService.Controllers;

[ApiController]
[Route("api")]
public class EmployeeController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IEmployeeService employeeService) : ControllerBase
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(EmployeeController));
    
    [HttpPost("notify")]
    public IActionResult HandleDirectorServiceRequest([FromBody] NotifyRequestDto? notifyRequestDto)
    {
        if (notifyRequestDto is null)
        {
            Logger.Warn("NotifyRequestDto is null!");
            return BadRequest("Request is null!");
        }
        Logger.Info($"HackathonInfo: [ID].{ notifyRequestDto.HackathonId }.");
        employeeService.HandleParticipantsList(notifyRequestDto.HackathonId);
        employeeService.PrepareWishLists();
        backgroundTaskQueue.EnqueueAsync(
            new("Send wish lists to manager: [Assignee].EmployeeHostedService."));
        return Ok("Got it.");
    }
}