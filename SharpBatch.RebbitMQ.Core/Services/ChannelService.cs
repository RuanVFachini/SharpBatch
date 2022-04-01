using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharpBatch.Core.Options;
using SharpBatch.RebbitMQ.Core.Interfaces;
using SharpBatch.RebbitMQ.Core.Options;
using System;
using System.Collections.Generic;

namespace SharpBatch.RebbitMQ.Core.Services
{
    public class ChannelService : IChannelService
    {
        private IConnection _conn;
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

            _conn = factory.CreateConnection();
            return _conn.CreateModel();
        }

        public void Dispose()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}
