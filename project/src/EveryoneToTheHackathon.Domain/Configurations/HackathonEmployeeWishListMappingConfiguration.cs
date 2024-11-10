using EveryoneToTheHackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EveryoneToTheHackathon.Domain.Configurations;

public class HackathonEmployeeWishListMappingConfiguration : IEntityTypeConfiguration<HackathonEmployeeWishListMapping>
{
    public void Configure(EntityTypeBuilder<HackathonEmployeeWishListMapping> builder)
    {
        builder.HasKey(m => m.MappingId);

        builder
            .HasOne(m => m.Employee)
            .WithMany(e => e.HackathonEmployeeWishListMappings)
            .HasForeignKey(m => new { m.EmployeeId, m.EmployeeRole })
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(m => m.Hackathon)
            .WithMany(h => h.HackathonEmployeeWishListMappings)
            .HasForeignKey(m => m.HackathonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(m => new { m.EmployeeId, m.EmployeeRole, m.HackathonId })
            .IsUnique();
    }
}