using System;

namespace After.Model
{
	public abstract class ExpirationType : Enumeration
	{
		public static readonly ExpirationType Assignment = new AssignmentExpiration(1, "Assignment");
		public static readonly ExpirationType Fixed = new FixedExpiration(2, "Fixed");

		protected ExpirationType(int value, string displayName)
			: base(value, displayName)
		{
		}

		public abstract DateTime CalculateExpirationDate(OfferType offerType);

		private class AssignmentExpiration : ExpirationType
		{
			public AssignmentExpiration(int value, string displayName)
				: base(value, displayName)
			{
			}

			public override DateTime CalculateExpirationDate(OfferType offerType)
			{
				return DateTime.Now.AddDays(offerType.DaysValid);
			}
		}

		private class FixedExpiration : ExpirationType
		{
			public FixedExpiration(int value, string displayName)
				: base(value, displayName)
			{
			}

			public override DateTime CalculateExpirationDate(OfferType offerType)
			{
				if (offerType.BeginDate != null)
					return offerType.BeginDate.Value.AddDays(offerType.DaysValid);
				else
					throw new InvalidOperationException();
			}
		}
	}
}