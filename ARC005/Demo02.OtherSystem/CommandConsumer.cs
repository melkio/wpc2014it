using MassTransit;
using Messages.Demo02;
using System;

namespace Demo02.OtherSystem
{
    public class CommandConsumer : Consumes<OtherSystemCommand>.All
    {
        public void Consume(OtherSystemCommand message)
        {
            Console.WriteLine("Hei, I'm in new system...");
        }
    }
}
