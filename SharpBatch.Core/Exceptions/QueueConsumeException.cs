using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.Core.Exceptions
{
    public class QueueConsumeException : Exception
    {
        public QueueConsumeException(Exception exception) : base(exception.Message, exception)
        {
        }

        public QueueConsumeException(string message) : base(message)
        {
        }
    }
}
