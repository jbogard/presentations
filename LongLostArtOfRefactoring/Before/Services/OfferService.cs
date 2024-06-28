using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Before.Model;
using Microsoft.EntityFrameworkCore;

namespace Before.Services;

public class OfferService
{
    private readonly AppDbContext _appDbContext;
    private readonly HttpClient _httpClient;

    public OfferService(
        AppDbContext appDbContext,
        HttpClient httpClient)
    {
        _appDbContext = appDbContext;
        _httpClient = httpClient;
    }

 
    public async Task AssignOffer(Guid memberId, Guid offerTypeId, CancellationToken cancellationToken)
    {
        var member = await _appDbContext.Members.FindAsync(memberId, cancellationToken);
        var offerType = await _appDbContext.OfferTypes.FindAsync(offerTypeId, cancellationToken);

        // Calculate offer value
        var response = await _httpClient.GetAsync(
            $"/calculate-offer-value?email={member.Email}&offerType={offerType.Name}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var value = await JsonSerializer.DeserializeAsync<int>(responseStream, cancellationToken: cancellationToken);

        // Calculate expiration date
        DateTime dateExpiring;

        switch (offerType.ExpirationType)
        {
            case ExpirationType.Assignment:
                dateExpiring = DateTime.Today.AddDays(offerType.DaysValid);
                break;
            case ExpirationType.Fixed:
                dateExpiring = offerType.BeginDate?.AddDays(offerType.DaysValid)
                               ?? throw new InvalidOperationException();
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
    }
    
    public async Task ExpireOffer(Guid memberId, Guid offerId, CancellationToken cancellationToken)
    {
        var member = await _appDbContext.Members
            .Include(m => m.AssignedOffers)
            .SingleOrDefaultAsync(m => m.Id == memberId, cancellationToken);

        var offer = member.AssignedOffers.SingleOrDefault(o => o.Id == offerId)
                    ?? throw new ArgumentException("Offer not found.", nameof(offerId));

        offer.DateExpiring = DateTime.Today;
        member.NumberOfActiveOffers--;

        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}