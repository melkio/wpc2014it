using MassTransit;
using System;

namespace Messages
{
    public abstract class BaseMessage : CorrelatedBy<Guid>
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }

        public BaseMessage()
        {
            Id = Guid.NewGuid();
            CorrelationId = Guid.NewGuid();
        }
    }
}
