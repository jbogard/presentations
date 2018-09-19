using System.Collections.Generic;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class DocumentBaseEqualityComparer : IEqualityComparer<DocumentBase>
    {
        public static readonly DocumentBaseEqualityComparer Instance = new DocumentBaseEqualityComparer();

        public bool Equals(DocumentBase x, DocumentBase y)
        {
            return x.GetType() == y.GetType() && x.Id == y.Id;
        }

        public int GetHashCode(DocumentBase obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}