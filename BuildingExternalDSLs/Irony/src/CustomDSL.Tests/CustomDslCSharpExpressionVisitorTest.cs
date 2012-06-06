using System;
using System.Linq.Expressions;
using CustomDsl.ExpressionBuilding;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using NUnit.Framework;
using CustomDsl.Ast;
using Should;

namespace CustomDsl.Tests
{
    [TestFixture]
    public class CustomDslCSharpExpressionVisitorTest 
    {
        private Parser _parser;

        private CSharpExpressionBuilder _visitor;

        public class Foo
        {
            public int Bar { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            var grammar = new CustomDslGrammar();
            var language = new LanguageData(grammar);
            _parser = new Parser(language);
            _visitor = new CSharpExpressionBuilder();
        }

        [Test]
        public void String()
        {
            var tree = _parser.Parse("'I am a string'");

            var func = _visitor.Build((AstNode)tree.Root.AstNode);

            func().ShouldEqual("I am a string");
        }

        [Test]
        public void Add()
        {
            var tree = _parser.Parse("1 + 2");

            var func = _visitor.Build((AstNode)tree.Root.AstNode);

            func().ShouldEqual(3);
        }

        [Test]
        public void ObjectProperty()
        {
            var tree = _parser.Parse("$Bar > 5");

            var func = _visitor.Build<Foo>((AstNode)tree.Root.AstNode);

            var invalid = new Foo {Bar = 4};
            var valid = new Foo {Bar = 6};

            func(invalid).ShouldEqual(false);
            func(valid).ShouldEqual(true);
        }
    }
}