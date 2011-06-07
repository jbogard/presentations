using System;

namespace After.Model
{
	public class OfferType
	{
		public string Name { get; private set; }
		public ExpirationType ExpirationType { get; private set; }
		public int DaysValid { get; private set; }
		public DateTime? BeginDate { get; private set; }

		public DateTime CalculateExpirationDate()
		{
			return ExpirationType.CalculateExpirationDate(this);
		}

		public static OfferType CreateAssignmentExpiringOfferType(string name, int daysValid)
		{
			return new OfferType
			{
				Name = name,
				DaysValid = daysValid,
				ExpirationType = ExpirationType.Assignment
			};
		}

		public static OfferType CreateFixedExpiringOfferType(string name, int daysValid, DateTime beginDate)
		{
			return new OfferType
			{
				Name = name,
				DaysValid = daysValid,
				ExpirationType = ExpirationType.Fixed,
				BeginDate = beginDate
			};
		}
	}
}