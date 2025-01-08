using EveryoneToTheHackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EveryoneToTheHackathon.Domain.Configurations;

public class HackathonConfiguration : IEntityTypeConfiguration<Hackathon>
{
    public void Configure(EntityTypeBuilder<Hackathon> builder)
    {
        builder.HasKey(h => h.HackathonId);

        builder
            .Property(h => h.HarmonicMean)
            .HasPrecision(16, 12);
    }
}