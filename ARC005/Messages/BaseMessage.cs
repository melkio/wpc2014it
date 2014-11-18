using MassTransit;
using System;

namespace Messages
{
    public abstract class BaseMessage : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public BaseMessage()
        {
            CorrelationId = Guid.NewGuid();
        }
    }
}
