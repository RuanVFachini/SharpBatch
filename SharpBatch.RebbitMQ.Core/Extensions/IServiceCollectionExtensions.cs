using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpBatch.RebbitMQ.Core.Interfaces;
using SharpBatch.RebbitMQ.Core.Options;
using SharpBatch.RebbitMQ.Core.Services;

namespace SharpBatch.RebbitMQ.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSharpBatchRabbitCore(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var rabbitMQOptions = config.GetSection(RabbitMQOptions.Section).Get<RabbitMQOptions>();

            services.Configure<RabbitMQOptions>(x =>
            {
                x.VirtualHost = rabbitMQOptions.VirtualHost;
                x.HostName = rabbitMQOptions.HostName;
                x.UserName = rabbitMQOptions.UserName;
                x.Password = rabbitMQOptions.Password;
            });

            services.AddScoped<IChannelService, ChannelService>();

            return services;
        }
    }
}
