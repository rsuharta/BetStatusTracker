namespace BetStatusTracker.Entity
{
    using System.Collections.Generic;

    using BetStatusTracker.Repositories.Enum;

    public class BetEntity
    {
        public int Id { get; set; }

        public ProductType ProductType { get; set; }

        public BetType BetType { get; set; }

        public virtual ICollection<SelectionEntity> Selections { get; set; }

        public virtual ICollection<SelectionEntity> SelectionOutcomes { get; set; }

        public string Status { get; set; }
    }
}
