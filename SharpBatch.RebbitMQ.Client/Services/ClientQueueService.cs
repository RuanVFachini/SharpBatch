using RabbitMQ.Client;
using SharpBatch.Core.Options;
using SharpBatch.RebbitMQ.Client.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ.Client
{
    public class ClientQueueService : IClientQueueService
    {
        private readonly IChannelService _channelService;
        
        public ClientQueueService(
            IChannelService channelService)
        {
            _channelService = channelService;

            
        }

        public Task EnqueueAsync(IDefaultMessage message, Action<IBasicProperties> optionsFunc = null)
        {

            return Task.Run(() =>
            {
                var channel = _channelService.GenerateChannel();

                IBasicProperties props = channel.CreateBasicProperties();

                optionsFunc?.Invoke(props);

                channel.BasicPublish(message.ExchangeName, message.RoutingKey, props, message.Message);

                channel.Close();
            });
            
        }


        public void Dispose()
        {
            _channelService.Dispose();
        }
    }
}
