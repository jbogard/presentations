using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IDocumentMessageHandler<in T>
        where T : IDocumentMessage
    {
        Task Handle(T message);
    }
}