
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class NilNode : AstNode
    {
        public object Value { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "NULL";
            Value = null;
        }
    }
}