using System.Linq;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class RoundFunctionNode : FunctionNode
    {
        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var truncateFunctionNode = GetTruncateFunctionNode(treeNode);

            if(truncateFunctionNode != null)
            {
                var truncateNode = truncateFunctionNode.FirstChild;
                AddChild("truncateFunction", truncateNode);
                _args.Add((AstNode)truncateNode.AstNode);    
            }

        }

        private ParseTreeNode GetTruncateFunctionNode(ParseTreeNode node)
        {
            var hasTruncateFunctionNode = node.ChildNodes.Any() && node.LastChild.ChildNodes.Any() && node.LastChild.FirstChild.Term.Name == "?" && node.LastChild.FirstChild.ChildNodes.Any();

            if(hasTruncateFunctionNode)
            {
                return node.LastChild.FirstChild;
            }
            return null;
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}