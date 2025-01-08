using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Dtos;

namespace EveryoneToTheHackathon.Infrastructure.Messages;

public record WishListFormed(
    int EmployeeId,
    string FullName,
    EmployeeRole Role,
    Guid HackathonId,
    List<WishListDto> WishListDtos)
{
    public static EmployeeResponseDto ToEmployeeResponse(WishListFormed w)
    {
        return new EmployeeResponseDto(
            w.EmployeeId,
            w.FullName, 
            w.Role, 
            w.HackathonId, 
            w.WishListDtos);
    }
}