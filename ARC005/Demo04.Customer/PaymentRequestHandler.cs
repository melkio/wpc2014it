using MassTransit;
using Messages.Demo04;
using System;

namespace Demo04.Customer
{
    public class PaymentRequestHandler : Consumes<PaymentRequest>.Context
    {
        public void Consume(IConsumeContext<PaymentRequest> context)
        {
            Console.WriteLine("Activating payment process...");

            context.Bus.Publish(new RegisterPaymentCommand { CorrelationId = context.Message.CorrelationId, Amount = context.Message.Amount });
        }
    }
}
