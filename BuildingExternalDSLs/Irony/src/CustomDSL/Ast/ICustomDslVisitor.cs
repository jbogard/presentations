using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public interface ICustomDslVisitor
    {
        void Visit(AstNode node);
    }
}