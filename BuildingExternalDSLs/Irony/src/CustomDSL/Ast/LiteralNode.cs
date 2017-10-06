using System;
using System.Globalization;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class LiteralNode : AstNode
    {
        public object Value { get; protected set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = treeNode.Token.Value;
            AsString = Value.ToString();
            
        }
    }

    public class StringNode : LiteralNode
    {

    }

    public class NumberNode : LiteralNode
    {
    }
}