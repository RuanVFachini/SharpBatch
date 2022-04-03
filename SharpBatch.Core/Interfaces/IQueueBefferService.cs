using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBatch.Core.Interfaces
{
    public interface IQueueBefferService : IDisposable
    {
        Task<Task> QueueAsync(string queueName, Action<CancellationToken> action);

        ValueTask<KeyValuePair<string, Task>> DequeueAsync();
        void Free(string key);
    }
}
