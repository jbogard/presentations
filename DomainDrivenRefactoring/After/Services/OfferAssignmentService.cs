using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using After.Model;

namespace After.Services
{
    public class OfferAssignmentService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IOfferValueCalculator _offerValueCalculator;

        public OfferAssignmentService(
            AppDbContext appDbContext, IOfferValueCalculator offerValueCalculator)
        {
            _appDbContext = appDbContext;
            _offerValueCalculator = offerValueCalculator;
        }

        public async Task AssignOffer(Guid memberId, Guid offerTypeId)
        {
            var member = await _appDbContext.Members.FindAsync(memberId);
            var offerType = await _appDbContext.OfferTypes.FindAsync(offerTypeId);

            var value = await _offerValueCalculator.Calculate(member, offerType);

            var dateExpiring = offerType.CalculateExpirationDate();

            var offer = AssignOffer(member, offerType, value, dateExpiring);

            await SaveOffer(offer);
        }

        private static Offer AssignOffer(Member member, OfferType offerType, int value, DateTime dateExpiring)
        {
            var offer = new Offer
            {
                MemberAssigned = member,
                Type = offerType,
                Value = value,
                DateExpiring = dateExpiring
            };
            member.AssignedOffers.Add(offer);
            member.NumberOfActiveOffers++;
            return offer;
        }

        private async Task SaveOffer(Offer offer)
        {
            await _appDbContext.Offers.AddAsync(offer);

            await _appDbContext.SaveChangesAsync();
        }
    }

}