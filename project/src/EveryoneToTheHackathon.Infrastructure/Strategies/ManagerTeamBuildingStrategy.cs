using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using log4net;

namespace EveryoneToTheHackathon.Infrastructure.Strategies;

public interface ITeamBuildingStrategy
{
    List<DreamTeamDto> BuildTeams(List<EmployeeResponseDto> juniorsList, List<EmployeeResponseDto> teamLeadsList);
}

public class ManagerTeamBuildingStrategy : ITeamBuildingStrategy
{
    public List<DreamTeamDto> BuildTeams(List<EmployeeResponseDto> juniorsList, List<EmployeeResponseDto> teamLeadsList)
    {
        var rand = new Random();
        var distribution = new List<int>();
        distribution.AddRange(Enumerable.Range(0, juniorsList.Count).OrderBy(i => rand.Next()));
        
        var dreamTeams = new List<DreamTeamDto>();
        
        for (int i = 0; i < juniorsList.Count; i++)
        {
            var junior = juniorsList[i];
            var teamLead = teamLeadsList[distribution[i]];
            var team = new DreamTeamDto
            (
               junior.EmployeeId,
               junior.WishListDtos,
               teamLead.EmployeeId,
               teamLead.WishListDtos,
               junior.HackathonId
            );
            dreamTeams.Add(team);
        }
        
        return dreamTeams;
    }
}