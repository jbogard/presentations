using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace AdventureWorksDistributed.Core.Infrastructure
{
    public interface IDocumentDbRepository<T> 
        where T : DocumentBase
    {
        Task<Document> CreateItemAsync(T item);
        Task DeleteItemAsync(Guid id);
        Task<T> GetItemAsync(Guid id);
        Task<T> GetItemAsync(Guid id, string partitionKey);
        Task<IEnumerable<T>> GetItemsAsync(
            Expression<Func<T, bool>> predicate);
        Task<Document> UpdateItemAsync(T item);
    }
}