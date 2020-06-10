using System;

namespace After.Model
{
    public class Offer : Entity
    {
        public Member MemberAssigned { get; set; }
        public OfferType Type { get; set; }
        public DateTime DateExpiring { get; set; }
        public int Value { get; set; }
    }
}