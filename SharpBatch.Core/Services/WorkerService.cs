using Microsoft.Extensions.Logging;
using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBatch.Core.Workers
{
    public class WorkerService : IWorkerService
    {
        private readonly ILogger<IWorkerService> _logger;
        private readonly IQueueService _queueService;

        public WorkerService(
            ILogger<IWorkerService> logger,
            IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }
        public virtual Thread CreateWorker(CancellationToken stoppingToken)
        {
            return 
                new Thread(() =>
                {
                    try
                    {
                        var workItem = _queueService.DequeueTask();

                        if (workItem != null)
                        {
                            workItem.Start();

                            workItem.Wait(stoppingToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Error occurred executing Background Server.");
                    }
                })
                {
                    IsBackground = true
                };
        }

        public void Dispose()
        {
            _queueService.Dispose();
        }
    }
}
