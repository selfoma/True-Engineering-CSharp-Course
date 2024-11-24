using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.DirectorService.Controllers;

[ApiController]
[Route("/api")]
public class DirectorController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IDirectorService directorService) : ControllerBase
{

    [HttpPost("teams")]
    public IActionResult HandleManagerRequestContainingDreamTeams([FromBody] List<HackathonDreamTeam>? dreamTeams)
    {
        if (dreamTeams is null)
        {
            return BadRequest("Dream team list is null!");
        }
        directorService.FinishHackathon(dreamTeams);
        backgroundTaskQueue.EnqueueAsync(new("Show hackathon result: [Assignee].DirectorService"));
        return Ok();
    }
    
}