using System.Text;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Repositories;
using log4net;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IHackathonService
{
    Hackathon StartHackathon();
    void ComputeHarmonicAndFinish(List<HackathonDreamTeam> dreamTeams);
    
    Hackathon GetHackathonById(Guid hackathonId);
    decimal GetAverageResult();
    
    List<HackathonEmployeeWishListMapping> GetMappingListByHackathonId(Guid hackathonId);

    void PrintInfo();
}

public class HackathonService(IHackathonRepository repository) : IHackathonService
{
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HackathonService));

    private Hackathon _hackathon;

    public Hackathon StartHackathon()
    {
        _hackathon = new();
        Logger.Info($"Hackathon ID: { _hackathon.HackathonId }");
        repository.Add(_hackathon);
        return _hackathon;
    }

    public void ComputeHarmonicAndFinish(List<HackathonDreamTeam> dreamTeams)
    {
        var harmonic = ComputeHarmonicMean(dreamTeams);
        Logger.Info($"Harmonic mean: {harmonic}");
        repository.UpdateHarmonicById(_hackathon.HackathonId, harmonic);
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
        Console.WriteLine($"Hackathon ID: { _hackathon.HackathonId }");
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
        Console.WriteLine("\n");
    }

    public Hackathon GetHackathonById(Guid hackathonId)
    {
        return repository.GetHackathonById(hackathonId);
    }
    
    private decimal ComputeHarmonicMean(List<HackathonDreamTeam> dreamTeams)
    {
        var harmonics = new List<int>(dreamTeams.Count * 2);
        dreamTeams.ForEach(dt =>
        {
            var junior = dt.Junior!;
            var teamLead = dt.TeamLead!;
            var hackathonId = dt.HackathonId;
            int juniorPreference = FindPreference(hackathonId, junior, teamLead);
            int teamLeadPreference = FindPreference(hackathonId, teamLead, junior);
            harmonics.Add(juniorPreference);
            harmonics.Add(teamLeadPreference);
            dt.Junior = null;
            dt.TeamLead = null;
        });
        return ApplyFormula(harmonics);
    }

    private int FindPreference(Guid hackathonId, Employee owner, Employee preferred)
    {
        var mapping = repository.GetMapping(hackathonId, owner.EmployeeId, owner.Role);
        if (mapping is not null)
        {
            var wishList = repository
                .GetWishListsByMappingId(mapping.MappingId)
                .FirstOrDefault(w => w.PreferredEmployeeId == preferred.EmployeeId && w.PreferredEmployeeRole == preferred.Role);
            if (wishList is not null) return wishList.PreferenceValue;
            Logger.Fatal($"FindPreference: No wishlist with [M].{ mapping.MappingId } - [PE].{ preferred.EmployeeId } - [PR].{ preferred.Role }");
            Environment.Exit(20);
        }
        Logger.Fatal($"FindPreference: No mapping with [M].{ mapping?.MappingId } - [H].{ hackathonId } - [E].{ owner.EmployeeId } - [ER].{ owner.Role }");
        Environment.Exit(20);
        return -1;
    }

    protected decimal ApplyFormula(List<int> harmonics)
    {
        Logger.Info($"ApplyFormula: { string.Join(' ', harmonics) }");
        var reverseSum  = harmonics.Aggregate(0m, (x, y) => x + 1m / y);
        return harmonics.Count / reverseSum;
    }
    
}