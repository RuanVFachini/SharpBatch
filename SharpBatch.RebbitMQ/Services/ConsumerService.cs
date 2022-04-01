using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharpBatch.Core.Exceptions;
using SharpBatch.Core.Interfaces;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;

namespace SharpBatch.RebbitMQ.Consumer.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IQueueBefferService _queueBefferService;
        private readonly IChannelService _channelService;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public ConsumerService(
            IQueueBefferService queueBefferService,
            IChannelService channelService,
            IServiceProvider serviceProvider)
        {
            _queueBefferService = queueBefferService;
            _channelService = channelService;
            _serviceProvider = serviceProvider;
        }

        public virtual void RegisterConsummers()
        {
            _channel = _channelService.GenerateChannel();

            foreach (var queue in _channelService.Queues)
            {
                var consumer = GenerateGenericAsyncConsumer(queue.Name);
                _channel.BasicConsume(queue.Name, true, consumer);
            }
        }

        protected virtual AsyncEventingBasicConsumer GenerateGenericAsyncConsumer(string name)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, message) =>
            {
                try
                {
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
                    if (e is QueueConsumeException)
                        throw e;

                    ThrowQueueServiceConsumeException(e);
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

        public void Dispose()
        {
            _channel.Close();
            _channel.Dispose();
            _queueBefferService.Dispose();
            _channelService.Dispose();
        }
    }
}
