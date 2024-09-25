using EveryoneToTheHackathon.HackathonParticipants;

namespace EveryoneToTheHackathon.Strategy;

public interface ITeamBuildingStrategy
{
    DreamTeamList MakeDreamTeamList(List<HackathonParticipant> juniorsList, List<HackathonParticipant> teamLeadsList);
}

public class ManagerTeamBuildingStrategy : ITeamBuildingStrategy
{
    
    public DreamTeamList MakeDreamTeamList(List<HackathonParticipant> juniorsList, List<HackathonParticipant> teamLeadsList)
    {
        Random rand = new();
        List<int> distribution = new();
        distribution.AddRange(Enumerable.Range(0, juniorsList.Count).OrderBy(i => rand.Next()));

        List<Tuple<HackathonParticipant, HackathonParticipant>> dreamTeam = new();
        for (int i = 0; i < juniorsList.Count; i++)
        {
            dreamTeam.Add(new(juniorsList[i], teamLeadsList[distribution[i]]));
        }
        
        return new (dreamTeam);
    }
    
}