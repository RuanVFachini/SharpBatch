using Microsoft.Extensions.Hosting;
using SharpBatch.RebbitMQ.Client.Extensions;
using System;
using System.Threading.Tasks;

namespace ClientConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureSharpBatchRabbitMQClient();
            })
            .Build();

            await host.StartAsync();

            await host.WaitForShutdownAsync();
        }
    }
}
