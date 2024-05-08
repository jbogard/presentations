using System.Threading.Tasks;
using After.Model;

namespace After.Services
{
    public interface IOfferValueCalculator
    {
        Task<int> Calculate(Member member, OfferType offerType);
    }
}