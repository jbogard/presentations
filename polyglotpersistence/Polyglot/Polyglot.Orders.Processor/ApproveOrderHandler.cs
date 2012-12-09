using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Polyglot.Orders.Processor.Messages;
using Polyglot.UI.Orders.Models;
using LineItem = Polyglot.Orders.Processor.Messages.LineItem;

namespace Polyglot.Orders.Processor
{
    public class ApproveOrderHandler : IHandleMessages<ApproveOrderMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(ApproveOrderMessage message)
        {
            using (var session = EndpointConfig.DocumentStore.OpenSession())
            {
                var order = session.Load<OrderRequest>(message.OrderId);

                if (order.Status == Status.Submitted)
                {
                    order.Status = Status.Approved;
                    Bus.Publish<IOrderApprovedMessage>(msg =>
                    {
                        msg.OrderId = order.Id;
                        msg.LineItems = order.Items.Select(li => new LineItem
                        {
                            ProductId = li.ProductID
                        }).ToList();
                    });
                }

                session.SaveChanges();
            }
        }
    }
}
