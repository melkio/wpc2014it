using MassTransit;
using System;

namespace Demo04.Cashier
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 04 - CASHIER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo04-cashier");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x =>
                    {
                        x.Consumer<OrderAcceptedGateway>();
                        x.Consumer<PrepareTicketCommandHandler>();
                        x.Consumer<RegisterPaymentCommandHandler>();
                    });
            });

            Console.ReadLine();
        }
    }
}
