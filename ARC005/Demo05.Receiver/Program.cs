using MassTransit;
using System;

namespace Demo05.Receiver
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 05 - RECEIVER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo05-receiver");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x => x.Consumer<MessageConsumer>());
            });

            Console.ReadLine();
        }
    }
}
