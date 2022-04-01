using RabbitMQ.Client;
using SharpBatch.Core.Options;
using SharpBatch.RebbitMQ.Client.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ.Client
{
    public class QueueService : IQueueService
    {
        private readonly IChannelService _channelService;
        private readonly IModel _channel;
        

        public IEnumerable<QueueOptions> Queues { get => _channelService.Queues; }

        public QueueService(
            IChannelService channelService)
        {
            _channelService = channelService;

            _channel = _channelService.GenerateChannel();
        }

        public Task EnqueueAsync(IDefaultMessage message)
        {
            return Task.Run(() =>
            {
                IBasicProperties props = _channel.CreateBasicProperties();

                props.ContentEncoding = message.ContentEncoding;
                props.ContentType= message.ContentType;
                props.DeliveryMode = message.DeliveryMode;
                props.Expiration = message.Expiration;
                props.Headers = message.Headers;
                props.MessageId = message.MessageId;
                props.Persistent= message.Persistent;
                props.Priority = message.Priority;
                props.ReplyTo = message.ReplyTo;
                props.ReplyToAddress = message.ReplyToAddress;
                props.Type = message.Type;
                props.UserId = message.UserId;

                _channel.BasicPublish(message.ExchangeName, message.RoutingKey, props, message.Message);
            });
            
        }


        public void Dispose()
        {
            _channel.Close();
            _channel.Dispose();
            _channelService.Dispose();
        }
    }
}
