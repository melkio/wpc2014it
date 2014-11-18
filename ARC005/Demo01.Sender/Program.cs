using MassTransit;
using Messages.Demo01;
using System;

namespace Demo01.Sender
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 01 - SENDER");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo01-sender");
                configuration.UseRabbitMqRouting();
                //configuration.SetConcurrentConsumerLimit(concurrentConsumerLimit);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x => x.Consumer<ResponseMessageConsumer>());
            });

            Console.WriteLine("Sending request...");
            Bus.Instance.Publish(new RequestMessage { Id = 1, Content = "ABCDEF12345" });

            Console.ReadLine();
        }
    }
}
