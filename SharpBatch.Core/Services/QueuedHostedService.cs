using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBatch.Core.Services
{
    public class QueuedHostedService : BackgroundService, IDisposable
    {
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly BackgroundOptions _backgroundOptions;
        private readonly QueueOptions _queueOptions;
        private readonly IWorkerService _workerService;
        private readonly Dictionary<string, Thread> ThreadQueueDictionary = new Dictionary<string, Thread>();

        public QueuedHostedService(
            ILogger<QueuedHostedService> logger,
            IOptions<BackgroundOptions> options,
            IOptions<QueueOptions> queueOptions,
            IWorkerService workerService)
        {
            _logger = logger;
            _backgroundOptions = options.Value;
            _queueOptions = queueOptions.Value;
            _workerService = workerService;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"Queued Hosted Service is running.{Environment.NewLine}" +
                $"{Environment.NewLine}Tap W to add a work item to the " +
                $"background queue.{Environment.NewLine}");

            await ExecuteInternalsAsync(stoppingToken);
        }

        private Task ExecuteInternalsAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        while (HasFreeWorker())
                        {
                            var queue = _queueOptions.Queues.FirstOrDefault(x => !ThreadQueueDictionary.ContainsKey(x.Name));

                            var thread = _workerService.CreateWorker(stoppingToken, queue.Name);

                            ThreadQueueDictionary.Add(queue.Name, thread) ;

                            thread.Start();
                        }

                        Thread.Sleep(_backgroundOptions.ServerWorkerRefresh);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Error occurred executing Background Server.");
                    }
                }
            });
        }

        private bool HasFreeWorker()
        {
            var toRemove = ThreadQueueDictionary.Where(x => !x.Value.IsAlive).Select(x => x.Key);

            foreach (var key in toRemove)
            {
                ThreadQueueDictionary.Remove(key);
            }

            return _backgroundOptions.Workers - ThreadQueueDictionary.Count > 0;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _workerService.Dispose();
            ThreadQueueDictionary.Clear();
            base.Dispose();
        }
    }
}
