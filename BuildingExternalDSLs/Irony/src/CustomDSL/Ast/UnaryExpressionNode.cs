using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class UnaryExpressionNode : AstNode
    {
        public AstNode Argument { get; private set; }
        public string Op { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var op = treeNode.FirstChild().FindTokenAndGetText();
            Op = op;
            Argument = (AstNode)treeNode.LastChild().AstNode;

        }
    }
}