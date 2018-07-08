using System;
using Paramore.Brighter;
using Paramore.Brighter.Eventsourcing.Attributes;

namespace BetStatusTracker
{
    public class BetRegistrationCommandHandler : RequestHandler<BetRegistrationCommand>
    {
        [UseCommandSourcing(step: 1, onceOnly : true, timing: HandlerTiming.Before)]
        public override BetRegistrationCommand Handle(BetRegistrationCommand @event)
        {
            Console.WriteLine($"{@event.Id}");
            return base.Handle(@event);
        }
    }
}