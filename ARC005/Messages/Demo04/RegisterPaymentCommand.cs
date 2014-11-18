using System;

namespace Messages.Demo04
{
    public class RegisterPaymentCommand : BaseMessage
    {
        public Double Amount { get; set; }
    }
}
