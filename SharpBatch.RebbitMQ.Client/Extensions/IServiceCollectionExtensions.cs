using Microsoft.Extensions.DependencyInjection;
using SharpBatch.RebbitMQ.Client.Interfaces;
using SharpBatch.RebbitMQ.Core.Extensions;

namespace SharpBatch.RebbitMQ.Client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSharpBatchRabbitMQClient(this IServiceCollection services)
        {
            services.AddSingleton<IClientQueueService, ClientQueueService>();
            services.ConfigureSharpBatchRabbitCore();

            return services;
        }
    }
    
}
