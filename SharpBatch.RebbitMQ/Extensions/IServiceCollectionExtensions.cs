using Microsoft.Extensions.DependencyInjection;
using SharpBatch.Core.Extensions;
using SharpBatch.Core.Interfaces;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
using SharpBatch.RebbitMQ.Consumer.Services;
using SharpBatch.RebbitMQ.Core.Extensions;

namespace SharpBatch.RebbitMQ.Consumer.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSharpBatchRabbitMQConsumer<T>(this IServiceCollection services)
            where T : class, IProcessorManager
        {
            services.AddScoped<IProcessorManager, T>();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.ConfigureSharpBatchRabbitCore();

            return services;

            
        }
    }
}
