using CustomDsl.ExpressionBuilding;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using Shouldly;
using Xunit;

namespace CustomDsl.Tests
{
    public class CustomDslCSharpExpressionVisitorTest 
    {
        private readonly Parser _parser;

        private readonly CSharpExpressionBuilder _visitor;

        public class Foo
        {
            public int Bar { get; set; }
        }

        public CustomDslCSharpExpressionVisitorTest()
        {
            var grammar = new CustomDslGrammar();
            var language = new LanguageData(grammar);
            _parser = new Parser(language);
            _visitor = new CSharpExpressionBuilder();
        }

        [Fact]
        public void String()
        {
            var tree = _parser.Parse("'I am a string'");

            var func = _visitor.Build((AstNode)tree.Root.AstNode);

            func().ShouldBe("I am a string");
        }

        [Fact]
        public void Add()
        {
            var tree = _parser.Parse("1 + 2");

            var func = _visitor.Build((AstNode)tree.Root.AstNode);

            func().ShouldBe(3);
        }

        [Fact]
        public void ObjectProperty()
        {
            var tree = _parser.Parse("$Bar > 5");

            var func = _visitor.Build<Foo>((AstNode)tree.Root.AstNode);

            var invalid = new Foo {Bar = 4};
            var valid = new Foo {Bar = 6};

            func(invalid).ShouldBe(false);
            func(valid).ShouldBe(true);
        }
    }
}