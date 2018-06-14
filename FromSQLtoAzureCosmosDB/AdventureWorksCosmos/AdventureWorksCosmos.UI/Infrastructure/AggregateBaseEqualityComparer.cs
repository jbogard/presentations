using System.Collections.Generic;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class AggregateBaseEqualityComparer : IEqualityComparer<AggregateBase>
    {
        public static readonly AggregateBaseEqualityComparer Instance = new AggregateBaseEqualityComparer();

        public bool Equals(AggregateBase x, AggregateBase y)
        {
            return x.GetType() == y.GetType() && x.Id == y.Id;
        }

        public int GetHashCode(AggregateBase obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}