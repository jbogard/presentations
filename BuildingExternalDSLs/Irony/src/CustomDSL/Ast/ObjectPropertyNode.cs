using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class ObjectPropertyNode : AstNode
    {
        public string VariableName { get; private set; }
        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            VariableName = treeNode.ChildNodes[1].FirstChild.FindTokenAndGetText();
        }
    }
}