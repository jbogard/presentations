using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace After.Services
{
    public class AssignOfferHandler : IRequestHandler<AssignOfferRequest>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IOfferValueCalculator _offerValueCalculator;

        public AssignOfferHandler(
            AppDbContext appDbContext, IOfferValueCalculator offerValueCalculator)
        {
            _appDbContext = appDbContext;
            _offerValueCalculator = offerValueCalculator;
        }

        public async Task Handle(AssignOfferRequest request, CancellationToken cancellationToken)
        {
            var member = await _appDbContext.Members.FindAsync(request.MemberId, cancellationToken);

            var offerType = await _appDbContext.OfferTypes.FindAsync(request.OfferTypeId, cancellationToken);

            var offer = await member.AssignOffer(offerType, _offerValueCalculator);

            await _appDbContext.Offers.AddAsync(offer, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }

}