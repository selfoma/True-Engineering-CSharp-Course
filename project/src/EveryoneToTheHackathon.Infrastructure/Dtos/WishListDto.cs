using EveryoneToTheHackathon.Domain.Entities;

namespace EveryoneToTheHackathon.Infrastructure.Dtos;

public record WishListDto(
    int PreferredEmployeeId,
    EmployeeRole PreferredEmployeeRole,
    int PreferenceValue)
{
    public static WishListDto ToDto(WishList wishList)
    {
        return new WishListDto
        (
            wishList.PreferredEmployeeId,
            wishList.PreferredEmployeeRole,
            wishList.PreferenceValue
        );
    }
}
