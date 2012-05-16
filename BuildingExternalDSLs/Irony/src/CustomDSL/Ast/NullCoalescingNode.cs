using System;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class NullCoalescingNode : AstNode
    {
        private ParseTreeNode _value;
        private ParseTreeNode _defaultValue;

        public AstNode Value { get { return (AstNode)_value.AstNode; } }
        public AstNode DefaultValue { get { return (AstNode)_defaultValue.AstNode; } }

        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "??(operator)";
            _value = treeNode.ChildNodes[0];
            _defaultValue = treeNode.ChildNodes[2];
            AddChild("value", _value);
            AddChild("defaultValue", _defaultValue);
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}