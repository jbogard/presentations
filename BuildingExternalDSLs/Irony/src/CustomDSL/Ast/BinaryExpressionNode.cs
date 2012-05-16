using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class BinaryExpressionNode : AstNode
    {
        public AstNode Left { get; private set; }
        public AstNode Right { get; private set; }
        public string Op { get; private set; }

        public override void Init(Irony.Parsing.ParsingContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            if(treeNode.FirstChild.FindTokenAndGetText() == "NOT")
            {
                Left = AddChild("Arg",treeNode.LastChild);
                var opToken = treeNode.FirstChild.Token;
                Op = opToken.Text;
            }
            else
            {
                Left = AddChild("Arg", treeNode.ChildNodes[0]);
                Right = AddChild("Arg", treeNode.ChildNodes[2]);
                var opToken = treeNode.ChildNodes[1].FindToken();
                Op = opToken.Text;
            }
            AsString = Op + "(operator)";
        }
        
        public void AcceptVisitor(ICustomDslVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}