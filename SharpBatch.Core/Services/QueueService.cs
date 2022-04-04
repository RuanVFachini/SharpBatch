
using Microsoft.Extensions.Options;
using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.Core.Workers
{
    public class QueueService : IQueueService
    {
        private readonly IConsumerService _consumerService;
        private readonly Dictionary<string, List<Task>> _queue = new Dictionary<string, List<Task>>();
        private readonly IList<Queue> _queues;


        public QueueService(
            IOptions<QueueOptions> queueOptions,
            IConsumerService consumerService)
        {
            _consumerService = consumerService;
            _queues = queueOptions.Value.Queues.ToList();
        }

        
        public Task DequeueTask()
        {
            //Desenvolver rotina para manutenção dos processos, excluir caso tenha falhado para reatribuir consumer

            foreach (var queue in _queues)
            {
                if (_queue.ContainsKey(queue.Name) && _queue[queue.Name].Count < queue.MaxParallel)
                {
                    var task = _consumerService.GetConsumerTask(queue.Name);

                    if (_queue.ContainsKey(queue.Name))
                    {
                        _queue[queue.Name].Add(task);
                    }
                    else
                    {
                        _queue.Add(queue.Name, new List<Task>() { task });
                    }
                }
            }

            return null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
