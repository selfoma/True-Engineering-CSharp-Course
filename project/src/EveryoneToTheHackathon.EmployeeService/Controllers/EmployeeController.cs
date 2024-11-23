using EveryoneToTheHackathon.EmployeeService.Controllers.Models;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.EmployeeService.Controllers;

[ApiController]
[Route("/api/employee")]
public class EmployeeController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IEmployeeService employeeService) : ControllerBase
{
    
    [HttpPost("notify")]
    public IActionResult HandleDirectorServiceRequest([FromBody] HackathonInfo? hackathonInfo)
    {
        if (hackathonInfo is null)
        {
            return BadRequest("Invalid hackathon information.");
        }
        employeeService.HandleParticipantsList(hackathonInfo.HackathonId);
        employeeService.PrepareWishLists();
        backgroundTaskQueue.EnqueueAsync(new BaseTaskModel("Send data to manager."));
        return Ok("Wishlists done!");
    }
    
}