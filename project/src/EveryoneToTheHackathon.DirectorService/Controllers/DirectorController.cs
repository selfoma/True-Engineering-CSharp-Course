using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using EveryoneToTheHackathon.Infrastructure.Services;
using log4net;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EveryoneToTheHackathon.DirectorService.Controllers;

[ApiController]
[Route("api")]
public class DirectorController(
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue,
    IDirectorService directorService) : ControllerBase
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(DirectorController));

    [HttpPost("teams")]
    public IActionResult HandleManagerRequestContainingDreamTeams([FromBody] ManagerResponseDto? managerResponseDto)
    {
        if (managerResponseDto is null)
        {
            Logger.Warn("ManagerResponseDto is null!");
            return BadRequest("Request is null!");
        }
        Logger.Info(
            $"HandleManagerRequestContainingDreamTeams: Received {managerResponseDto.DreamTeamDtos.Count} dream teams.");
        directorService.HandleDreamTeams(managerResponseDto.DreamTeamDtos);
        backgroundTaskQueue.EnqueueAsync(new("Show hackathon result: [Assignee].DirectorService."));
        return Ok("Got it.");
    }
}