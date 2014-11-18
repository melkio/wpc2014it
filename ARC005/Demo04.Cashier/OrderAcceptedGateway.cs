using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Cashier
{
    public class OrderAcceptedGateway : Consumes<OrderAcceptedEvent>.Context
    {
        public void Consume(IConsumeContext<OrderAcceptedEvent> context)
        {
            Console.WriteLine("CASHIER SYSTEM'S ACL: preparing right command and forwarding it");
            context.Bus.Endpoint.Send(new PrepareTicketCommand { CorrelationId = context.Message.CorrelationId });
        }
    }
}
