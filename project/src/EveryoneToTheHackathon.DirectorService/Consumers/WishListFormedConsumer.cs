using System.Collections.Concurrent;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.Messages;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using log4net;
using MassTransit;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.DirectorService.Consumers;

public class WishListFormedConsumer(
    IOptions<ConfigOptions> options,
    IDirectorService directorService,
    IBackgroundTaskQueue<BaseTaskModel> backgroundTaskQueue) : IConsumer<WishListFormed>
{
    private static readonly ConcurrentBag<WishListFormed> EmployeeResponses = new();
    private static int _responseCount;
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(WishListFormedConsumer));
    
    public Task Consume(ConsumeContext<WishListFormed> context)
    {
        var wishListFormed = context.Message;
        EmployeeResponses.Add(wishListFormed);
        Interlocked.Increment(ref _responseCount);
        Logger.Info($"HandleEmployeeRequest: Employee: [ID].{ wishListFormed.EmployeeId } - [R].{ wishListFormed.Role }.");
        if (Interlocked.CompareExchange(ref _responseCount, 0, options.Value.Hackathon!.ParticipantsCount) 
            == options.Value.Hackathon!.ParticipantsCount)
        {
            var responses = EmployeeResponses
                .Select(WishListFormed.ToEmployeeResponse).ToList();
            directorService.HandleWishLists(responses);
            EmployeeResponses.Clear();
            backgroundTaskQueue.EnqueueAsync(new("Manage preferences: [Assignee].DirectorService."));
        }
        return Task.CompletedTask;
    }
}