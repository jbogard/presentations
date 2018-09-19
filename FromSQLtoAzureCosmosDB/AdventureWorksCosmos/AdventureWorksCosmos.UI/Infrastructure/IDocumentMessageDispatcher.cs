using System;
using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IDocumentMessageDispatcher
    {
        Task<Exception> Dispatch(DocumentBase document);
    }
}