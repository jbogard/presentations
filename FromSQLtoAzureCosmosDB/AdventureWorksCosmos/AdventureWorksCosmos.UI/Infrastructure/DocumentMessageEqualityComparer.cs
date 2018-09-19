using System.Collections.Generic;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class DocumentMessageEqualityComparer 
        : IEqualityComparer<IDocumentMessage>
    {
        public static readonly DocumentMessageEqualityComparer Instance 
            = new DocumentMessageEqualityComparer();

        public bool Equals(IDocumentMessage x, IDocumentMessage y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(IDocumentMessage obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}