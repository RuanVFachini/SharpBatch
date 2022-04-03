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
        private readonly IQueueService _queueStrategyService;

        public QueueBefferService(IQueueService queueStrategyService)
        {
            _queueStrategyService = queueStrategyService;

            foreach (var queue in _queueStrategyService.Queues)
            {
                var channelOptions = new BoundedChannelOptions(int.MaxValue)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                };

                var channelQueue = Channel.CreateBounded<Task>(channelOptions);

                _executionQueue.TryAdd(queue.Name, channelQueue);
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

        public async ValueTask<KeyValuePair<string, Task>> DequeueAsync()
        {
            var queueName = _queueStrategyService.GetByStrategy();
            var workItem = await _executionQueue[queueName].Reader.ReadAsync();

            return new KeyValuePair<string, Task>(queueName, workItem);
        }

        public void Free(string key)
        {
            _queueStrategyService.Free(key);
        }

        public void Dispose()
        {
            _executionQueue.Clear();
        }
    }
}
