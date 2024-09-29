using EveryoneToTheHackathon.Strategy;
using log4net;

namespace EveryoneToTheHackathon.HR;

public class Hackathon
{

    private static readonly ILog Logger = LogManager.GetLogger(typeof(Hackathon));

    private readonly ITeamBuildingStrategy _buildingStrategy;
    
    public double HarmonicMean { get; private set; }

    public Hackathon(ITeamBuildingStrategy buildingStrategy)
    {
        _buildingStrategy = buildingStrategy;
    }

    public void Hold(HRManager manager)
    {
        Logger.Info("Holding hackathon");
        var juniors = manager.JuniorsList;
        var teamLeads = manager.TeamLeadsList;
        HarmonicMean = _buildingStrategy.MakeDreamTeamList(juniors, teamLeads).ComputeHarmonicMean();
    }

    public override string? ToString()
    {
        return $"Harmonic: {HarmonicMean}";
    }
    
}