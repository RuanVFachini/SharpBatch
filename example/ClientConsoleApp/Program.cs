using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SharpBatch.RebbitMQ.Client.Extensions;
using SharpBatch.RebbitMQ.Client.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using SharpBatch.RebbitMQ.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Iniciando serviço");

            using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {

                services.ConfigureSharpBatchRabbitMQClient();
            })
            .Build();

            await host.StartAsync();

            var clientService = host.Services.GetRequiredService<IClientQueueService>();

            while (true)
            {
                await clientService.EnqueueAsync(
                        new DefaultMessage(
                            "amq.direct",
                            "jobs",
                            Encoding.ASCII.GetBytes("teste")
                            ));

                Thread.Sleep(1000);
            }

            Console.WriteLine("Rodando serviço");

            await host.WaitForShutdownAsync();
            Console.WriteLine("Parando serviço");


        }
    }

}
