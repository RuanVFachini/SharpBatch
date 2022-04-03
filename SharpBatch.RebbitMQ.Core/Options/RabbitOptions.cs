using SharpBatch.Core.Options;
using System.Collections.Generic;

namespace SharpBatch.RebbitMQ.Core.Options
{
    public class RabbitMQOptions
    {
        public const string Section = "SharpBatch:RabbitMQ";

        public string UserName {get;set;}
        public string Password {get;set;}
        public string VirtualHost {get;set;}
        public string HostName { get; set; }

    }
}
