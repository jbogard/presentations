using System;
using System.Linq.Expressions;
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

        private CustomDslCSharpExpressionVisitor _visitor;

        [SetUp]
        public void SetUp()
        {
            var grammar = new CustomDslGrammar();
            var language = new LanguageData(grammar);
            _parser = new Parser(language);
            _visitor = new CustomDslCSharpExpressionVisitor();
        }

        [Test]
        public void String()
        {
            var tree = _parser.Parse("'I am a string'");

            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = _visitor.GetCShaprExpression();

            var func = expression.Compile();

            func().ShouldEqual("I am a string");
        }

        [Test]
        public void Add()
        {
            var tree = _parser.Parse("1 + 2");

            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = _visitor.GetCShaprExpression();

            var func = expression.Compile();

            func().ShouldEqual(3);
        }

        [Test]
        public void Should_evaluate_a_string_literal()
        {
            var tree = _parser.Parse("'Some string value'");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            ConstantExpression castedExpression = ((ConstantExpression)expression);

            castedExpression.Type.ShouldEqual(typeof(string));
            castedExpression.Value.ShouldEqual("Some string value");
        }

        [Test]
        public void Should_evaluate_a_integer_literal()
        {
            var tree = _parser.Parse("123");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            ConstantExpression castedExpression = ((ConstantExpression)expression);

            castedExpression.Type.ShouldEqual(typeof(Int32));
            castedExpression.Value.ShouldEqual(123);
        }

        [Test]
        public void Should_evaluate_a_negative_integer_literal()
        {
            var tree = _parser.Parse("-123");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            ConstantExpression castedExpression = ((ConstantExpression)expression);

            castedExpression.Type.ShouldEqual(typeof(Int32));
            castedExpression.Value.ShouldEqual(-123);
        }

        [Test]
        public void Should_evaluate_a_decimal_literal()
        {
            var tree = _parser.Parse("123.45");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            ConstantExpression castedExpression = ((ConstantExpression)expression);

            castedExpression.Type.ShouldEqual(typeof(Decimal));
            castedExpression.Value.ShouldEqual(123.45m);
        }

        [Test]
        public void Should_evaluate_a_date_literal()
        {
            var tree = _parser.Parse("Date('7/21/2011')");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            ConstantExpression castedExpression = ((ConstantExpression)expression);

            castedExpression.Type.ShouldEqual(typeof(DateTime));
            castedExpression.Value.ShouldEqual(new DateTime(2011,7,21));
        }

        [Test]
        public void Should_evaluate_a_boolean_literal()
        {
            var tree = _parser.Parse("true");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            ConstantExpression castedExpression = ((ConstantExpression)expression);

            castedExpression.Type.ShouldEqual(typeof(bool));
            castedExpression.Value.ShouldEqual(true);
        }

        [Test]
        public void Should_evaluate_null()
        {
            var tree = _parser.Parse("null");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            ConstantExpression expression = (ConstantExpression)((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;
            expression.Value.ShouldBeNull();
            expression.Type.ShouldEqual(typeof(object));
        }
        
        [Test]
        public void Should_evaluate_into_a_Add_expression()
        {
            var tree = _parser.Parse("1+2");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);
            
            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(2);
            
            castedExpression.NodeType.ShouldEqual(ExpressionType.Add);

        }

        [Test]
        public void Should_evaluate_into_a_Subtract_expression()
        {
            var tree = _parser.Parse("1-2");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(2);

            castedExpression.NodeType.ShouldEqual(ExpressionType.Subtract);
        }

        [Test]
        public void Should_evaluate_into_a_Multiply_expression()
        {
            var tree = _parser.Parse("1*2");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(2);

            castedExpression.NodeType.ShouldEqual(ExpressionType.Multiply);
        }

        [Test]
        public void Should_evaluate_into_a_Foo_expression()
        {
            var tree = _parser.Parse("1/2");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(2);

            castedExpression.NodeType.ShouldEqual(ExpressionType.Divide);
        }

        [Test]
        public void Should_evaluate_into_a_Modulo_expression()
        {
            var tree = _parser.Parse("1%2");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(2);

            castedExpression.NodeType.ShouldEqual(ExpressionType.Modulo);
        }

        [Test]
        public void Should_evaluate_AND()
        {
            var tree = _parser.Parse("true AND false");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Boolean));
            left.Value.ShouldEqual(true);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Boolean));
            right.Value.ShouldEqual(false);

            castedExpression.NodeType.ShouldEqual(ExpressionType.AndAlso);
        }

        [Test]
        public void Should_evaluate_OR()
        {
            var tree = _parser.Parse("true OR false");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Boolean));
            left.Value.ShouldEqual(true);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Boolean));
            right.Value.ShouldEqual(false);

            castedExpression.NodeType.ShouldEqual(ExpressionType.OrElse);
        }

        [Test]
        public void Should_evaluate_EQUAL()
        {
            var tree = _parser.Parse("1 = 3");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(3);

            castedExpression.NodeType.ShouldEqual(ExpressionType.Equal);
        }

        [Test]
        public void Should_evaluate_NOT_EQUAL()
        {
            var tree = _parser.Parse("1 <> 3");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(Int32));
            left.Value.ShouldEqual(1);

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Type.ShouldEqual(typeof(Int32));
            right.Value.ShouldEqual(3);

            castedExpression.NodeType.ShouldEqual(ExpressionType.NotEqual);
        }

        [Test]
        public void Should_evaluate_Null_Comparison()
        {
            var tree = _parser.Parse("'some string' = null");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(String));
            left.Value.ShouldEqual("some string");

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Value.ShouldBeNull();
            right.Type.ShouldEqual(typeof(object));
            castedExpression.NodeType.ShouldEqual(ExpressionType.Equal);
        }

        [Test]
        public void Should_evaluate_NOT_EQUAL_Null_Comparison()
        {
            var tree = _parser.Parse("'some string' <> null");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            var expression = ((UnaryExpression)_visitor.GetCShaprExpression().Body).Operand;

            BinaryExpression castedExpression = ((BinaryExpression)expression);

            ConstantExpression left = (ConstantExpression)castedExpression.Left;
            left.Type.ShouldEqual(typeof(String));
            left.Value.ShouldEqual("some string");

            ConstantExpression right = (ConstantExpression)castedExpression.Right;
            right.Value.ShouldBeNull();
            right.Type.ShouldEqual(typeof(object));
            castedExpression.NodeType.ShouldEqual(ExpressionType.NotEqual);
        }

    }
}