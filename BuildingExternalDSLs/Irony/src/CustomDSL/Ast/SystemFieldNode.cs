using System.Text;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDsl.Ast
{
    public class SystemFieldNode : AstNode
    {
        private StringBuilder _name;
        public string Name { get { return _name.ToString(); } }

        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            _name = new StringBuilder();

            foreach (var node in treeNode.FirstChild.ChildNodes)
            {
                _name.Append(node.FindTokenAndGetText());
            }

            AsString = Name + "(systemField)";
        }

        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}