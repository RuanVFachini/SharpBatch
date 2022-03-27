using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBatch.Core.Services
{
    public interface IQueueService
    {
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
            string queueName,
            CancellationToken cancellationToken);
    }
}
