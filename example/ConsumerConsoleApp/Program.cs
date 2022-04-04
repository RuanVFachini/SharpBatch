using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using SharpBatch.RebbitMQ.Consumer.Extensions;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
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
                services.ConfigureSharpBatchRabbitMQConsumer<ProcessorManager>();
            })
            .Build();

            await host.StartAsync();

            await host.WaitForShutdownAsync();
        }
    }

    public class ProcessorManager : IProcessorManager
    {
        public Task ExecuteAsync(BasicDeliverEventArgs message)
        {
            throw new NotImplementedException();
        }
    }
}
