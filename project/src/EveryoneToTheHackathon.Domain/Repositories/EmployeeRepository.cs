using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Entities.Comparers;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EveryoneToTheHackathon.Domain.Repositories;

public interface IEmployeeRepository
{
    void Add(Employee employee);
}

public class EmployeeRepository(HackathonContext context) : IEmployeeRepository
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(EmployeeRepository));
    private static readonly EmployeeComparer Comparer = new();

    public void Add(Employee employee)
    {
        try
        {
            var existence = context.Employees.Find(employee.EmployeeId, employee.Role);
            if (existence is not null)
            {
                context.HackathonEmployeeWishListMappings.Add(employee.HackathonEmployeeWishListMappings.Last());
            }
            else
            {
                context.Employees.Add(employee);
            }
            context.SaveChanges();
        }
        catch (Exception e)
        {
            Logger.Fatal($"Add: [ID].{ employee.EmployeeId } - [ROLE].{ employee.Role } failed to add!");
            Logger.Fatal("Exception: ", e);
            Environment.Exit(40);
        }
    }
}