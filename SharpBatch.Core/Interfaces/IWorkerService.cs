using System;
using System.Threading;

namespace SharpBatch.Core.Interfaces
{
    public interface IWorkerService : IDisposable
    {
        Thread CreateWorker(CancellationToken stoppingToken, string queueName);
    }
}
