using System.Threading.Tasks;
using AdventureWorksDistributed.Core.Commands;
using AdventureWorksDistributed.Core.Infrastructure;
using NServiceBus;

namespace AdventureWorksDistributed.Dispatcher
{
    public class ProcessDocumentMessagesHandler
        : IHandleMessages<ProcessDocumentMessages>
    {
        private readonly IDocumentMessageDispatcher _dispatcher;

        public ProcessDocumentMessagesHandler(IDocumentMessageDispatcher dispatcher) 
            => _dispatcher = dispatcher;

        public Task Handle(ProcessDocumentMessages message, IMessageHandlerContext context) 
            => _dispatcher.Dispatch(message);
    }
}