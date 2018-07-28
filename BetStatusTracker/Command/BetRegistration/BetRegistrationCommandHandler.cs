namespace BetStatusTracker.Command.BetRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BetStatusTracker.Entity;
    using BetStatusTracker.Repositories;

    using Paramore.Brighter;
    using Paramore.Brighter.Eventsourcing.Attributes;

    public class BetRegistrationCommandHandler : RequestHandler<BetRegistrationCommand>
    {
        private readonly BetStatusTrackerContext context;

        public BetRegistrationCommandHandler(BetStatusTrackerContext context)
        {
            this.context = context;
        }

        [UseCommandSourcing(step: 1, onceOnly : true, timing: HandlerTiming.Before)]
        public override BetRegistrationCommand Handle(BetRegistrationCommand @event)
        {
            // get the current state of the bet
            var betGroup = this.context.BetGroups.SingleOrDefault(bg => bg.BetClientRef == @event.BetClientRef && bg.SequenceNo == @event.SequenceNo);

            if (betGroup == null)
            {
                this.context.BetGroups.Add(this.CreateNewBetGroupEntity(@event));
            }

            this.context.SaveChanges();

            return base.Handle(@event);
        }

        private BetGroupEntity CreateNewBetGroupEntity(BetRegistrationCommand @event)
        {
            var newBetGroup = new BetGroupEntity
                                  {
                                      BetClientRef = @event.BetClientRef,
                                      SequenceNo = @event.SequenceNo,
                                      BetCombinationNo = 1,
                                      BetSubmittedTime = @event.BetSubmittedTime,
                                      CustomerId = @event.CustomerId,
                                      StakeAmount = @event.StakeAmount,
                                      UpdatedDate = DateTimeOffset.Now,
                                      //Bets = new List<BetEntity>()
                                  };

            /*
            foreach (var bet in @event.Bets)
            {
                var betEntity = new BetEntity
                                    {
                                        SequenceNo = bet.SequenceNo,
                                        ProductType = bet.ProductType,
                                        Selections = new List<SelectionEntity>(),
                                        Status = bet.Status
                                    };

                foreach (var selection in bet.Selections)
                {
                    betEntity.Selections.Add(
                        new SelectionEntity { CompetitorSeq = selection.CompetitorSeq, Position = selection.Position });
                }

                newBetGroup.Bets.Add(betEntity);
            }*/

            return newBetGroup;
        }
    }
}