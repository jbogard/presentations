using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Before.Services;
using Microsoft.EntityFrameworkCore;

namespace Before.Features.Offers.ExpireOffer
{
    public class ExpireOfferService
    {
        private readonly AppDbContext _appDbContext;

        public ExpireOfferService(AppDbContext appDbContext) => _appDbContext = appDbContext;

        public async Task Expire(ExpireOfferRequest request, CancellationToken cancellationToken)
        {
            var member = await _appDbContext.Members
                .Include(m => m.AssignedOffers)
                .SingleOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);

            var offer = member.AssignedOffers.SingleOrDefault(o => o.Id == request.OfferId)
                ?? throw new ArgumentException("Offer not found.", nameof(request.OfferId));

            offer.DateExpiring = DateTime.Today;
            member.NumberOfActiveOffers--;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}