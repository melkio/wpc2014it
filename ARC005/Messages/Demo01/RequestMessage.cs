using MassTransit;
using System;

namespace Messages.Demo01
{
    public class RequestMessage : BaseMessage
    {
        public Int32 Id { get; set; }
        public String Content { get; set; }
    }
}
