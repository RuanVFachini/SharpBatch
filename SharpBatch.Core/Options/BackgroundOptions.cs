using System.Collections.Generic;

namespace SharpBatch.Core.Options
{
    public class BackgroundOptions
    {
        public const string Section = "Background";

        public int Workers { get; set; }
        public int ServerWorkerRefresh { get; set; }
        public IList<QueueOptions> Queues { get; set; }
    }
}
