namespace EveryoneToTheHackathon.Domain.Entities;

public enum EmployeeRole
{
    Junior,
    TeamLead,
    None
}

public static class EmployeeRoleExtensions
{
    public static EmployeeRole GetRole(string? role)
    {
        if (role == "Junior")
        {
            return EmployeeRole.Junior;
        }
        if (role == "TeamLead")
        {
            return EmployeeRole.TeamLead;
        }
        return EmployeeRole.None;
    }
    
}

public class Employee
{
    public int EmployeeId { get; init; }
    
    public string FullName { get; set; } = string.Empty;
    public EmployeeRole Role { get; init; } = EmployeeRole.None;
    
    public List<HackathonEmployeeWishListMapping> HackathonEmployeeWishListMappings { get; set; } = new();

    public List<HackathonDreamTeam> DreamTeamJuniors { get; set; } = new();
    public List<HackathonDreamTeam> DreamTeamLeads { get; set; } = new();
    
    public override string ToString()
    {
        return $"{ EmployeeId }. { FullName } - { Role } ";
    }
}