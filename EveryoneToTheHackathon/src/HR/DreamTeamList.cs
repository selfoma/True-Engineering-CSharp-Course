using EveryoneToTheHackathon.HackathonParticipants;

namespace EveryoneToTheHackathon;


public class DreamTeamList
{
    private readonly List<Tuple<HackathonParticipant, HackathonParticipant>> _dreamTeam;

    public DreamTeamList(List<Tuple<HackathonParticipant, HackathonParticipant>> dreamTeam)
    {
        _dreamTeam = dreamTeam;
    }

    public double ComputeHarmonicMean()
    {
        double inverseSatisfaction = 0;
        double participantsCount = 0;
        
        foreach (var pair in _dreamTeam)
        {
            var junior = pair.Item1;
            var teamLead = pair.Item2;
            inverseSatisfaction += 1.0 / junior.WishList.GetSatisfaction(teamLead);
            inverseSatisfaction += 1.0 / teamLead.WishList.GetSatisfaction(junior);
            participantsCount += 2;
        }

        return participantsCount / inverseSatisfaction;
    }
}