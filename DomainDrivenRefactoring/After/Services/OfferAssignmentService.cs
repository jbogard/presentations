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

            var offer = await member.AssignOffer(offerType, _offerValueCalculator);

            await _appDbContext.Offers.AddAsync(offer);

            await _appDbContext.SaveChangesAsync();
        }
    }

}