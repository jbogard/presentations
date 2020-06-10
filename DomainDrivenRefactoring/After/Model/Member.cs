using System.Collections.Generic;
using System.Threading.Tasks;
using After.Services;

namespace After.Model
{
    public class Member : Entity
    {
        private readonly List<Offer> _assignedOffers = new List<Offer>();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IEnumerable<Offer> AssignedOffers => _assignedOffers;

        public int NumberOfActiveOffers { get; set; }

        public async Task<Offer> AssignOffer(OfferType offerType, IOfferValueCalculator offerValueCalculator)
        {
            var value = await offerValueCalculator.Calculate(this, offerType);

            var offer = new Offer(this, offerType, value);

            _assignedOffers.Add(offer);

            NumberOfActiveOffers++;

            return offer;
        }
    }
}