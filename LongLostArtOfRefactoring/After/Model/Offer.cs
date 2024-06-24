using System;

namespace After.Model
{
    public class Offer : Entity
    {
        public Member MemberAssigned { get; private set; }
        public OfferType Type { get; private set; }
        public DateTime DateExpiring { get; private set; }
        public int Value { get; private set; }

        public Offer(Member memberAssigned, OfferType type, int value)
        {
            MemberAssigned = memberAssigned;
            Type = type;
            DateExpiring = type.CalculateExpirationDate();
            Value = value;
        }

        private Offer()
        {
        }
    }
}