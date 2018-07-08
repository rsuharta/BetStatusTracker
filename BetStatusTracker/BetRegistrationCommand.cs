namespace BetStatusTracker
{
    using System;
    using Paramore.Brighter;

    public class BetRegistrationCommand : Command
    {
        public BetRegistrationCommand() : base(Guid.NewGuid())
        {
        }

        public string BetClientRef { get; set; }

        public string SequenceNo { get; set; }

        public DateTimeOffset BetSubmittedTime { get; set; }

        public string CustomerId { get; set; }

        public decimal StakeAmount { get; set; }

        public DateTimeOffset MessageTimeStamp { get; set; }
    }
}
