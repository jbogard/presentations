using System;
using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class BooleanNode : AstNode
    {
        public Boolean Value { get; private set; }
        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var stringfiedValue = treeNode.FirstChild.FindTokenAndGetText();

            bool value;

            if(!Boolean.TryParse(stringfiedValue,out value))
            {
                var message = string.Format("{0} is not a valid boolean value",stringfiedValue);
                throw new AstException(this,message);

            }

            Value = value;
            AsString = stringfiedValue + "(boolean)";
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}