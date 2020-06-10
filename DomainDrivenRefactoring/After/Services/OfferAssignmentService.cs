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
        private readonly HttpClient _httpClient;

        public OfferAssignmentService(
            AppDbContext appDbContext,
            HttpClient httpClient)
        {
            _appDbContext = appDbContext;
            _httpClient = httpClient;
        }

        public async Task AssignOffer(Guid memberId, Guid offerTypeId)
        {
            var member = await _appDbContext.Members.FindAsync(memberId);
            var offerType = await _appDbContext.OfferTypes.FindAsync(offerTypeId);

            var value = await CalculateOfferValue(member, offerType);

            var dateExpiring = CalculateExpirationDate(offerType);

            var offer = AssignOffer(member, offerType, value, dateExpiring);

            await SaveOffer(offer);
        }

        private async Task<int> CalculateOfferValue(Member member, OfferType offerType)
        {
            var response =
                await _httpClient.GetAsync($"/calculate-offer-value?email={member.Email}&offerType={offerType.Name}");

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<int>(responseStream);
        }

        private static DateTime CalculateExpirationDate(OfferType offerType)
        {
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

            return dateExpiring;
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