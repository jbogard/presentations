using System.Linq;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public static class ParseTreeNodeExtensions
    {
        public static ParseTreeNode FirstChild(this ParseTreeNode node)
        {
            return node.ChildNodes.First();
        }

        public static ParseTreeNode LastChild(this ParseTreeNode node)
        {
            return node.ChildNodes.Last();
        }
    }
}