using EveryoneToTheHackathon.Domain.Entities;

namespace EveryoneToTheHackathon.Infrastructure.Dtos;

public record EmployeeResponseDto(
    int EmployeeId,
    string FullName,
    EmployeeRole Role,
    Guid HackathonId,
    List<WishListDto> WishListDtos);