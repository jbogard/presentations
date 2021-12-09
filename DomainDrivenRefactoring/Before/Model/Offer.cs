using System;

namespace Before.Model
{
    public class Offer : Entity
    {
        public Offer(Member memberAssigned, OfferType type, int value)
        {
            MemberAssigned = memberAssigned;
            Type = type;
            DateExpiring = type.CalculateExpirationDate();
            Value = value;
        }

        public Member MemberAssigned { get; set; }
        public OfferType Type { get; set; }
        public DateTime DateExpiring { get; set; }
        public int Value { get; set; }
    }
}