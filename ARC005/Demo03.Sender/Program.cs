using MassTransit;
using Messages.Demo03;
using System;

namespace Demo03.Sender
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 03 - SENDER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo03-sender");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();
            });

            Console.WriteLine("Sending request...");
            Bus.Instance
                .GetEndpoint(new Uri("rabbitmq://localhost/wpc2014/demo03-receiver"))
                .Send(new PoisonMessage { Content = "Mo' son 'azzi" });

            Console.ReadLine();
        }
    }
}
