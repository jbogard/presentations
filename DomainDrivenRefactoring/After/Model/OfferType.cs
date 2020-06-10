using System;

namespace After.Model
{
    public class OfferType
    {
        public string Name { get; set; }
        public ExpirationType ExpirationType { get; set; }
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
                    if (BeginDate != null)
                        dateExpiring =
                            BeginDate.Value.AddDays(DaysValid);
                    else
                        throw new InvalidOperationException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dateExpiring;
        }
    }
}