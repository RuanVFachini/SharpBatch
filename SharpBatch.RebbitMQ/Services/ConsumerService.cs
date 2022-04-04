using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharpBatch.Core.Interfaces;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ.Consumer.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IChannelService _channelService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;


        public ConsumerService(
            IChannelService channelService,
            IServiceProvider serviceProvider)
        {
            _channelService = channelService;
            _serviceProvider = serviceProvider;
            _channel = _channelService.GenerateChannel();
        }

        public virtual Task GetConsumerTask(string queueName)
        {
            var consumer = CreateConsummer();

            return new Task(() => _channel.BasicConsume(queueName, false, consumer));
        }

        protected virtual AsyncEventingBasicConsumer CreateConsummer()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, message) =>
            {
                try
                {
                    var scope = _serviceProvider.CreateScope();

                    await scope.ServiceProvider.GetRequiredService<IProcessorManager>()
                        .ExecuteAsync(message);

                    _channel.BasicAck(message.DeliveryTag, true);

                }
                catch (Exception e)
                {
                    _channel.BasicReject(message.DeliveryTag, true);
                }
            };

            return consumer;
        }

        public void Dispose()
        {
            _channelService.Dispose();
        }
    }
}
