using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using log4net;
using Microsoft.IdentityModel.Tokens;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IDirectorService
{
    Hackathon StartHackathon();
    Task FinishHackathon();
    void HandleWishLists(List<EmployeeResponseDto> wishListDtos);
    void HandleDreamTeams(List<DreamTeamDto> dreamTeamDtos);
    void PrintHackathonInfoById();
    void ShowOverallResult();
}

public class DirectorService(IHackathonService hackathonService) : IDirectorService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(DirectorService));

    private List<EmployeeResponseDto> _responseDtos = new();
    private List<DreamTeamDto> _dreamTeamDtos = new();
    
    public Hackathon StartHackathon()
    {
        Logger.Info("-----------------------------------------------------");
        Logger.Info("Starting hackathon");
        return hackathonService.StartHackathon();
    }

    public async Task FinishHackathon()
    {
        Logger.Info("Finishing hackathon");
        Logger.Info($"Response count: {_responseDtos.Count}");
        _dreamTeamDtos.ForEach(d =>
        {
            d.TeamLeadWishLists.Clear();
            d.JuniorWishLists.Clear();
            d.TeamLeadWishLists.AddRange( _responseDtos.FirstOrDefault(w => w.EmployeeId == d.TeamLeadId)!.WishListDtos);
            d.JuniorWishLists.AddRange( _responseDtos.FirstOrDefault(w => w.EmployeeId == d.JuniorId)!.WishListDtos);
        });
        await hackathonService.ComputeHarmonicAndFinish(_dreamTeamDtos);
    }

    public void HandleWishLists(List<EmployeeResponseDto> responseDtos)
    {
        Logger.Info("HandleWishLists: Got all wish lists.");
        _responseDtos = responseDtos;
    }

    public void HandleDreamTeams(List<DreamTeamDto> dreamTeamDtos)
    {
        Logger.Info("HandleDreamTeams: Got all dream teams.");
        _dreamTeamDtos = dreamTeamDtos;
    }
    
    public void PrintHackathonInfoById() => hackathonService.PrintInfo();

    public void ShowOverallResult()
    {
        Console.WriteLine("\n-- OVERALL RESULT --");
        Console.WriteLine($"Average: { hackathonService.GetAverageResult() }\n");
    }
}