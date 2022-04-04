using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.Core.Interfaces
{
    public interface IConsumerService
    {
        Task GetConsumerTask(string queueName);
    }
}
