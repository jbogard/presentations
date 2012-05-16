using System;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class DateNode : AstNode
    {
        public DateTime Value { get; private set; }
        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var date = new DateTime();
            var cleanDate = GetCleanDate(treeNode);

            if(!DateTime.TryParse(GetCleanDate(treeNode), out date))
            {
                var message = string.Format("{0} is not a valid date",GetCleanDate(treeNode));
                throw new AstException(this,message);
            }

            Value = date;
            AsString = cleanDate;

        }

        private string GetCleanDate(ParseTreeNode treeNode)
        {
            return treeNode.LastChild.FindTokenAndGetText().Replace("\'", "");
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}