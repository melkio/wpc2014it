using MassTransit;
using System;

namespace Demo02.CommandHandler
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 02 - COMMAND HANDLER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo02-commandHandler");
                configuration.UseRabbitMqRouting();
                //configuration.SetConcurrentConsumerLimit(concurrentConsumerLimit);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x => x.Consumer<CommandConsumer>());
            });

            Console.ReadLine();
        }
    }
}
