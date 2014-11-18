using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Cashier
{
    public class RegisterPaymentCommandHandler : Consumes<RegisterPaymentCommand>.Context
    {
        public void Consume(IConsumeContext<RegisterPaymentCommand> context)
        {
            Console.WriteLine("Handling RegisterPaymentCommand");

            context.Bus.Publish(new PaymentRegisteredEvent { CorrelationId = context.Message.CorrelationId });
        }
    }
}
