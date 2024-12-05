namespace EveryoneToTheHackathon.Domain.Entities;

public class WishList
{
    public Guid WishListId { get; init; } = Guid.NewGuid();
    
    public int PreferenceValue { get; init; }
    
    public int PreferredEmployeeId { get; init; }
    public EmployeeRole PreferredEmployeeRole { get; init; } = EmployeeRole.None;
    
    public Guid MappingId { get; init; } = Guid.Empty;
    public HackathonEmployeeWishListMapping? HackathonEmployeeWishListMappings { get; set; }
    
    public override string ToString()
    {
        return $"[Pe].{ PreferredEmployeeId } [V].{ PreferenceValue } [M].{ MappingId }";
    }

    public static WishList Create(Guid mappingId, int preferenceValue, Employee preferredEmployee)
    {
        return new()
        {
            MappingId = mappingId, 
            PreferenceValue = preferenceValue,
            PreferredEmployeeId = preferredEmployee.EmployeeId,
            PreferredEmployeeRole = preferredEmployee.Role,
        };
    }
}