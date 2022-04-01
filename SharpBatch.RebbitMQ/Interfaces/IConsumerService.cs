using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;

namespace SharpBatch.RebbitMQ.Consumer.Interfaces
{
    public interface IConsumerService : IDisposable
    {
        void RegisterConsummers();
    }
}
