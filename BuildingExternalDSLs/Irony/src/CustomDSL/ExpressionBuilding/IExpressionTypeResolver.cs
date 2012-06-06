using System;
using Irony.Interpreter.Ast;

namespace CustomDsl.ExpressionBuilding
{
    public interface IExpressionTypeResolver
    {
        Type GetTypeFor(AstNode rootNode, Type objectType);
    }
}