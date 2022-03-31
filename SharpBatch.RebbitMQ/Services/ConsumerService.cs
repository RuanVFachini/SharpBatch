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

        public ConsumerService(
            IQueueBefferService queueBefferService,
            IChannelService channelService)
        {
            _queueBefferService = queueBefferService;
            _channelService = channelService;
        }

        public virtual void RegisterConsummers()
        {
            var channel = _channelService.GenerateChannel();

            foreach (var queue in _channelService.Queues)
            {
                var consumer = GenerateGenericAsyncConsumer(channel, queue.Name);
                channel.BasicConsume(queue.Name, true, consumer);
            }
        }

        protected virtual AsyncEventingBasicConsumer GenerateGenericAsyncConsumer(IModel channel, string name)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, message) =>
            {
                try
                {
                    var taskawait = await _queueBefferService.QueueAsync(name, x =>
                    {

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
    }
}
