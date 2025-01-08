namespace EveryoneToTheHackathon.Domain.Entities.Comparers;

public class EmployeeComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee? x, Employee? y)
    {
        if (x is null || y is null) return false;
        return x.EmployeeId == y.EmployeeId && x.Role == y.Role;
    }

    public int GetHashCode(Employee? e)
    {
        return e is null ? 0 : HashCode.Combine(e.EmployeeId, e.Role);
    }
}