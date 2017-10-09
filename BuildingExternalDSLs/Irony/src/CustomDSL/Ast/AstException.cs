using System;
using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class AstException : Exception
    {
        public AstNode Node { get; }

        public AstException(AstNode node, string message)
            : base(message)
        {
            Node = node;

        }
    }
}