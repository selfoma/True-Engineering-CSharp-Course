using EveryoneToTheHackathon.Domain.Configurations;
using EveryoneToTheHackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EveryoneToTheHackathon.Domain.Contexts;

public class HackathonContext(DbContextOptions<HackathonContext> options) : DbContext(options)
{
    public DbSet<Hackathon> Hackathons { get; init; }
    public DbSet<Employee> Employees { get; init; }
    public DbSet<HackathonEmployeeWishListMapping> HackathonEmployeeWishListMappings { get; init; }
    public DbSet<WishList> WishLists { get; init; }
    public DbSet<HackathonDreamTeam> HackathonDreamTeams { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new HackathonConfiguration());
        modelBuilder.ApplyConfiguration(new HackathonEmployeeWishListMappingConfiguration());
        modelBuilder.ApplyConfiguration(new WishListConfiguration());
        modelBuilder.ApplyConfiguration(new HackathonDreamTeamConfiguration());
    }
}
