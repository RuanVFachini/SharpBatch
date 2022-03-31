using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharpBatch.Core.Options;
using SharpBatch.RebbitMQ.Core.Interfaces;
using SharpBatch.RebbitMQ.Core.Options;
using System.Collections.Generic;

namespace SharpBatch.RebbitMQ.Core.Services
{
    public class ChannelService : IChannelService
    {
        public RabbitMQOptions ServerOptions { get; }
        public IEnumerable<QueueOptions> Queues { get => ServerOptions.Queues; }

        public ChannelService(IOptions<RabbitMQOptions> options)
        {
            ServerOptions = options.Value;
        }

        public virtual IModel GenerateChannel()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = ServerOptions.UserName,
                Password = ServerOptions.Password,
                VirtualHost = ServerOptions.VirtualHost,
                HostName = ServerOptions.HostName,
                DispatchConsumersAsync = true
            };

            IConnection conn = factory.CreateConnection();
            return conn.CreateModel();
        }
    }
}
