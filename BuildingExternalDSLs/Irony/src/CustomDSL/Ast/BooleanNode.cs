using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class BooleanNode : AstNode
    {
        public Boolean Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var stringfiedValue = treeNode.FirstChild().FindTokenAndGetText();

            bool value;

            if(!Boolean.TryParse(stringfiedValue,out value))
            {
                var message = $"{stringfiedValue} is not a valid boolean value";
                throw new AstException(this,message);

            }

            Value = value;
            AsString = stringfiedValue + "(boolean)";
        }
    }
}