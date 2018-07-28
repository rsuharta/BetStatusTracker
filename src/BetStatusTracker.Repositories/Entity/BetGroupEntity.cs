namespace BetStatusTracker.Entity
{
    using System;
    using System.Collections.Generic;

    public class BetGroupEntity
    {
        public int Id { get; set; }

        public string BetClientRef { get; set; }

        public int SequenceNo { get; set; }

        // default to '1', only multi bets are constructed from parlay could have value > 1
        public int BetCombinationNo { get; set; }

        public DateTimeOffset BetSubmittedTime { get; set; }

        public string CustomerId { get; set; }

        public decimal StakeAmount { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        //public virtual ICollection<BetEntity> Bets { get; set; }
    }
}
