
using Microsoft.Extensions.Options;
using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpBatch.Core.Services
{
    public class QueueService : IQueueService
    {
        public IEnumerable<Queue> Queues { get; }
        private readonly int MaxParallel;
        private readonly Queue<string> _queueStrategyHistory;

        public QueueService(IOptions<QueueOptions> options)
        {
            Queues = options.Value.Queues;
            MaxParallel = options.Value.Queues.Sum(x => x.MaxParallel);
            _queueStrategyHistory = new Queue<string>(MaxParallel);
        }

        public string GetByStrategy()
        {
            throw new NotImplementedException();
        }

        public void Free(string key)
        {
            throw new NotImplementedException();
        }
    }

}
