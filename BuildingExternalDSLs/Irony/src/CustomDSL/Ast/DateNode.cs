using System;
using System.Globalization;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class DateNode : AstNode
    {
        public DateTime Value { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            DateTime date;
            var cleanDate = GetCleanDate(treeNode);

            if (!TryParseDate(cleanDate, out date))
            {
                var message = $"{GetCleanDate(treeNode)} is not a valid date";
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
            return treeNode.LastChild().FindTokenAndGetText().Replace("\'", "");
        }
    }
}