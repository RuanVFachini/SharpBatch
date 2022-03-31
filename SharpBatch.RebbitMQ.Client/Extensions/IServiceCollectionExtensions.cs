using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.RebbitMQ.Client.Extensions
{
    public static IServiceCollection ConfigureSharpBatchRabbitMQClient(this IServiceCollection services)
    {
        services.ConfigureSharpBatchRabbitCore();

        return services.ConfigureSharpBatch<QueueService>();
    }
}
