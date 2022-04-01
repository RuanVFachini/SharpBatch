using RabbitMQ.Client;
using SharpBatch.Core.Options;
using SharpBatch.RebbitMQ.Core.Options;
using System;
using System.Collections.Generic;

namespace SharpBatch.RebbitMQ.Core.Interfaces
{
    public interface IChannelService : IDisposable
    {
        IModel GenerateChannel();
        RabbitMQOptions ServerOptions { get; }
        IEnumerable<QueueOptions> Queues { get; }
    }
}
