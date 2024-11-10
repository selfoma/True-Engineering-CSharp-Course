using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Strategies;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IManagerService
{
    List<HackathonDreamTeam> ManageTeams(List<Employee> juniors, List<Employee> teamLeads);
}

public class ManagerService(ITeamBuildingStrategy strategy) : IManagerService
{
    
    public List<HackathonDreamTeam> ManageTeams(List<Employee> juniors, List<Employee> teamLeads)
    { 
        return strategy.BuildTeams(juniors, teamLeads);
    }
    
}