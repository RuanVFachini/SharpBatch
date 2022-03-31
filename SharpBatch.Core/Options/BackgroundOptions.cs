using System.Collections.Generic;

namespace SharpBatch.Core.Options
{
    public class BackgroundOptions
    {
        public const string Section = "SharpBatch:BackgroundService";

        public int Workers { get; set; }
        public int ServerWorkerRefresh { get; set; }
    }
}
