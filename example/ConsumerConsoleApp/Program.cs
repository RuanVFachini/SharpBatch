using Microsoft.Extensions.Hosting;
using SharpBatch.RebbitMQ.Consumer.Extensions;
using System;
using System.Threading.Tasks;

namespace ConsumerConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureSharpBatchRabbitMQConsumer();
            })
            .Build();

            await host.StartAsync();

            await host.WaitForShutdownAsync();
        }
    }
}
