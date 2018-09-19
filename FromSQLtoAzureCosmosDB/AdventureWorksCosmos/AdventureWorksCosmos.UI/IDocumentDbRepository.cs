using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AdventureWorksCosmos.UI.Infrastructure;
using Microsoft.Azure.Documents;

namespace AdventureWorksCosmos.UI
{
    public interface IDocumentDBRepository<T> where T : DocumentBase
    {
        Task<Document> CreateItemAsync(T item);
        Task DeleteItemAsync(Guid id);
        Task<T> GetItemAsync(Guid id);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<Document> UpdateItemAsync(T item);
    }
}