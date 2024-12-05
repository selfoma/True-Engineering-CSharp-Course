using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using EveryoneToTheHackathon.Infrastructure.Strategies;
using log4net;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IManagerService
{
    List<DreamTeamDto> ManageTeams();
    void SplitResponses(List<EmployeeResponseDto> responses);
}

public class ManagerService(ITeamBuildingStrategy strategy) : IManagerService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ManagerService));
    
    private readonly List<EmployeeResponseDto> _juniorsDto = new();
    private readonly List<EmployeeResponseDto> _teamLeadsDto = new();
    
    public List<DreamTeamDto> ManageTeams()
    {
        return strategy.BuildTeams(_juniorsDto, _teamLeadsDto);
    }

    public void SplitResponses(List<EmployeeResponseDto> responses)
    {
        _juniorsDto.Clear();
        _teamLeadsDto.Clear();
        
        _juniorsDto.AddRange(responses
            .Select(r => r)
            .Where(r => r.Role == EmployeeRole.Junior)
            .ToList()
        );
        
        _teamLeadsDto.AddRange(responses
            .Select(r => r)
            .Where(r => r.Role == EmployeeRole.TeamLead)
            .ToList()
        );
        
        Logger.Info($"SplitResponses: [JC].{ _juniorsDto.Count } - [TLC].{ _teamLeadsDto.Count }");
    }
}