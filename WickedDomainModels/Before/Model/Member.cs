using System.Collections.Generic;

namespace Before.Model
{
    using System;
    using Services;

    public class Member : Entity
	{
	    private ICollection<Offer> _assignedOffers;

	    public Member()
		{
            _assignedOffers = new List<Offer>();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public IEnumerable<Offer> AssignedOffers => _assignedOffers;

	    public int NumberOfActiveOffers { get; private set; }

	    public Offer AssignOffer(OfferType offerType,
            IOfferValueCalculator offerValueCalulator)
	    {
            var value = offerValueCalulator.CalculateValue(this, offerType);

	        var dateExpiring = offerType.ExpirationType.GetDateExpiring(offerType);

            var offer = new Offer(this, offerType, dateExpiring, value);

            _assignedOffers.Add(offer);
            NumberOfActiveOffers++;

            return offer;
        }
    }
}