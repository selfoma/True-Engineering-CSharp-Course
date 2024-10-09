using EveryoneToTheHackathon.HackathonParticipants;
using EveryoneToTheHackathon.Options;
using EveryoneToTheHackathon.Strategy;
using log4net;

namespace EveryoneToTheHackathon.HR;

public class Hackathon
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(Hackathon));

    private ITeamBuildingStrategy _buildingStrategy;
    
    private readonly IHRManager _hrManager;
    
    private List<HackathonParticipant> _juniors;
    private List<HackathonParticipant> _teamLeads;
    private DreamTeamList _dreamTeam;
    
    public double HarmonicMean { get; private set; }

    public Hackathon(IHRManager hrManager)
    {
        _hrManager = hrManager;
        _juniors = CSVParticipantsReader.ReadParticipants(ConfigOptions.JuniorsListPath);
        _teamLeads = CSVParticipantsReader.ReadParticipants(ConfigOptions.TeamLeadsListPath);
    }

    public void Hold()
    {
        Logger.Info("Holding hackathon");

        _hrManager.AskParticipantsWishLists(_juniors, _teamLeads);
        _hrManager.AskParticipantsWishLists(_teamLeads, _juniors);
        
        _dreamTeam = _hrManager.BuildDreamTeam(_juniors, _teamLeads);
        HarmonicMean = _dreamTeam.ComputeHarmonicMean();
    }

    public override string? ToString()
    {
        return $"Harmonic: {HarmonicMean}";
    }
    
}