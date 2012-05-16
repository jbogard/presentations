using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class NilNode : AstNode
    {
        public object Value { get; private set; }
        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "NULL";
            Value = null;
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}