using System;
using System.Collections.Generic;
using System.Linq;

namespace Before.Model
{
    public class Member : Entity
    {
        private int _numberOfActiveOffers;

        public Member(string firstName, string lastName, string email)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public List<Offer> AssignedOffers { get; set; } = new List<Offer>();

        public int NumberOfActiveOffers => _numberOfActiveOffers;

        public Offer AssignOffer(OfferType offerType, int value)
        {
            var offer = new Offer(this, offerType, value);

            AssignedOffers.Add(offer);
            _numberOfActiveOffers++;
            return offer;
        }

        public void ExpireOffer(Guid offerId)
        {
            var offer = AssignedOffers.SingleOrDefault(o => o.Id == offerId)
                        ?? throw new ArgumentException("Offer not found.", nameof(offerId));

            offer.DateExpiring = DateTime.Today;
            _numberOfActiveOffers--;
        }
    }
}