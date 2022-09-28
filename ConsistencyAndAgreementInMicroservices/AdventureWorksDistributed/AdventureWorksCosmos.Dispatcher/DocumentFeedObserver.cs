using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksDistributed.Core;
using AdventureWorksDistributed.Core.Commands;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;

namespace AdventureWorksDistributed.Dispatcher
{
    public class DocumentFeedObserver<T> : IChangeFeedObserver
        where T : DocumentBase
    {
        static ILog log = LogManager.GetLogger<DocumentFeedObserver<T>>();

        public Task OpenAsync(IChangeFeedObserverContext context) 
            => Task.CompletedTask;

        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason) 
            => Task.CompletedTask;

        public async Task ProcessChangesAsync(
            IChangeFeedObserverContext context, 
            IReadOnlyList<Document> docs, 
            CancellationToken cancellationToken)
        {
            foreach (var doc in docs)
            {
                log.Info($"Processing changes for document {doc.Id}");

                var item = (dynamic)doc;

                if (item.Outbox.Count > 0)
                {
                    ProcessDocumentMessages message = ProcessDocumentMessages.New<T>(item);

                    await Program.Endpoint.SendLocal(message);
                }
            }
        }
    }
}