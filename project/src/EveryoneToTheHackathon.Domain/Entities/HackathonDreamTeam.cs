namespace EveryoneToTheHackathon.Domain.Entities;

public class HackathonDreamTeam
{
    public int JuniorId { get; init; }
    public EmployeeRole JuniorRole { get; private init; } = EmployeeRole.Junior;
    public Employee? Junior { get; set; }
    
    public int TeamLeadId { get; init; }
    public EmployeeRole TeamLeadRole { get; private init; } = EmployeeRole.TeamLead;
    public Employee? TeamLead { get; set; }
    
    public Guid HackathonId { get; init; } = Guid.Empty;
    public Hackathon? Hackathon { get; set; }

    public override string ToString()
    {
        return $"{ Junior?.FullName } - { TeamLead?.FullName }";
    }
}