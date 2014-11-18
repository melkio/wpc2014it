using MassTransit;
using Messages.Demo01;
using System;

namespace Demo01.Sender
{
    public class ResponseMessageConsumer : Consumes<ResponseMessage>.All
    {
        public void Consume(ResponseMessage message)
        {
            Console.WriteLine("Request {0} completed", message.CorrelationId);
        }
    }
}
