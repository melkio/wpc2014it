using Magnum.StateMachine;
using MassTransit;
using MassTransit.Saga;
using Messages.Demo04;
using System;

namespace Demo04.Starbucks
{
    public class StarbucksSaga : SagaStateMachine<StarbucksSaga>, ISaga
    {
        static StarbucksSaga()
        {
            Define(() =>
            {
                Initially(
                    When(NewOrder)
                        .Then((saga, message) => Console.WriteLine("Received order by {0}", message.Customer))
                        .Publish((saga, message) => new OrderAcceptedEvent { CorrelationId = message.CorrelationId })
                        .TransitionTo(Waiting)
                    );

                During(Waiting, When(TicketPrepared)
                    .Then((saga, message) => saga.Amount = message.Amount));

                Combine(TicketPrepared, OrderPrepared)
                    .Into(ReadyToProcess, saga => saga.ReadyFlags);

                During(Waiting,
                    When(ReadyToProcess)
                        .Then((saga, message) => Console.WriteLine("Prepared ticket and order"))
                        .Publish(saga => new PaymentRequest {  CorrelationId = saga.CorrelationId, Amount = saga.Amount })
                        .TransitionTo(WaitingPayment));

                During(WaitingPayment, When(PaymentRegistered)
                    .Then((saga, message) => Console.WriteLine("Process completed"))
                    .Complete());
            });
        }

        public StarbucksSaga(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public IServiceBus Bus { get; set; }
        public Int32 ReadyFlags { get; set; }
        public Double Amount { get; set; }

        public static State Initial { get; set; }
        public static State Waiting { get; set; }
        public static State WaitingPayment { get; set; }
        public static State Completed { get; set; }

        public static Event<RegisterOrderCommand> NewOrder { get; set; }
        public static Event<TicketPreparedEvent> TicketPrepared { get; set; }
        public static Event<OrderPreparedEvent> OrderPrepared { get; set; }
        public static Event ReadyToProcess { get; set; }        
        public static Event<PaymentRegisteredEvent> PaymentRegistered { get; set; }
    }
}
