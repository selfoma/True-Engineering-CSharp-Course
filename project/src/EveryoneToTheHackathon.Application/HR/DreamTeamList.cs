using EveryoneToTheHackathon.HackathonParticipants;

namespace EveryoneToTheHackathon;


public class DreamTeamList
{
    public List<Tuple<HackathonParticipant, HackathonParticipant>> DreamTeam { get; }
    
    public DreamTeamList() {}

    public DreamTeamList(List<Tuple<HackathonParticipant, HackathonParticipant>> dreamTeam)
    {
        DreamTeam = dreamTeam;
    }

    public double ComputeHarmonicMean()
    {
        var harmonics = new List<double>(DreamTeam.Count * 2);
        
        foreach (var pair in DreamTeam)
        {
            var junior = pair.Item1;
            var teamLead = pair.Item2;
            harmonics.Add(junior.WishList.GetSatisfaction(teamLead));
            harmonics.Add(teamLead.WishList.GetSatisfaction(junior));
        }
        
        return ApplyFormula(harmonics);
    }

    protected double ApplyFormula(List<double> harmonics)
    {
        var reverseSum  = harmonics.Aggregate(0m, (x, y) => x + 1m / (decimal)y);
        return (double) (harmonics.Count / reverseSum);
    }
}