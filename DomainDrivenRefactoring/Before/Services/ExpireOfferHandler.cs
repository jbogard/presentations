using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Before.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Before.Services
{
    public class ExpireOfferHandler : IRequestHandler<ExpireOfferRequest>
    {
        private readonly AppDbContext _appDbContext;

        public ExpireOfferHandler(AppDbContext appDbContext) => _appDbContext = appDbContext;

        public async Task<Unit> Handle(ExpireOfferRequest request, CancellationToken cancellationToken)
        {
            var member = await _appDbContext.Members
                .Include(m => m.AssignedOffers)
                .SingleOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);

            member.ExpireOffer(request.OfferId);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}