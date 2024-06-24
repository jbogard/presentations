using System;
using Ardalis.SmartEnum;

namespace After.Model
{
    public abstract class ExpirationType : SmartEnum<ExpirationType>
    {
        public static readonly ExpirationType Assignment = new AssignmentType();
        public static readonly ExpirationType Fixed = new FixedType();

        protected ExpirationType(string name, int value) : base(name, value)
        {
        }

        private class AssignmentType : ExpirationType
        {
            public AssignmentType() : base(nameof(Assignment), 1)
            {
            }

            public override DateTime CalculateExpirationDate(OfferType offerType) 
                => DateTime.Today.AddDays(offerType.DaysValid);
        }

        private class FixedType : ExpirationType
        {
            public FixedType() : base(nameof(Fixed), 2)
            {
            }

            public override DateTime CalculateExpirationDate(OfferType offerType) =>
                offerType.BeginDate?.AddDays(offerType.DaysValid) ?? throw new InvalidOperationException();
        }

        public abstract DateTime CalculateExpirationDate(OfferType offerType);
    }
}