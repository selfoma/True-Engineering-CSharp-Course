using EveryoneToTheHackathon.Options;
using log4net;
using EveryoneToTheHackathon.HackathonParticipants;

namespace EveryoneToTheHackathon.HR;

public class HRManager
{
    private static readonly ILog Logger = LogManager.GetLogger(nameof(HRManager));

    public List<HackathonParticipant> JuniorsList { get; }
    public List<HackathonParticipant> TeamLeadsList { get; }
    
    public HRManager()
    {
        JuniorsList = CSVParticipantsReader.ReadParticipants(ConfigOptions.JuniorsListPath);
        TeamLeadsList = CSVParticipantsReader.ReadParticipants(ConfigOptions.TeamLeadsListPath);
        
        Logger.Info("HR manager formed participants lists.");
    }

    public void AskParticipantsWishLists()
    {
        LetParticipantsPrepareWishList(JuniorsList, TeamLeadsList);
        LetParticipantsPrepareWishList(TeamLeadsList, JuniorsList);
        
        Logger.Info("Participants provided their wish lists.");
    }

    private void LetParticipantsPrepareWishList(List<HackathonParticipant> participants, List<HackathonParticipant> candidates)
    {
        foreach (var participant in participants)
        {
            participant.PrepareWishList(candidates);
        }
    }
}