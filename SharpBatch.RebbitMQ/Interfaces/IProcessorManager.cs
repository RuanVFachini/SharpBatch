using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.RebbitMQ.Consumer.Interfaces
{
    public interface IProcessorManager
    {
        void Execute(BasicDeliverEventArgs message);
    }
}
