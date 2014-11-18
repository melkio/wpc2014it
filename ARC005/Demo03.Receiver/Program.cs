using MassTransit;
using System;

namespace Demo03.Receiver
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 03 - RECEIVER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo03-receiver");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();
                configuration.SetDefaultRetryLimit(5);

                configuration.Subscribe(x => x.Consumer<PoisonMessageConsumer>());
            });

            Console.ReadLine();
        }
    }
}
