using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharpBatch.Core.Exceptions;
using SharpBatch.Core.Interfaces;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ.Consumer.Services
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IQueueBefferService _queueBefferService;
        private readonly IChannelService _channelService;
        private readonly IServiceProvider _serviceProvider;
        
        public ConsumerHostedService(
            IQueueBefferService queueBefferService,
            IChannelService channelService,
            IServiceProvider serviceProvider)
        {
            _queueBefferService = queueBefferService;
            _channelService = channelService;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                RegisterConsummers();

                while (!stoppingToken.IsCancellationRequested)
                {

                }
            });
            
        }

        protected virtual void RegisterConsummers()
        {
            var channel = _channelService.GenerateChannel();

            foreach (var queue in _queueBefferService.Queues)
            {
                var consumer = GenerateGenericAsyncConsumer(channel, queue.Name);
                channel.BasicConsume(queue.Name, false, consumer);
            }
        }

        protected virtual AsyncEventingBasicConsumer GenerateGenericAsyncConsumer(IModel channel, string name)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, message) =>
            {
                try
                {
                    channel.BasicAck(message.DeliveryTag, true);

                    var taskawait = await _queueBefferService.QueueAsync(name, x =>
                    {
                        var scope = _serviceProvider.CreateScope();

                        scope.ServiceProvider.GetRequiredService<IProcessorManager>().Execute(message);

                    });

                    taskawait.Wait();

                    if (taskawait.IsCompleted && (taskawait.IsCanceled || taskawait.IsFaulted))
                    {
                        ThrowQueueServiceConsumeException(taskawait.Exception);
                    }

                }
                catch (Exception e)
                {
                    channel.BasicReject(message.DeliveryTag, true);
                }
            };

            return consumer;
        }

        protected virtual void ThrowQueueServiceConsumeException(Exception e)
        {
            if (e != null)
                throw new QueueConsumeException(e);

            throw new QueueConsumeException("Unexpected background consumer error");
        }

        public override void Dispose()
        {
            _queueBefferService.Dispose();
            _channelService.Dispose();

            base.Dispose();
        }
    }
}
