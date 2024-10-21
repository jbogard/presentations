using System;
using System.Collections.Generic;
using System.Linq;

namespace Before.Model;

public class Member : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<Offer> AssignedOffers { get; set; } = new List<Offer>();
    public int NumberOfActiveOffers { get; set; }

    public Offer AssignOffer(OfferType offerType, int value)
    {
        var offer = new Offer
        {
            MemberAssigned = this,
            Type = offerType,
            Value = value,
            DateExpiring = offerType.CalculateExpirationDate()
        };
        AssignedOffers.Add(offer);
        NumberOfActiveOffers++;
        return offer;
    }

    public void ExpireOffer(
        Guid offerId)
    {
        var offer = AssignedOffers.SingleOrDefault(o => o.Id == offerId)
                    ?? throw new ArgumentException("Offer not found.", nameof(offerId));

        offer.DateExpiring = DateTime.Today;
        NumberOfActiveOffers--;
    }
}