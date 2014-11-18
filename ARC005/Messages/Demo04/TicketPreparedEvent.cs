using System;

namespace Messages.Demo04
{
    public class TicketPreparedEvent : BaseMessage
    {
        public Double Amount { get; set; }
    }
}
