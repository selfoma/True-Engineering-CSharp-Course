using System.Linq.Expressions;
using log4net;

namespace EveryoneToTheHackathon.HackathonParticipants;

public class HackathonParticipant
{
    public int ParticipantId { get;  }
    public string Name { get;  }
    
    public virtual IWishList WishList { get; set; }

    public HackathonParticipant() {}
    
    public HackathonParticipant(int listId, string name)
    {
        ParticipantId = listId;
        Name = name;
    }

    public override string? ToString()
    {
        return $"#{ParticipantId}  {Name}";
    }

    public override bool Equals(object? obj)
    {
        var participant =  obj as HackathonParticipant;
        if (participant == null) return false;
        return ParticipantId.Equals(participant.ParticipantId);
    }

    public override int GetHashCode()
    {
        return ParticipantId.GetHashCode();
    }
    
    public void PrepareWishList(List<HackathonParticipant> candidates)
    {
        WishList = new WishList(candidates);
    }
}

