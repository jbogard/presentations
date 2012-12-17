using System.Text;
using NServiceBus;
using Neo4jClient;
using Polyglot.OrderAnalytics.Model;
using Polyglot.Orders.Processor.Messages;
using log4net;
using System.Linq;

namespace Polyglot.OrderAnalytics
{
    public class OrderApprovedHandler : IHandleMessages<IOrderApprovedMessage>
    {
        public void Handle(IOrderApprovedMessage message)
        {
            LogManager.GetLogger("OrderApprovedHandler").Info("Updating orders approved for " + message.OrderId);

            var client = EndpointConfig.GraphClient;

            var nodeQueryText = new StringBuilder("ProductId:" + message.LineItems[0].ProductId);

            foreach (var lineItem in message.LineItems.Skip(1))
            {
                nodeQueryText.AppendFormat(" OR ProductId:{0}", lineItem.ProductId);
            }
            var nodeQuery = nodeQueryText.ToString();

            var nodes = client
                .QueryIndex<Product>("node_auto_index", IndexFor.Node, nodeQuery)
                .ToList();

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                var targetNode = nodes[i];

                var otherNodes = nodes.Skip(i+1).Select(n => n.Reference.Id.ToString()).ToArray();
                var otherNodesJoined = string.Join(",", otherNodes);

                var addMissingEdgesQuery = string.Format(
                    "g.v({0})_().filter{{it.both('{1}').has('id', {2}L).count() == 0}}.each{{g.addEdge(g.v({2}), it, '{1}', ['Count':0])}}",
                    otherNodesJoined,
                    AlsoBoughtWith.TypeKey,
                    targetNode.Reference.Id
                    );
                client.ExecuteScalarGremlin(addMissingEdgesQuery, null);

                var incrementCountQuery = string.Format(
                    "g.v({0})_().bothE('{1}').filter{{it.inVertex.id == {2} || it.outVertex.id == {2}}}.sideEffect{{it.Count++}}",
                    otherNodesJoined,
                    AlsoBoughtWith.TypeKey,
                    targetNode.Reference.Id
                    );
                client.ExecuteScalarGremlin(incrementCountQuery, null);
            }
        }
    }
}
