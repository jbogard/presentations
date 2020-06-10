using System;

namespace Before.Model
{
    public class Offer : Entity
    {
        public Member MemberAssigned { get; set; }
        public OfferType Type { get; set; }
        public DateTime DateExpiring { get; set; }
        public int Value { get; set; }
    }
}