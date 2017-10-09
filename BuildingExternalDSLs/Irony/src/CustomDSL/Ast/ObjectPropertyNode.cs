
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class ObjectPropertyNode : AstNode
    {
        public string VariableName { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            VariableName = treeNode.ChildNodes[1].FirstChild().FindTokenAndGetText();
        }
    }
}