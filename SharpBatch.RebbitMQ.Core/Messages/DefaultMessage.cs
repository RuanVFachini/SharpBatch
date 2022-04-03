using RabbitMQ.Client;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.RebbitMQ.Core.Messages
{
    public class DefaultMessage : IDefaultMessage
    {
        public DefaultMessage(
            string exchangeName,
            string routingKey,
            byte[] message)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Message = message;
        }

        public string ExchangeName { get; protected set; }

        public string RoutingKey { get; protected set; }

        public byte[] Message { get; set; }
    }
}
