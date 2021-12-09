using System;

namespace Before.Model
{
	public class OfferType
	{
		public string Name { get; set; }
		public ExpirationType ExpirationType { get; set; }
		public int DaysValid { get; set; }
		public DateTime? BeginDate { get; set; }

        public DateTime CalculateExpirationDate() =>
            ExpirationType switch
            {
                ExpirationType.Assignment => DateTime.Today.AddDays(DaysValid),
                ExpirationType.Fixed => BeginDate?.AddDays(DaysValid) ?? throw new InvalidOperationException(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}