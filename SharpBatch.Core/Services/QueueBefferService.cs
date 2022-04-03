using Microsoft.Extensions.Options;
using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SharpBatch.Core.Services
{
    public class QueueBefferService : IQueueBefferService
    {
        private readonly ConcurrentDictionary<string, Channel<Task>> _executionQueue =
            new ConcurrentDictionary<string, Channel<Task>>();

        public IEnumerable<string> Queues { get; }

        public QueueBefferService(IOptions<QueueOptions> options)
        {
            Queues = options.Value.Queues.Select(x => x.Name);

            foreach (var queue in Queues)
            {
                var channelOptions = new BoundedChannelOptions(int.MaxValue)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                };

                var channelQueue = Channel.CreateBounded<Task>(channelOptions);

                _executionQueue.TryAdd(queue, channelQueue);
            }
        }

        public async Task<Task> QueueAsync(
            string queueName,
            Action<CancellationToken> action)
        {

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Task task = new Task(() =>
            {
                action(Task.Factory.CancellationToken);
            });

            await _executionQueue[queueName].Writer.WriteAsync(task);

            return task;
        }

        public async ValueTask<Task> DequeueAsync(
            string queueName)
        {
            var workItem = await _executionQueue[queueName].Reader.ReadAsync();

            return workItem;
        }

        public void Dispose()
        {
            _executionQueue.Clear();
        }
    }
}
