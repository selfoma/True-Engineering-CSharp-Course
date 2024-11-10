using EveryoneToTheHackathon.Domain.Entities;
using log4net;

namespace EveryoneToTheHackathon.Infrastructure.Strategies;

public interface ITeamBuildingStrategy
{
    List<HackathonDreamTeam> BuildTeams(List<Employee> juniorsList, List<Employee> teamLeadsList);
}

public class ManagerTeamBuildingStrategy : ITeamBuildingStrategy
{
    
    public List<HackathonDreamTeam> BuildTeams(List<Employee> juniorsList, List<Employee> teamLeadsList)
    {
        var rand = new Random();
        var distribution = new List<int>();
        distribution.AddRange(Enumerable.Range(0, juniorsList.Count).OrderBy(i => rand.Next()));
        
        var dreamTeams = new List<HackathonDreamTeam>();
        
        for (int i = 0; i < juniorsList.Count; i++)
        {
            var junior = juniorsList[i];
            var teamLead = teamLeadsList[distribution[i]];
            var team = new HackathonDreamTeam
            {
                JuniorId = junior.EmployeeId,
                Junior = junior,
                TeamLeadId = teamLead.EmployeeId,
                TeamLead = teamLead,
                HackathonId = junior.HackathonEmployeeWishListMappings.Last().HackathonId
            };
            dreamTeams.Add(team);
        }
        
        return dreamTeams;
    }
    
}