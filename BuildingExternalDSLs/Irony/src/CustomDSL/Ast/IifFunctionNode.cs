using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class IifFunctionNode : FunctionNode
    {
        public AstNode Test { get; private set; }
        public AstNode IfTrue { get; private set; }
        public AstNode IfFalse { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            Test = (AstNode)treeNode.ChildNodes[1].AstNode;
            IfTrue = (AstNode)treeNode.ChildNodes[2].AstNode;
            IfFalse = (AstNode)treeNode.ChildNodes[3].AstNode;
        }
    }


}