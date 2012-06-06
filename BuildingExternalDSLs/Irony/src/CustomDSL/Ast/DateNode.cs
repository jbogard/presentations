using System;
using System.Globalization;
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

            DateTime date;
            var cleanDate = GetCleanDate(treeNode);

            if (!TryParseDate(cleanDate, out date))
            {
                var message = string.Format("{0} is not a valid date", GetCleanDate(treeNode));
                throw new AstException(this, message);
            }

            Value = date;
            AsString = cleanDate;

        }

        private bool TryParseDate(string value, out DateTime date)
        {
            return DateTime.TryParse(value, CultureInfo.CurrentUICulture, DateTimeStyles.None, out date)
                   || DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        private string GetCleanDate(ParseTreeNode treeNode)
        {
            return treeNode.LastChild.FindTokenAndGetText().Replace("\'", "");
        }
    }
}