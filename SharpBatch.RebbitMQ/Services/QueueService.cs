using SharpBatch.Core.Interfaces;
using SharpBatch.Core.Options;
using SharpBatch.RebbitMQ.Consumer.Interfaces;
using SharpBatch.RebbitMQ.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace SharpBatch.RebbitMQ.Consumer
{
    public class QueueService : IQueueService
    {
        private readonly IConsumerService _connectionService;
        private readonly IChannelService _channelService;

        public IEnumerable<QueueOptions> Queues { get => _channelService.Queues; }

        public QueueService(
            IConsumerService connectionService,
            IChannelService channelService)
        {
            _channelService = channelService;
            _connectionService = connectionService;

            _connectionService.RegisterConsummers();
            
        }

        public void Dispose()
        {
            _channelService.Dispose();
            _connectionService.Dispose();
        }
    }
}
