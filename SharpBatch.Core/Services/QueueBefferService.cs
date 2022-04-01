using SharpBatch.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SharpBatch.Core.Services
{
    public class QueueBefferService : IQueueBefferService
    {
        private readonly ConcurrentDictionary<string, Channel<Func<CancellationToken, Task>>> _executionQueue =
            new ConcurrentDictionary<string, Channel<Func<CancellationToken, Task>>>();

        public async Task<Task> QueueAsync(
            string queueName,
            Action<CancellationToken> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Task task = null;

            await _executionQueue[queueName].Writer.WriteAsync(x => task = new Task(() => action(x), x));

            return task;
        }

        public async ValueTask<Func<CancellationToken, Task>> DequeueAsync(
            string queueName,
            CancellationToken cancellationToken)
        {
            return await _executionQueue[queueName].Reader.ReadAsync();
        }

        public void Dispose()
        {
            _executionQueue.Clear();
        }
    }
}
