using EveryoneToTheHackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EveryoneToTheHackathon.Domain.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => new { e.EmployeeId, e.Role });

        builder
            .Property(e => e.Role)
            .HasConversion<string>();

        builder
            .Property(e => e.FullName)
            .HasMaxLength(50);
    }
}