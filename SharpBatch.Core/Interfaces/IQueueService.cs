using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.Core.Interfaces
{
    public interface IQueueService : IDisposable
    {
        Task DequeueTask();
    }
}
