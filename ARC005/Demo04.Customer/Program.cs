using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Customer
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 04 - CUSTOMER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo04-customer");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x => x.Consumer<PaymentRequestHandler>());
            });

            do
            {
                Console.Write("Customer name:");
                var customer = Console.ReadLine();

                var command = new RegisterOrderCommand { Customer = customer };
                Bus.Instance.Publish(command, x => x.SetResponseAddress(Bus.Instance.Endpoint.Address.Uri));

            } while (true);
            
        }
    }
}
