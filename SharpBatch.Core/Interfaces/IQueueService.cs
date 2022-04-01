using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;

namespace SharpBatch.Core.Interfaces
{
    public interface IQueueService : IDisposable
    {
        IEnumerable<QueueOptions> Queues { get; }
    }
}
