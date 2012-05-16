using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class UnaryExpressionNode : AstNode
    {
        public AstNode Argument { get; private set; }
        public string Op { get; private set; }

        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var op = treeNode.FirstChild.FindTokenAndGetText();
            Op = op;
            Argument = (AstNode)treeNode.LastChild.AstNode;

        }
        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}