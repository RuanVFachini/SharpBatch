using SharpBatch.Core.Options;
using System.Collections.Generic;

namespace SharpBatch.Core.Interfaces
{
    public interface IQueueService
    {
        IEnumerable<QueueOptions> Queues { get; }
    }
}
