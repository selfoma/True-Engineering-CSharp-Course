using System.Text;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using log4net;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IHackathonService
{
    Hackathon StartHackathon();
    void ComputeHarmonicAndFinish(List<DreamTeamDto> dreamTeamDtos);
    
    Hackathon GetHackathonById(Guid hackathonId);
    decimal GetAverageResult();
    
    List<HackathonEmployeeWishListMapping> GetMappingListByHackathonId(Guid hackathonId);

    void PrintInfo();
}

public class HackathonService(IHackathonRepository repository) : IHackathonService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HackathonService));

    private Hackathon? _hackathon;

    public Hackathon StartHackathon()
    {
        _hackathon = new();
        Logger.Info($"Hackathon ID: { _hackathon.HackathonId }");
        repository.Add(_hackathon);
        return _hackathon;
    }

    public void ComputeHarmonicAndFinish(List<DreamTeamDto> dreamTeamDtos)
    {
        var harmonic = ComputeHarmonicMean(dreamTeamDtos);
        Logger.Info($"Harmonic mean: { harmonic }");
        repository.UpdateHarmonicById(_hackathon!.HackathonId, harmonic);
        var dreamTeams = dreamTeamDtos.Select(dto => DreamTeamDto.FromDto(dto)).ToList();
        repository.UpdateHackathonDreamTeams(_hackathon.HackathonId, dreamTeams);
    }
    
    public decimal GetAverageResult()
    {
        var hackathons = repository.GetAll();
        return hackathons.Aggregate(0.0m, (acc, h) => acc + h.HarmonicMean) / hackathons.Count;
    }

    public List<HackathonEmployeeWishListMapping> GetMappingListByHackathonId(Guid hackathonId)
    {
        return repository.GetMappingListByHackathonId(hackathonId);
    }

    public void PrintInfo()
    {
        Console.WriteLine("\n-- HACKATHON INFO --");
        Console.WriteLine($"Hackathon ID: { _hackathon!.HackathonId }");
        var hackathon = GetHackathonById(_hackathon.HackathonId);
        Console.WriteLine($"Harmonic: { hackathon.HarmonicMean }\n");
        
        var mappings = GetMappingListByHackathonId(hackathon.HackathonId);
        var juniors = mappings
            .Where(m => 
                m.HackathonId == _hackathon.HackathonId && m.EmployeeRole == EmployeeRole.Junior)
                .Select(m => m.Employee)
                .ToList();
        var teamLeads = mappings
            .Where(m => 
                m.HackathonId == _hackathon.HackathonId && m.EmployeeRole == EmployeeRole.TeamLead)
                .Select(m => m.Employee)
                .ToList();
        
        Console.WriteLine("PARTICIPANTS: ");
        Console.WriteLine("ID ---------- Full name -- Role");
        juniors.ForEach(Console.WriteLine);
        teamLeads.ForEach(Console.WriteLine);
        Console.WriteLine("\nDREAM TEAMS: ");
        Console.WriteLine("Junior ------- Team Lead");
        hackathon.HackathonDreamTeams.ForEach(Console.WriteLine);
    }

    public Hackathon GetHackathonById(Guid hackathonId)
    {
        return repository.GetHackathonById(hackathonId);
    }
    
    private decimal ComputeHarmonicMean(List<DreamTeamDto> dreamTeamDtos)
    {
        var harmonics = new List<int>(dreamTeamDtos.Count * 2);
        dreamTeamDtos.ForEach(dto =>
        {
            int juniorPreference = FindPreference(dto.JuniorWishLists, dto.TeamLeadId);
            int teamLeadPreference = FindPreference(dto.TeamLeadWishLists, dto.JuniorId);
            harmonics.Add(juniorPreference);
            harmonics.Add(teamLeadPreference);
        });
        return ApplyFormula(harmonics);
    }

    private int FindPreference(List<WishListDto> employeeWishLists, int preferredEmployeId)
    {
        Logger.Info($"FindPreference: [WLC].{ employeeWishLists.Count }.");
        return employeeWishLists
            .FirstOrDefault(w => w.PreferredEmployeeId == preferredEmployeId)?
            .PreferenceValue ?? -1;
    }

    protected decimal ApplyFormula(List<int> harmonics)
    {
        Logger.Info($"ApplyFormula: { string.Join(' ', harmonics) }");
        var reverseSum  = harmonics.Aggregate(0m, (x, y) => x + 1m / y);
        return harmonics.Count / reverseSum;
    }
}