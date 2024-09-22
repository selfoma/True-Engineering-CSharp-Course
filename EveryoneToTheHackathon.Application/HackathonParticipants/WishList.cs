namespace EveryoneToTheHackathon.HackathonParticipants;


public class WishList
{
    private readonly Dictionary<HackathonParticipant, double> _participantWishList = new();
    
    public WishList(List<HackathonParticipant> candidates)
    {
        Random random = new();
        List<int> distributionList = new();
        distributionList.AddRange(Enumerable.Range(1, candidates.Count).OrderBy(i => random.Next()));
        
        for (int i = 0; i < candidates.Count; i++)
        {
            _participantWishList.Add(candidates[i], distributionList[i]);
        }
    }

    public double GetSatisfaction(HackathonParticipant participant)
    {
        return _participantWishList[participant];
    }
}
