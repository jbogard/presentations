using System;

namespace Before.Model;

public abstract class OfferType
{
	public string Name { get; set; }
	public abstract ExpirationType ExpirationType { get; }
	public int DaysValid { get; set; }
	public DateTime? BeginDate { get; set; }


	public DateTime CalculateExpirationDate()
	{
		DateTime dateExpiring;

		switch (ExpirationType)
		{
			case ExpirationType.Assignment:
				dateExpiring = DateTime.Today.AddDays(DaysValid);
				break;
			case ExpirationType.Fixed:
				dateExpiring = BeginDate?.AddDays(DaysValid)
				               ?? throw new InvalidOperationException();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return dateExpiring;
	}
}

public class FixedOfferType : OfferType
{
	public override ExpirationType ExpirationType => ExpirationType.Fixed;

	public override DateTime CalculateExpirationDate() =>
		BeginDate?.AddDays(DaysValid) ?? throw new InvalidOperationException();

	public decimal Multiplier { get; set; }
}

public class AssignmentOfferType : OfferType
{
	public override DateTime CalculateExpirationDate() =>
		DateTime.Today.AddDays(DaysValid);

	public override ExpirationType ExpirationType => ExpirationType.Assignment;

	public bool CanExtend { get; set; }
}