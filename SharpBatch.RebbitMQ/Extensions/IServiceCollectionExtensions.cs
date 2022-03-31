using Microsoft.Extensions.DependencyInjection;
using SharpBatch.Core.Extensions;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
using SharpBatch.RebbitMQ.Consumer.Services;
using SharpBatch.RebbitMQ.Core.Extensions;

namespace SharpBatch.RebbitMQ.Consumer.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSharpBatchRabbitMQConsumer(this IServiceCollection services)
        {
            services.AddScoped<IConsumerService, ConsumerService>();

            services.ConfigureSharpBatchRabbitCore();

            return services.ConfigureSharpBatch<QueueService>();
        }
    }
}
