namespace Before.Model
{
    using System;

    public abstract class ExpirationType : Enumeration
	{
		public static readonly ExpirationType Assignment = new AssignmentType();
		public static readonly ExpirationType Fixed = new FixedType();

	    private ExpirationType(int value, string displayName) : base(value, displayName)
	    {
	        
	    }

        public abstract DateTime GetDateExpiring(OfferType offerType);

        private class AssignmentType : ExpirationType
        {
            public AssignmentType() : base(1, "Assignment")
            {
                
            }

            public override DateTime GetDateExpiring(OfferType offerType)
            {
                return DateTime.Now.AddDays(offerType.DaysValid);
            }
        }

        private class FixedType : ExpirationType
        {
            public FixedType() : base(2, "Fixed")
            {
                
            }

            public override DateTime GetDateExpiring(OfferType offerType)
            {
                if (offerType.BeginDate != null)
                    return offerType.BeginDate.Value.AddDays(offerType.DaysValid);

                throw new InvalidOperationException();
            }
        }
    }
}