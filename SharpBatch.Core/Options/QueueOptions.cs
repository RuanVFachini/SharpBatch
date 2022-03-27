using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.Core.Options
{
    public class QueueOptions
    {
        public const string Section = "Background:Queues";
        public string Name { get; set; }
        public int Parallel { get; set; }
    }
}
