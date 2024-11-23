using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Strategies;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IManagerService
{
    List<HackathonDreamTeam> ManageTeams();
    void AddAndSplitEmployees(List<Employee> employees);
}

public class ManagerService(ITeamBuildingStrategy strategy) : IManagerService
{

    private readonly List<Employee> _juniors = new();
    private readonly List<Employee> _teamLeads = new();
    
    public List<HackathonDreamTeam> ManageTeams()
    { 
        return strategy.BuildTeams(_juniors, _teamLeads);
    }

    public void AddAndSplitEmployees(List<Employee> employees)
    {
        _juniors.Clear();
        _teamLeads.Clear();
        
        _juniors.AddRange
        (
            employees
            .Select(e => e)
            .Where(e => e.Role == EmployeeRole.Junior)
            .ToList()
        );
        _teamLeads.AddRange
        (
            employees
                .Select(e => e)
                .Where(e => e.Role == EmployeeRole.TeamLead)
                .ToList()
        );
    }
    
}