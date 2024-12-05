using System.Data.Common;
using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Entities;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace EveryoneToTheHackathon.Domain.Repositories;

public interface IHackathonRepository
{
    void Add(Hackathon hackathon);
    Hackathon GetHackathonById(Guid hackathonId);
    void UpdateHackathonDreamTeams(Guid hackathonId, List<HackathonDreamTeam> dreamTeams);
    void UpdateHarmonicById(Guid hackathonId, decimal harmonic);
    List<Hackathon> GetAll();

    HackathonEmployeeWishListMapping? GetMapping(Guid hackathonId, int employeeId, EmployeeRole role);
    List<HackathonEmployeeWishListMapping> GetMappingListByHackathonId(Guid hackathonId);
    List<WishList> GetWishListsByMappingId(Guid mappingId);
}

public class HackathonRepository(HackathonContext context) : IHackathonRepository
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HackathonRepository));

    public void Add(Hackathon hackathon)
    {
        var record = context
            .Hackathons
            .Find(hackathon.HackathonId);
        if (record is not null)
        {
            Logger.Fatal($"Add: Hackathon with [ID]={ hackathon.HackathonId } already exists.");
            Environment.Exit(30);
        }
        try
        {
            context.Hackathons.Add(hackathon);
            context.SaveChanges();
        }
        catch (Exception e) 
        {
            Logger.Fatal($"Add: Failed to add hackathon! [ID]={ hackathon.HackathonId }");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(40);
        }
    }

    public Hackathon GetHackathonById(Guid hackathonId)
    {
        var hackathon = context
            .Hackathons
            .Include(h => h.HackathonDreamTeams)
            .ThenInclude(dt => dt.Junior)
            .Include(h => h.HackathonDreamTeams)
            .ThenInclude(dt => dt.TeamLead)
            .FirstOrDefault(h => h.HackathonId == hackathonId);
        if (hackathon is not null) return hackathon;
        Logger.Fatal($"GetHackathonById: Hackathon with [ID]={ hackathonId } does not exist.");
        Environment.Exit(30);
        return null;
    }

    public void UpdateHackathonDreamTeams(Guid hackathonId, List<HackathonDreamTeam> dreamTeams)
    {
        var hackathon = context
            .Hackathons
            .Include(h => h.HackathonDreamTeams)
            .FirstOrDefault(h => h.HackathonId == hackathonId);
        if (hackathon is null)
        {
            Logger.Fatal($"UpdateHackathonDreamTeams: Hackathon with [ID]={ hackathonId } does not exist.");
            Environment.Exit(30);
        }
        try
        {
            hackathon.HackathonDreamTeams.Clear();
            hackathon.HackathonDreamTeams.AddRange(dreamTeams);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            Logger.Fatal($"UpdateHackathonDreamTeams: Failed to update hackathon dream teams! [ID]={ hackathonId }");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(40);
        }
    }

    public void UpdateHarmonicById(Guid hackathonId, decimal harmonic)
    {
        var hackathon = context
            .Hackathons
            .Find(hackathonId);
        if (hackathon is null)
        {
            Logger.Fatal($"UpdateHarmonicById: Hackathon with [ID]={ hackathonId } does not exist.");
            Environment.Exit(30);
        }
        try
        {
            hackathon.HarmonicMean = harmonic;
            context.SaveChanges();
        }
        catch (Exception e)
        {
            Logger.Fatal($"UpdateHarmonicById: Failed to update hackathon harmonic! [ID]={ hackathonId }");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(40);
        }
    }

    public List<Hackathon> GetAll()
    {
        return context
            .Hackathons
            .ToList();
    }

    public HackathonEmployeeWishListMapping? GetMapping(Guid hackathonId, int employeeId, EmployeeRole role)
    {
        return context
            .HackathonEmployeeWishListMappings
            .AsNoTracking()
            .FirstOrDefault(m =>
                m.HackathonId == hackathonId && 
                m.EmployeeId == employeeId && 
                m.EmployeeRole == role);
    }

    public List<HackathonEmployeeWishListMapping> GetMappingListByHackathonId(Guid hackathonId)
    {
        return context.HackathonEmployeeWishListMappings
            .AsNoTracking()
            .Where(m => m.HackathonId == hackathonId)
                .Select(m => m)
                    .Include(m => m.Employee)
                .ToList();
    }

    public List<WishList> GetWishListsByMappingId(Guid mappingId)
    {
        return context
            .WishLists
            .AsNoTracking()
            .Include(w => w.HackathonEmployeeWishListMappings)
            .Select(w => w)
                .Where(w => w.MappingId == mappingId)
            .ToList();
    }
}