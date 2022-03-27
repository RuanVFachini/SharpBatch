using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharpBatch.Core.Options;
using SharpBatch.Core.Services;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ
{
    public class QueueService : IQueueService
    {
        private readonly ConcurrentDictionary<string, Channel<Func<CancellationToken, ValueTask>>> _executionQueue = 
            new ConcurrentDictionary<string, Channel<Func<CancellationToken, ValueTask>>>();


        public QueueService(IOptions<BackgroundOptions> options)
        {
            ConnectionFactory factory = new ConnectionFactory();
            
            factory.UserName = String.Empty;
            factory.Password = String.Empty;
            factory.VirtualHost = String.Empty;
            factory.HostName = String.Empty;
            factory.DispatchConsumersAsync = true;

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            //bool autoAck = false;
            //BasicGetResult result = channel.BasicGet(String.Empty, autoAck);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, message) =>
            {
                
            };
        }

        private async ValueTask QueueAsync(
            string queueName,
            Func<CancellationToken, ValueTask> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _executionQueue[queueName].Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
            string queueName,
            CancellationToken cancellationToken)
        {
            return await _executionQueue[queueName].Reader.ReadAsync();
        }
    }
}
