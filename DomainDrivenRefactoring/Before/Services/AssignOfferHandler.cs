using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Before.Model;
using MediatR;

namespace Before.Services
{
    public class AssignOfferHandler : IRequestHandler<AssignOfferRequest>
    {
        private readonly AppDbContext _appDbContext;
        private readonly HttpClient _httpClient;

        public AssignOfferHandler(
            AppDbContext appDbContext,
            HttpClient httpClient)
        {
            _appDbContext = appDbContext;
            _httpClient = httpClient;
        }

        public async Task<Unit> Handle(AssignOfferRequest request, CancellationToken cancellationToken)
        {
            var member = await _appDbContext.Members.FindAsync(request.MemberId);
            var offerType = await _appDbContext.OfferTypes.FindAsync(request.OfferTypeId);

            // Calculate offer value
            var response = await _httpClient.GetAsync(
                $"/calculate-offer-value?email={member.Email}&offerType={offerType.Name}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<int>(responseStream, cancellationToken: cancellationToken);

            // Calculate expiration date
            DateTime dateExpiring;

            switch (offerType.ExpirationType)
            {
                case ExpirationType.Assignment:
                    dateExpiring = DateTime.Today.AddDays(offerType.DaysValid);
                    break;
                case ExpirationType.Fixed:
                    if (offerType.BeginDate != null)
                        dateExpiring =
                            offerType.BeginDate.Value.AddDays(offerType.DaysValid);
                    else
                        throw new InvalidOperationException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Assign offer
            var offer = new Offer
            {
                MemberAssigned = member,
                Type = offerType,
                Value = value,
                DateExpiring = dateExpiring
            };
            member.AssignedOffers.Add(offer);
            member.NumberOfActiveOffers++;

            await _appDbContext.Offers.AddAsync(offer, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}