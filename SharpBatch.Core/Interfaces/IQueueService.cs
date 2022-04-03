using SharpBatch.Core.Options;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.Core.Interfaces
{
    public interface IQueueService
    {
        IEnumerable<Queue> Queues { get; }
        string GetByStrategy();
        void Free(string key);
    }
}
