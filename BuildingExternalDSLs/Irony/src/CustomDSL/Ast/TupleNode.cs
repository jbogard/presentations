using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class TupleNode : AstNode
    {
        public AstNode Expression { get; private set; }
        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AddChild("expression",treeNode.FirstChild);
            Expression = (AstNode)treeNode.FirstChild.AstNode;
        }
    }
}