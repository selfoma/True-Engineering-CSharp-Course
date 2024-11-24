using EveryoneToTheHackathon.EmployeeService.Controllers.Models;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.EmployeeService.Controllers;

[ApiController]
[Route("/api")]
public class EmployeeController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IEmployeeService employeeService) : ControllerBase
{
    
    [HttpPost("notify")]
    public IActionResult HandleDirectorServiceRequest([FromBody] HackathonInfo? hackathonInfo)
    {
        if (hackathonInfo is null)
        {
            return BadRequest("Hackathon is null!");
        }
        employeeService.HandleParticipantsList(hackathonInfo.HackathonId);
        employeeService.PrepareWishLists();
        backgroundTaskQueue.EnqueueAsync(
            new("Send wish lists to manager: [Assignee].EmployeeHostedService"));
        return Ok();
    }
    
}