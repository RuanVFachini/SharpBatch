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
        IBasicProperties BasicProperties { get; }
        string ContentEncoding { get; }
        string ContentType { get; }
        byte DeliveryMode { get; }
        IDictionary<string, object> Headers { get; }
        string MessageId { get; }
        bool Persistent { get; }
        byte Priority { get; }
        string ReplyTo { get; }
        PublicationAddress ReplyToAddress { get; }
        string Type { get; }
        string UserId { get; }
        string Expiration { get; }
    }
}
