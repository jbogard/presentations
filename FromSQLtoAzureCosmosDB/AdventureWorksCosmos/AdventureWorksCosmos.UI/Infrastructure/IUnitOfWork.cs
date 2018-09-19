using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IUnitOfWork
    {
        T Find<T>(Guid id) where T : DocumentBase;
        void Register(DocumentBase document);
        void Register(IEnumerable<DocumentBase> aggregates);
        Task Complete();
    }
}