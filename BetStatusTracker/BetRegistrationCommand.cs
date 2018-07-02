namespace BetStatusTracker
{
    using System;
    using Paramore.Brighter;

    public class BetRegistrationCommand : ICommand
    {
        /// <summary>
        /// Correlation Id.
        /// </summary>
        public Guid Id { get; set; }
        
        public string BetClientRef { get; set; }

        public string SequenceNo { get; set; }

        public DateTimeOffset BetSubmittedTime { get; set; }
        
        public string CustomerId { get; set; }

        public decimal StakeAmount { get; set; }

        public DateTimeOffset MessageTimeStamp { get; set; }
    }

    public class BetRegisteredCommandHandler : RequestHandler<BetRegistrationCommand>
    {
        public override BetRegistrationCommand Handle(BetRegistrationCommand @event)
        {
            Console.WriteLine($"{@event.Id}");
            return base.Handle(@event);
        }
    }
}
