using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpBatch.Core.Options;
using SharpBatch.Core.Services;
using SharpBatch.Core.Workers;
using System.Collections.Generic;
using System.Linq;

namespace SharpBatch.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSharpBatch<T>(
             this IServiceCollection services)
            where T : class, IQueueService
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var backgroundOption = config.GetSection(BackgroundOptions.Section).Get<BackgroundOptions>();

            services.Configure<BackgroundOptions>(x =>
            {
                x.Queues = backgroundOption.Queues;
                x.ServerWorkerRefresh = backgroundOption.ServerWorkerRefresh;
                x.Workers = backgroundOption.Workers;
            });

            services.AddSingleton<IQueueService, T>();

            services.AddSingleton<IWorkerService, WorkerService>();

            services.AddHostedService<QueuedHostedService>();

            return services;
        }
    }
}
