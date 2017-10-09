
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class ConcatFunctionNode : FunctionNode
    {
        public AstNode FirstValue { get; private set; }
        public AstNode LastValue { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            FirstValue = (AstNode)treeNode.ChildNodes[1].AstNode;
            LastValue = (AstNode)treeNode.ChildNodes[2].AstNode;
        }
    }
}