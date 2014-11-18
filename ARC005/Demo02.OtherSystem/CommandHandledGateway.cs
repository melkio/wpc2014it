using MassTransit;
using Messages.Demo02;
using System;

namespace Demo02.OtherSystem
{
    public class CommandHandledGateway : Consumes<CommandHandled>.Context
    {
        public void Consume(IConsumeContext<CommandHandled> context)
        {
            // THIS IS THE ENTRY POINT FOR THE 'OTHER SYSTEM' (ACL IN DDD'S WORLD)
            // COLLECT INFORMATION, VALIDATE AND COMPOSE A NEW COMMAND FOR THE 'OTHER SYSTEM'

            Console.WriteLine("OtherSystem's ACL...forwarding command");

            context.Bus
                .GetEndpoint(new Uri("rabbitmq://localhost/wpc2014/demo02-otherSystem"))
                .Send(new OtherSystemCommand { CorrelationId = context.Message.CorrelationId });
        }
    }
}
