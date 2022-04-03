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
        private readonly IWorkerService _workerService;
        private readonly List<Thread> ThreadList = new List<Thread>();

        public QueuedHostedService(
            ILogger<QueuedHostedService> logger,
            IOptions<BackgroundOptions> options,
            IWorkerService workerService)
        {
            _logger = logger;
            _backgroundOptions = options.Value;
            _workerService = workerService;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
                            var thread = _workerService.CreateWorker(stoppingToken);

                            ThreadList.Add(thread) ;

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
            ThreadList.RemoveAll(x => !x.IsAlive);

            return _backgroundOptions.Workers - ThreadList.Count > 0;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _workerService.Dispose();
            ThreadList.ForEach(x => x.Abort());
            ThreadList.Clear();
            base.Dispose();
        }
    }
}
