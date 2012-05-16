using System;
using System.Collections.Generic;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class FunctionNode : AstNode
    {
        public string FunctionName { get; private set; }
        public IEnumerable<AstNode> Arguments { get { return _args; } }
        protected List<AstNode> _args;
        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            FunctionName = treeNode.FirstChild.FindTokenAndGetText();

            AsString = FunctionName + "(functionCall)";

            _args = new List<AstNode>();
            for (int i = 1; i <= treeNode.ChildNodes.Count - 1; i++)
            {
                var node = treeNode.ChildNodes[i];

                if(node.AstNode != null)
                {
                    AddChild("Arg", node);
                    _args.Add((AstNode)node.AstNode);    
                }
            }
        }
    }

    public class IifFunctionNode : FunctionNode
    {
        
    }

    public class IsNullFunctionNode : FunctionNode
    {
        
    }

    public class BetweenFunctionNode : FunctionNode
    {

    }

    public class ConcatFunctionNode : FunctionNode
    {
        
    }

    public class TrimFunctionNode : FunctionNode
    {

    }

    public class TrimStartFunctionNode : FunctionNode
    {
        
    }

    public class TrimEndFunctionNode : FunctionNode
    {

    }
}