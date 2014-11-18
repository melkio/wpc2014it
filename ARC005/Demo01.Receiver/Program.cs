using MassTransit;
using System;

namespace Demo01.Receiver
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 01 - RECEIVER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo01-receiver");
                configuration.UseRabbitMqRouting();
                configuration.UseJsonSerializer();

                configuration.Subscribe(x => x.Consumer<RequestMessageConsumer>());
            });

            Console.ReadLine();
        }
    }
}
