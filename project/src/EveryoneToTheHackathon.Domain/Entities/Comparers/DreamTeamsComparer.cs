namespace EveryoneToTheHackathon.Domain.Entities.Comparers;

public class DreamTeamsComparer : IEqualityComparer<HackathonDreamTeam>
{
    public bool Equals(HackathonDreamTeam? x, HackathonDreamTeam? y)
    {
        if (x is null || y is null) return false;
        return x.HackathonId == y.HackathonId &&
               x.TeamLeadId == y.TeamLeadId && 
               x.JuniorId == y.JuniorId;
    }

    public int GetHashCode(HackathonDreamTeam? obj)
    {
        return obj is null ? 0 : HashCode.Combine(obj.HackathonId, obj.TeamLeadId, obj.JuniorId);
    }
}