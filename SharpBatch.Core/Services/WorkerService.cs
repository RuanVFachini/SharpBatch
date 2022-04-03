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
        private readonly IQueueBefferService _queueBefferService;
        private readonly ILogger<IWorkerService> _logger;

        public WorkerService(
            IQueueBefferService service,
            ILogger<IWorkerService> logger)
        {
            _queueBefferService = service;
            _logger = logger;
        }
        public Thread CreateWorker(CancellationToken stoppingToken, string queueName)
        {
            return 
                new Thread(() =>
                {
                    Task.Run(async () =>
                    {
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                var workItem = await _queueBefferService.DequeueAsync(queueName);

                                try
                                {
                                    workItem.Start();
                                    workItem.Wait(stoppingToken);
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
                    }).Wait();
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
