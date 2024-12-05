using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using log4net;
using Microsoft.IdentityModel.Tokens;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IDirectorService
{
    Hackathon StartHackathon();
    void FinishHackathon(List<DreamTeamDto> dreamTeamDtos);
    void PrintHackathonInfoById();
    void ShowOverallResult();
}

public class DirectorService(IHackathonService hackathonService) : IDirectorService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(DirectorService));
    
    public Hackathon StartHackathon()
    {
        Logger.Info("-----------------------------------------------------");
        Logger.Info("Starting hackathon");
        return hackathonService.StartHackathon();
    }

    public void FinishHackathon(List<DreamTeamDto> dreamTeamDtos)
    {
        Logger.Info("Finishing hackathon");
        hackathonService.ComputeHarmonicAndFinish(dreamTeamDtos);
    }

    public void PrintHackathonInfoById() => hackathonService.PrintInfo();

    public void ShowOverallResult()
    {
        Console.WriteLine("\n-- OVERALL RESULT --");
        Console.WriteLine($"Average: { hackathonService.GetAverageResult() }\n");
    }
}