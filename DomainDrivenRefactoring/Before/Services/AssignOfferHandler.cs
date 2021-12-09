using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Before.Model;
using MediatR;

namespace Before.Services
{
    public interface IOfferValueCalculator
    {
        Task<int> Calculate(
            Member member, 
            OfferType offerType, 
            CancellationToken cancellationToken);
    }

    public class WebServiceOfferValueCalculator : IOfferValueCalculator
    {
        private readonly HttpClient _httpClient;

        public WebServiceOfferValueCalculator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> Calculate(
            Member member, 
            OfferType offerType, 
            CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(
                $"/calculate-offer-value?email={member.Email}&offerType={offerType.Name}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var value = await JsonSerializer.DeserializeAsync<int>(responseStream, cancellationToken: cancellationToken);
            return value;
        }
    }

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

        public async Task<Unit> Handle(AssignOfferRequest request, CancellationToken cancellationToken)
        {
            var member = await _appDbContext.Members.FindAsync(request.MemberId, cancellationToken);
            var offerType = await _appDbContext.OfferTypes.FindAsync(request.OfferTypeId, cancellationToken);

            var value = await _offerValueCalculator.Calculate(member, offerType, cancellationToken);

            var offer = member.AssignOffer(offerType, value);

            await _appDbContext.Offers.AddAsync(offer, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}