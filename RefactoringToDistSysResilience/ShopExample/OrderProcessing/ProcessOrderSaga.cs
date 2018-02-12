using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using OrderProcessing.Messages;
using PaymentGateway.Messages;

namespace OrderProcessing
{
    public class ProcessOrderData : ContainSagaData
    {
        public virtual int OrderId { get; set; }
    }
    public class ProcessOrderSaga : Saga<ProcessOrderData>,
        IAmStartedByMessages<ProcessOrderCommand>,
        IHandleMessages<ProcessPaymentResult>
    {
        private static ILog _log = LogManager.GetLogger<ProcessOrderSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ProcessOrderData> mapper)
        {
            mapper.ConfigureMapping<ProcessOrderCommand>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public Task Handle(ProcessOrderCommand message, IMessageHandlerContext context)
        {
            _log.InfoFormat("Processing order {0}", Data.OrderId);

            return context.Send(new ProcessPaymentCommand
            {
                OrderId = Data.OrderId
            });
        }

        public Task Handle(ProcessPaymentResult message, 
            IMessageHandlerContext context)
        {
            if (!message.Success)
            {
                _log.InfoFormat("Payment was not successful; Marking order {0} as needing manual intervention.", Data.OrderId);

                // Mark order as needing intervention
                // order.PaymentFailed = true;

                return Task.FromResult(0);
            }

            _log.InfoFormat("Marking order {0} as successful.", Data.OrderId);
            // order.PaymentSuccessful = true;;

            return context.Publish(new OrderAcceptedEvent {OrderId = Data.OrderId});
        }
    }
}