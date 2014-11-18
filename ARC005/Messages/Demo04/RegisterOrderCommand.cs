using System;

namespace Messages.Demo04
{
    public class RegisterOrderCommand : BaseMessage
    {
        public String Customer { get; set; }
    }
}
