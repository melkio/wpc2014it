using MassTransit;
using Messages.Demo02;
using System;

namespace Demo02.CommandHandler
{
    public class CommandConsumer : Consumes<Command>.Context
    {
        public void Consume(IConsumeContext<Command> context)
        {
            Console.WriteLine("Handling command...");

            var @event = new CommandHandled { CorrelationId = context.Message.CorrelationId };
            context.Bus.Publish(@event);
        }
    }
}
