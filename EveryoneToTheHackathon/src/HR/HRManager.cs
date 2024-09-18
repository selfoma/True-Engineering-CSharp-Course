using EveryoneToTheHackathon.Options;
using log4net;
using EveryoneToTheHackathon.HackathonParticipants;

namespace EveryoneToTheHackathon.HR;

public class HRManager
{

    private readonly List<HackathonParticipant> _juniorsList;
    private readonly List<HackathonParticipant> _teamLeadsList;
    
    private readonly ILog _logger = LogManager.GetLogger(nameof(HRManager));
    
    public HRManager()
    {
        _juniorsList = CSVParticipantsReader.ReadParticipants(ConfigOptions.JuniorsListPath);
        _teamLeadsList = CSVParticipantsReader.ReadParticipants(ConfigOptions.TeamLeadsListPath);
        
        _logger.Info("HR manager formed participants lists.");
    }

    public DreamTeamList MakeDreamTeamList()
    {
        LetParticipantsPrepareWishList(_juniorsList, _teamLeadsList);
        LetParticipantsPrepareWishList(_teamLeadsList, _juniorsList);
        
        Random rand = new();
        List<int> distribution = new();
        distribution.AddRange(Enumerable.Range(0, _juniorsList.Count).OrderBy(i => rand.Next()));

        List<Tuple<HackathonParticipant, HackathonParticipant>> dreamTeam = new();
        for (int i = 0; i < _juniorsList.Count; i++)
        {
            dreamTeam.Add(new(_juniorsList[i], _teamLeadsList[distribution[i]]));
        }

        return new(dreamTeam);
    }

    private void LetParticipantsPrepareWishList(List<HackathonParticipant> participants, List<HackathonParticipant> candidates)
    {
        foreach (var participant in participants)
        {
            participant.PrepareWishList(candidates);
        }
        _logger.Info("Participants provided their wish lists.");
    }
}