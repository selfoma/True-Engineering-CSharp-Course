using System.Runtime.CompilerServices;

namespace EveryoneToTheHackathon.HackathonParticipants;

public interface IWishList
{
    double GetSatisfaction(HackathonParticipant participant);
}


public class WishList : IWishList
{
    public Dictionary<HackathonParticipant, double> ParticipantWishList { get; set;  } = new(new HackathonParticipantsComparer());
    
    public WishList(List<HackathonParticipant> candidates)
    {
        Random random = new();
        List<int> distributionList = new();
        distributionList.AddRange(Enumerable.Range(1, candidates.Count).OrderBy(i => random.Next()));
        
        for (int i = 0; i < candidates.Count; i++)
        {
            ParticipantWishList.Add(candidates[i], distributionList[i]);
        }
    }

    public double GetSatisfaction(HackathonParticipant participant)
    {
        return ParticipantWishList[participant];
    }
}

internal class HackathonParticipantsComparer : IEqualityComparer<HackathonParticipant>
{
    public bool Equals(HackathonParticipant? x, HackathonParticipant? y)
    {
        if (x is null || y is null) return false;
        return x.Equals(y);
    }

    public int GetHashCode(HackathonParticipant obj)
    {
        return obj.GetHashCode();
    }
}
