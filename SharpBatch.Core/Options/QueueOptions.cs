using System.Collections.Generic;

namespace SharpBatch.Core.Options
{
    public class QueueOptions
    {
        public const string Section = "SharpBatch";

        public IEnumerable<Queue> Queues {get;set;}

    }

    public class Queue
    {
        public string Name { get; set; }
        public int MaxParallel { get; set; }
    }
}
