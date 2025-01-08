using EveryoneToTheHackathon.Domain.Entities;

namespace EveryoneToTheHackathon.Infrastructure.Dtos;

public record DreamTeamDto(
    int JuniorId,
    List<WishListDto> JuniorWishLists,
    int TeamLeadId,
    List<WishListDto> TeamLeadWishLists,
    Guid HackathonId)
{
    public static HackathonDreamTeam FromDto(DreamTeamDto dto)
    {
        return new HackathonDreamTeam
        {
            JuniorId = dto.JuniorId,
            TeamLeadId = dto.TeamLeadId,
            HackathonId = dto.HackathonId
        };
    }
}