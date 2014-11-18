using MassTransit;
using MassTransit.Saga;
using System;

namespace Demo04.Starbucks
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("DEMO 04 - STARBUCKS SAGA");

            Bus.Initialize(configuration =>
            {
                configuration.ReceiveFrom("rabbitmq://localhost/wpc2014/demo04-starbucks");
                configuration.UseRabbitMqRouting();
                configuration.SetConcurrentConsumerLimit(1);
                configuration.UseJsonSerializer();

                configuration.Subscribe(x => x.Saga<StarbucksSaga>(new InMemorySagaRepository<StarbucksSaga>()));
            });

            Console.ReadLine();
        }
    }
}
