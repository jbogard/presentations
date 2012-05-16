using System.Collections.Generic;
using System.Linq;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class SystemFunctionNode : AstNode
    {
        public string Name { get; private set; }
        public IEnumerable<AstNode> Arguments { get { return GetChildNodes().Cast<AstNode>(); } }

        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            Name = treeNode.FirstChild.FindTokenAndGetText();

            if (treeNode.LastChild.ChildNodes.Any())
            {
                var argumentsRootNode = treeNode.LastChild.FirstChild;
                AddChildNodes(argumentsRootNode);
            }

            AsString = Name + "(systemFunction)";
        }

        private void AddChildNodes(ParseTreeNode argumentsRootNode)
        {
            foreach (var argNode in argumentsRootNode.ChildNodes)
            {
                AddChild("Argument", argNode);
            }
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}