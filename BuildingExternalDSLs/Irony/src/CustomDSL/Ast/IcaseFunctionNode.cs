using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class IcaseFunctionNode : AstNode
    {
        public IEnumerable<CaseConditionNode> CaseConditionNodes { get; private set; }
        public AstNode ElseResultNode { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var caseListNode = (CaseConditionListNode)treeNode.ChildNodes[1].AstNode;

            CaseConditionNodes = caseListNode.CaseConditionNodes.ToArray();

            var elseNode = treeNode.ChildNodes[2].ChildNodes[0];

            if (elseNode.ChildNodes.Any())
            {
                ElseResultNode = (AstNode)elseNode.ChildNodes[0].AstNode;
            }
            else
            {
                ElseResultNode = new NilNode();
            }
        }
    }
}