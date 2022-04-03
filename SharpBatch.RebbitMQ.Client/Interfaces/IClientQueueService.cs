using RabbitMQ.Client;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ.Client.Interfaces
{
    public interface IClientQueueService : IDisposable
    {
        Task EnqueueAsync(IDefaultMessage message, Action<IBasicProperties> optionsFunc = null);
    }
}
