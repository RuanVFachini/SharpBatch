using Microsoft.Extensions.Logging;
using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Services;
using System;
using System.Threading;

namespace SharpBatch.Core.Workers
{
    public class WorkerService : IWorkerService
    {
        private readonly IQueueBefferService _queueBefferService;
        private readonly ILogger<IWorkerService> _logger;

        public WorkerService(
            IQueueBefferService service,
            ILogger<IWorkerService> logger)
        {
            _queueBefferService = service;
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
                            var workItem = _queueBefferService.DequeueAsync("default", stoppingToken).GetAwaiter().GetResult();

                            try
                            {
                                workItem(stoppingToken).Wait();
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

        public void Dispose()
        {
            _queueBefferService.Dispose();
        }
    }
}
