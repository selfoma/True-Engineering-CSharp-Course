using log4net;

namespace EveryoneToTheHackathon.HR;

public class Hackathon
{

    private static readonly HRManager _hrManager = new();
    
    private readonly DreamTeamList _dreamTeamList;
    private readonly ILog _logger = LogManager.GetLogger(nameof(Hackathon));
    
    public double HarmonicMean { get; private set; }

    public Hackathon()
    {
        _logger.Info("Hackathon is opened");
        _dreamTeamList = _hrManager.MakeDreamTeamList();
        _logger.Info("Dream team list is formed.");
    }

    public void Hold()
    {
        HarmonicMean = _dreamTeamList.ComputeHarmonicMean();
    }

    public override string? ToString()
    {
        return $"Harmonic: {HarmonicMean}";
    }
    
}