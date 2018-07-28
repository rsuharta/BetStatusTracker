namespace BetStatusTracker.Command.BetRegistration
{
    using System;
    using System.Collections.Generic;

    using Paramore.Brighter;

    public class BetRegistrationCommand : Command
    {
        public BetRegistrationCommand() : base(Guid.NewGuid())
        {
        }

        public string BetClientRef { get; set; }

        public int SequenceNo { get; set; }

        public DateTimeOffset BetSubmittedTime { get; set; }

        public string CustomerId { get; set; }

        public decimal StakeAmount { get; set; }

        public List<Bet> Bets { get; set; }
        
        public DateTimeOffset MessageTimeStamp { get; set; }
    }

    public class Bet
    {
        public string ProductType { get; set; }
        
        public List<Selection> Selections { get; set; }

        public string Status { get; set; }
    }

    public class Selection
    {
        public int Position { get; set; }

        public string CompetitorSeq { get; set; }
    }
}
