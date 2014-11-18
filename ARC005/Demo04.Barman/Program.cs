using MassTransit;
using System;

namespace Demo04.Barman
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 04 - BARMAN");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo04-barman");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x =>
                {
                    x.Consumer<OrderAcceptedGateway>();
                    x.Consumer<PrepareOrderCommandHandler>();
                });
            });

            Console.ReadLine();
        }
    }
}
