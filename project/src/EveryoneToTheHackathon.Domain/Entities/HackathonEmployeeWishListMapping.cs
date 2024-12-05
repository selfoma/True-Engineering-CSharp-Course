namespace EveryoneToTheHackathon.Domain.Entities;

public class HackathonEmployeeWishListMapping
{
    public Guid MappingId { get; init; } = Guid.NewGuid();
    
    public Guid HackathonId { get; init; } = Guid.Empty;
    public Hackathon? Hackathon { get; init; }
    
    public int EmployeeId { get; init; }
    public EmployeeRole EmployeeRole { get; init; } = EmployeeRole.None;
    public Employee? Employee { get; set; }

    public List<WishList> WishLists { get; set; } = new();

    public override string ToString()
    {
        return $"Employee: { Employee }, HackathonId: { HackathonId }";
    }
}