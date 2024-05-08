using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using After.Model;

namespace After.Services
{
    public class OfferValueCalculator : IOfferValueCalculator
    {
        private readonly HttpClient _httpClient;

        public OfferValueCalculator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> Calculate(Member member, OfferType offerType)
        {
            var response =
                await _httpClient.GetAsync($"/calculate-offer-value?email={member.Email}&offerType={offerType.Name}");

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<int>(responseStream);
        }
    }
}