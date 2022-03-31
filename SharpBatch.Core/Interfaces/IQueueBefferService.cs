using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBatch.Core.Interfaces
{
    public interface IQueueBefferService
    {
        Task<Task> QueueAsync(string queueName, Action<CancellationToken> action);

        ValueTask<Func<CancellationToken, Task>> DequeueAsync(
            string queueName,
            CancellationToken cancellationToken);
    }
}
