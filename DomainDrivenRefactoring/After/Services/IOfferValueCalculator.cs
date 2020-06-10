using System.Threading.Tasks;
using After.Model;

namespace After.Services
{
    public interface IOfferValueCalculator
    {
        Task<int> CalculateValue(Member member, OfferType offerType);
    }
}