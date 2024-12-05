namespace EveryoneToTheHackathon.Domain.Entities;

public class Hackathon
{
    public Guid HackathonId { get; init; } = Guid.NewGuid();
    
    public decimal HarmonicMean { get; set; }
    
    public List<HackathonEmployeeWishListMapping> HackathonEmployeeWishListMappings { get; set; } = new();
    
    public List<HackathonDreamTeam> HackathonDreamTeams { get; set; } = new();

    public override string ToString()
    {
        return $"Harmonic: { HarmonicMean } ";
    }
}