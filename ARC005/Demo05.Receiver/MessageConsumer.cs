using MassTransit;
using Messages.Demo05;
using System;
using System.Threading;

namespace Demo05.Receiver
{
    public class MessageConsumer : Consumes<Message>.All
    {
        public void Consume(Message message)
        {
            Console.WriteLine("Handling message {0}...", message.Value);
            Thread.Sleep(500);
            Console.WriteLine("Message {0} handled!", message.Value);
        }
    }
}
