using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class CaseConditionNode : AstNode
    {
        public AstNode ConditionNode { get; private set; }
        public AstNode ResultNode { get; private set; }

        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            ConditionNode = (AstNode)treeNode.FirstChild.AstNode;
            ResultNode = (AstNode)treeNode.LastChild.AstNode;
        }
    }
}