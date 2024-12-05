using System.Threading.Channels;

namespace EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;

public interface IBackgroundTaskQueue<T>
{
    Task EnqueueAsync(T task);
    Task<T> DequeueAsync(CancellationToken cancellationToken);
}

public class BackgroundTaskQueue<T> : IBackgroundTaskQueue<T>
{
    private readonly Channel<T> _queue = Channel.CreateUnbounded<T>();
    
    public async Task EnqueueAsync(T task)
    {
        await _queue.Writer.WriteAsync(task);
    }

    public async Task<T> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}