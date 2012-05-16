using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class IdNode : AstNode
    {
        public string Symbol { get; private set; }

        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            Symbol = treeNode.FindTokenAndGetText();
            AsString = Symbol;
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}