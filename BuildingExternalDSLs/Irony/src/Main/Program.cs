using System;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using CustomDsl;
using CustomDsl.Ast;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var grammar = new CustomDslGrammar();
            var parser = new Parser(grammar);
            var tree = parser.Parse("${Field.A} > 2 AND (NOT ${FieldB} < 3)");
            var rootNode = (AstNode)tree.Root.AstNode;
            var visitor = new CustomDslSqlVisitor();
            visitor.Visit(rootNode);
            Console.WriteLine(visitor.GetSqlExpression());

        }
    }
}
