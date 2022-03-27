using System.Threading;

namespace SharpBatch.Core.Services
{
    public interface IWorkerService
    {
        Thread CreateWorker(CancellationToken stoppingToken);
    }
}
