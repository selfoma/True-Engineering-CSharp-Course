using log4net;

namespace EveryoneToTheHackathon.HackathonParticipants;

public class HackathonParticipant
{
    private int _participantId;
    private string _firstName;
    private string _lastName;
    
    public WishList WishList { get; private set; }
    
    public HackathonParticipant(int listId, string firstName, string lastName)
    {
        _participantId = listId;
        _firstName = firstName;
        _lastName = lastName;
    }

    public override string? ToString()
    {
        return $"#{_participantId}  {_firstName}  {_lastName}";
    }

    public void PrepareWishList(List<HackathonParticipant> candidates)
    {
        WishList = new WishList(candidates);
    }
}

