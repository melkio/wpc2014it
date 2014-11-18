using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Barman
{
    public class PrepareOrderCommandHandler : Consumes<PrepareOrderCommand>.Context
    {
        public void Consume(IConsumeContext<PrepareOrderCommand> context)
        {
            Console.WriteLine("Handling PrepareOrderCommand");

            context.Bus.Publish(new OrderPreparedEvent { CorrelationId = context.Message.CorrelationId });
        }
    }
}
