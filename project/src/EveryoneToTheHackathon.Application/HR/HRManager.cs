using EveryoneToTheHackathon.Options;
using log4net;
using EveryoneToTheHackathon.HackathonParticipants;
using EveryoneToTheHackathon.Strategy;

namespace EveryoneToTheHackathon.HR;

public interface IHRManager
{
    void AskParticipantsWishLists(List<HackathonParticipant> participants, List<HackathonParticipant> candidates);
    
    DreamTeamList BuildDreamTeam(List<HackathonParticipant> juniors, List<HackathonParticipant> teamLeads);
}

public class HRManager : IHRManager
{
    private static readonly ILog Logger = LogManager.GetLogger(nameof(HRManager));

    private readonly ITeamBuildingStrategy _buildingStrategy;
    
    public HRManager(ITeamBuildingStrategy buildingStrategy)
    {
        _buildingStrategy = buildingStrategy;
    }

    public void AskParticipantsWishLists(List<HackathonParticipant> participants, List<HackathonParticipant> candidates)
    {
        Logger.Info("Polling participants wish lists...");
        
        foreach (var participant in participants)
        {
            participant.PrepareWishList(candidates);
        }
    }

    public DreamTeamList BuildDreamTeam(List<HackathonParticipant> juniors, List<HackathonParticipant> teamLeads)
    {
        Logger.Info("Building dream team...");

        return _buildingStrategy.MakeDreamTeamList(juniors, teamLeads);
    }
}