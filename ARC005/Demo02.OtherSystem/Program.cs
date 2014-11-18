using MassTransit;
using Messages.Demo02;
using System;

namespace Demo02.OtherSystem
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 02 - OTHER SYSTEM");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo02-otherSystem");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();

                //configuration.Subscribe(x => x.Consumer<CommandHandledConsumer>());

                configuration.Subscribe(x =>
                    {
                        x.Consumer<CommandHandledGateway>();
                        x.Consumer<CommandConsumer>();
                    });
            });

            Console.ReadLine();
        }
    }
}
