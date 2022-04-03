using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.RebbitMQ.Core.Interfaces
{
    public interface IDefaultMessage
    {
        string ExchangeName { get; }
        string RoutingKey { get; }
        byte[] Message { get; }
    }
}
