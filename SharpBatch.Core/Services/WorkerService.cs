using Microsoft.Extensions.Logging;
using SharpBatch.Core.Services;
using System;
using System.Threading;

namespace SharpBatch.Core.Workers
{
    public class WorkerService : IWorkerService
    {
        private readonly IQueueService _taskQueueService;
        private readonly ILogger<IWorkerService> _logger;

        public WorkerService(
            IQueueService service,
            ILogger<IWorkerService> logger)
        {
            _taskQueueService = service;
            _logger = logger;
        }
        public Thread CreateWorker(CancellationToken stoppingToken)
        {
            return 
                new Thread(() =>
                {
                    while(!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var workItem = _taskQueueService.DequeueAsync(stoppingToken).GetAwaiter().GetResult();

                            try
                            {
                                workItem(stoppingToken).AsTask().Wait();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex,
                                    "Error occurred executing Background Server.");
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                })
                {
                    IsBackground = true
                };
        }
    }
}
