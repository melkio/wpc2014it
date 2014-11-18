using MassTransit;
using Messages.Demo02;
using System;

namespace Demo02.OtherSystem
{
    public class CommandHandledConsumer : Consumes<CommandHandled>.All
    {
        public void Consume(CommandHandled message)
        {
            Console.WriteLine("Consuming command handled event...");
        }
    }
}
