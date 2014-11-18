using MassTransit;
using Messages.Demo01;
using System;

namespace Demo01.Receiver
{
    public class RequestMessageConsumer : Consumes<RequestMessage>.Context
    {
        public void Consume(IConsumeContext<RequestMessage> context)
        {
            var request = context.Message;

            var response = new ResponseMessage { CorrelationId = request.CorrelationId, Result = true };
            context.Respond(response);

            Console.WriteLine("Request {0} consumed", request.Id);
        }
    }
}
