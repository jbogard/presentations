using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;

namespace Polyglot.OrderAnalytics.Model
{
    public class AlsoBoughtWith : Relationship<AlsoBoughtWith.TimesBought>,
        IRelationshipAllowingSourceNode<Product>,
        IRelationshipAllowingTargetNode<Product>
    {
        public AlsoBoughtWith(NodeReference targetNode, TimesBought data)
            : base(targetNode, data)
        {
        }

        public AlsoBoughtWith() : base(null, null) {}

        public const string TypeKey = "PRODUCT_ALSO_BOUGHT_WITH";

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }

        public class TimesBought
        {
            public int Count { get; set; }
        }
    }
}
