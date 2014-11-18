using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Barman
{
    public class OrderAcceptedGateway : Consumes<OrderAcceptedEvent>.Context
    {
        public void Consume(IConsumeContext<OrderAcceptedEvent> context)
        {
            Console.WriteLine("BARMAN SYSTEM'S ACL: preparing right command and forwarding it");
            context.Bus.Endpoint.Send(new PrepareOrderCommand { CorrelationId = context.Message.CorrelationId });
        }
    }
}
