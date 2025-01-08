using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Infrastructure.Dtos;
using EveryoneToTheHackathon.Infrastructure.Messages;
using EveryoneToTheHackathon.Infrastructure.Services;
using log4net;
using MassTransit;
using HackathonStarted = EveryoneToTheHackathon.Infrastructure.Messages.HackathonStarted;

namespace EveryoneToTheHackathon.EmployeeService.Consumers;

public class HackathonStartedConsumer(    
    IBus bus,
    IEmployeeService employeeService) : IConsumer<HackathonStarted>
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HackathonStartedConsumer));

    public Task Consume(ConsumeContext<HackathonStarted> context)
    {
        Logger.Info($"HackathonStartedConsumer: received message - [M].{context.Message}");
        employeeService.HandleParticipantsList(context.Message.HackathonId);
        employeeService.PrepareWishLists();
        PublishWishLists(employeeService.GetThisEmployee()!);
        return Task.CompletedTask;
    }

    private void PublishWishLists(Employee employee)
    {
        var message = new WishListFormed(
            EmployeeId: employee.EmployeeId,
            FullName: employee.FullName,
            Role: employee.Role,
            HackathonId: employee.HackathonEmployeeWishListMappings.Last().HackathonId,
            WishListDtos: employee
                .HackathonEmployeeWishListMappings
                .Last()
                .WishLists
                .Select(WishListDto.ToDto)
                .ToList());
        bus.Publish(message);
        Logger.Info("Publish done.");
    }
}