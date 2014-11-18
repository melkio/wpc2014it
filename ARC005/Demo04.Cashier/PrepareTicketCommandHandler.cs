using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Cashier
{
    public class PrepareTicketCommandHandler : Consumes<PrepareTicketCommand>.Context
    {
        public void Consume(IConsumeContext<PrepareTicketCommand> context)
        {
            Console.WriteLine("Handling PrepareTicketCommand");

            context.Bus.Publish(new TicketPreparedEvent { CorrelationId = context.Message.CorrelationId, Amount = 10.5 });
        }
    }
}
