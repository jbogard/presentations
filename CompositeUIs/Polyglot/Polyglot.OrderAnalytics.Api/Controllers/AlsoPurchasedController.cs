using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Polyglot.OrderAnalytics.Model;
using Neo4jClient.Gremlin;

namespace Polyglot.OrderAnalytics.Api.Controllers
{
    public class AlsoPurchasedController : ApiController
    {
        public int[] Get(int id)
        {
            var client = WebApiApplication.GraphClient;

            var node = client
                .QueryIndex<Product>("node_auto_index", IndexFor.Node, "ProductId:" + id)
                .Single();

            var queryText =
                string.Format(
                    "g.v({0}).bothE('{1}').sort{{a,b -> b.Count <=> a.Count}}_()[0..2].bothV().filter{{it.id != {0}}}"
                    , node.Reference.Id
                    , AlsoBoughtWith.TypeKey);

            var result = client.ExecuteGetAllNodesGremlin<Product>(queryText, null);

            return result.Select(p => p.Data.ProductId).ToArray();
        }
    }
}
