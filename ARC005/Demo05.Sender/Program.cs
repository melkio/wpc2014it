using MassTransit;
using Messages.Demo05;
using System;
using System.Linq;

namespace Demo05.Sender
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 05 - SENDER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo05-sender");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();
            });

            Console.Write("Publishing...");
            Enumerable
                .Range(0, 1000000)
                .ToList()
                .ForEach(x => Bus.Instance.Publish(new Message { Value = x }));
            Console.WriteLine("Completed!");


            Console.ReadLine();
        }
    }
}
