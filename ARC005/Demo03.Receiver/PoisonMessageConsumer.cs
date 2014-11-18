using MassTransit;
using Messages.Demo03;
using System;

namespace Demo03.Receiver
{
    public class PoisonMessageConsumer : Consumes<PoisonMessage>.Context
    {
        public void Consume(IConsumeContext<PoisonMessage> context)
        {
            Console.WriteLine("Processing poison message...");

            throw new NotImplementedException();
        }
    }
}
