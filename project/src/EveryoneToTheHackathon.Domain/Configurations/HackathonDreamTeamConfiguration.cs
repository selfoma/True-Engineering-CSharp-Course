using EveryoneToTheHackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EveryoneToTheHackathon.Domain.Configurations;

public class HackathonDreamTeamConfiguration : IEntityTypeConfiguration<HackathonDreamTeam>
{
    public void Configure(EntityTypeBuilder<HackathonDreamTeam> builder)
    {
        builder.HasKey(dt => new { dt.HackathonId, dt.TeamLeadId, dt.JuniorId });
        
        builder
            .HasOne(dt => dt.TeamLead)
            .WithMany(tl => tl.DreamTeamLeads)
            .HasForeignKey(dt => new { dt.TeamLeadId, dt.TeamLeadRole})
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(dt => dt.Junior)
            .WithMany(j => j.DreamTeamJuniors)
            .HasForeignKey(dt => new { dt.JuniorId, dt.JuniorRole })
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(dt => dt.Hackathon)
            .WithMany(h => h.HackathonDreamTeams)
            .HasForeignKey(dt => dt.HackathonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}