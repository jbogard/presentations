using System;

namespace Before.Model;

public abstract class OfferType
{
	public string Name { get; set; }
	public abstract ExpirationType ExpirationType { get; }
	public int DaysValid { get; set; }
	public DateTime? BeginDate { get; set; }
}

public class FixedOfferType : OfferType
{
	public override ExpirationType ExpirationType => ExpirationType.Fixed;

	public decimal Multiplier { get; set; }
}

public class AssignmentOfferType : OfferType
{
	public override ExpirationType ExpirationType => ExpirationType.Assignment;

	public bool CanExtend { get; set; }
}