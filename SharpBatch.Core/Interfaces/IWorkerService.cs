using System.Threading;

namespace SharpBatch.Core.Interfaces
{
    public interface IWorkerService
    {
        Thread CreateWorker(CancellationToken stoppingToken);
    }
}
