using MassTransit;
using Messages.Demo02;
using System;

namespace Demo02.CommandDispatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DEMO 02 - COMMAND DISPATCHER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo02-commandDispatcher");
                configuration.UseRabbitMqRouting();
                configuration.UseJsonSerializer();
            });

            Bus.Instance.Publish(new Command { Value = "Hi wpc2014it!" });

            Console.ReadLine();
        }
    }
}
