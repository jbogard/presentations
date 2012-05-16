using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class LiteralNode : AstNode
    {
        public object Value { get; protected set; }

        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = treeNode.Token.Value;
            AsString = Value.ToString();
            
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class StringNode : LiteralNode
    {

    }

    public class NumberNode : LiteralNode
    {
        
    }
}