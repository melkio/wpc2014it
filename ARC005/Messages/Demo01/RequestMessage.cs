using MassTransit;
using System;

namespace Messages.Demo01
{
    public class RequestMessage : BaseMessage
    {
        public String Content { get; set; }
    }
}
