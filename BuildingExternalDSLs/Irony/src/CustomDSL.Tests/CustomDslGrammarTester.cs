using System;
using System.Linq;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using NUnit.Framework;
using CustomDsl.Ast;
using Should;

namespace CustomDsl.Tests
{
    [TestFixture]
    public class CustomDslGrammarTester
    {
        private Parser _parser;

        [SetUp]
        public void SetUp()
        {
            var grammar = new CustomDslGrammar();
            var language = new LanguageData(grammar);
            _parser = new Parser(language);
        }

        [Test]
        public void Should_NOT_be_case_sensitive()
        {
            var g = new CustomDslGrammar();
            g.CaseSensitive.ShouldBeFalse();
        }

        [Test]
        public void Should_Parse_a_simple_string_literal()
        {
            var ast = (AstNode)_parser.Parse("'Some String'").Root.AstNode;
            ast.ShouldBeType<StringNode>();
            ((StringNode)ast).Value.ShouldEqual("Some String");
            ast.Term.Name.ShouldEqual("string");
        }

        [Test]
        public void Should_parse_a_string_literal_with_double_quoted_content()
        {
            var ast = (AstNode)_parser.Parse("'Some \"String\"'").Root.AstNode;
            ast.ShouldBeType<StringNode>();
            ((StringNode)ast).Value.ShouldEqual("Some \"String\"");
            ast.Term.Name.ShouldEqual("string");
        }

        [Test]
        public void Should_parse_a_integer_literal()
        {
            var ast = (AstNode)_parser.Parse("123").Root.AstNode;
            ast.ShouldBeType<NumberNode>();
            ((NumberNode) ast).Value.ShouldEqual(123);
            ast.Term.Name.ShouldEqual("number");
        }

        [Test]
        public void Should_parse_a_negative_integer_literal()
        {
            var ast = (AstNode)_parser.Parse("-123").Root.AstNode;
            ast.ShouldBeType<NumberNode>();
            ((NumberNode)ast).Value.ShouldEqual(-123);
            ast.Term.Name.ShouldEqual("number");
        }
        
        [Test]
        public void Should_parse_date_literals()
        {
            var ast = (AstNode)_parser.Parse("Date('7/18/2011')").Root.AstNode;
            ast.ShouldBeType<DateNode>();
            ((DateNode)ast).Value.ShouldEqual(new DateTime(2011,7,18));
            ast.Term.Name.ShouldEqual("date");
            ast.AsString.ShouldEqual("7/18/2011");
        }

        [Test]
        public void Should_parse_a_decimal()
        {
            var ast = (AstNode)_parser.Parse("10.5").Root.AstNode;
            ast.ShouldBeType<NumberNode>();
            ((NumberNode)ast).Value.ShouldEqual(10.5m);
            ast.AsString.ShouldEqual("10.5");
            ast.Term.Name.ShouldEqual("number");
        }

        [Test]
        public void Should_parse_a_boolean()
        {
            var ast = (AstNode)_parser.Parse("true").Root.AstNode;
            ast.ShouldBeType<BooleanNode>();
            ((BooleanNode) ast).Value.ShouldBeTrue();
            
            ast = (AstNode)_parser.Parse("false").Root.AstNode;
            ast.ShouldBeType<BooleanNode>();
            ((BooleanNode)ast).Value.ShouldBeFalse();
        }

        [Test]
        public void Should_parse_null()
        {
            var ast = (AstNode)_parser.Parse("null").Root.AstNode;
            ast.ShouldBeType<NilNode>();
            ast.AsString.ShouldEqual("NULL");
            ((NilNode)ast).Value.ShouldBeNull();
        }

        [Test]
        public void Should_parse_Add()
        {
            EnsureCanParseBinaryExpression("A + B", "A", "B", "+");
        }

        [Test]
        public void Should_parse_Subtract()
        {
            EnsureCanParseBinaryExpression("A - B", "A", "B", "-");
        }

        [Test]
        public void Should_parse_Multiply()
        {
            EnsureCanParseBinaryExpression("A * B", "A", "B", "*");
        }

        [Test]
        public void Should_parse_Divide()
        {
            EnsureCanParseBinaryExpression("A / B", "A", "B", "/");
        }

        [Test]
        public void Should_parse_Modulus()
        {
            EnsureCanParseBinaryExpression("A % B", "A", "B", "%");
        }

        [Test]
        public void Should_parse_And()
        {
            EnsureCanParseBinaryExpression("A AND B", "A", "B", "AND");
        }

        [Test]
        public void Should_parse_Or()
        {
            EnsureCanParseBinaryExpression("A OR B", "A", "B", "OR");
        }

        [Test]
        public void Should_parse_Equal()
        {
            EnsureCanParseBinaryExpression("A = B", "A", "B", "=");
        }

        [Test]
        public void Should_parse_Not_Equal()
        {
            EnsureCanParseBinaryExpression("A <> B", "A", "B", "<>");
        }

        [Test]
        public void Should_parse_GreaterThan()
        {
            EnsureCanParseBinaryExpression("A > B", "A", "B", ">");
        }

        [Test]
        public void Should_parse_GreaterThanEqual()
        {
            EnsureCanParseBinaryExpression("A >= B", "A", "B", ">=");
        }

        [Test]
        public void Should_parse_LessThan()
        {
            EnsureCanParseBinaryExpression("A < B", "A", "B", "<");
        }

        [Test]
        public void Should_parse_LessThanEqual()
        {
            EnsureCanParseBinaryExpression("A <= B", "A", "B", "<=");
        }

        private void EnsureCanParseBinaryExpression(string expression, string expectedFirstChildValue, string expectedLastChildValue, string expectedOperator)
        {
            var ast = (AstNode)_parser.Parse(expression).Root.AstNode;
            ast.ShouldBeType<BinaryExpressionNode>();
            ast.AsString.ShouldEqual(expectedOperator+"(operator)");

            var left = ast.ChildNodes[0];
            left.Role.ShouldEqual("Arg");
            left.ShouldBeType<IdNode>();
            ((IdNode)left).Symbol.ShouldEqual(expectedFirstChildValue);

            var right = ast.ChildNodes[1];
            right.Role.ShouldEqual("Arg");
            right.ShouldBeType<IdNode>();
            ((IdNode)right).Symbol.ShouldEqual(expectedLastChildValue);
        }

        [Test]
        public void Should_parse_nullCoalescing()
        {
            var ast = (AstNode)_parser.Parse("A ?? B").Root.AstNode;
            ast.AsString.ShouldEqual("??(operator)");
            var left = ast.ChildNodes[0];
            left.Role.ShouldEqual("value");
            left.ShouldBeType<IdNode>();
            ((IdNode)left).Symbol.ShouldEqual("A");

            var right = ast.ChildNodes[1];
            right.Role.ShouldEqual("defaultValue");
            right.ShouldBeType<IdNode>();
            ((IdNode)right).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_binaryExpression_with_NOT()
        {
            var ast = (AstNode)_parser.Parse("NOT B").Root.AstNode;
            ast.ShouldBeType<BinaryExpressionNode>();
            ((BinaryExpressionNode)ast).Op.ShouldEqual("NOT");
            var arg = ((BinaryExpressionNode)ast).Left;
            arg.ShouldBeType<IdNode>();
            ((IdNode)arg).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_unaryExpression_with_Plus()
        {
            var ast = (AstNode)_parser.Parse("+ B").Root.AstNode;
            ast.ShouldBeType<UnaryExpressionNode>();
            ((UnaryExpressionNode)ast).Op.ShouldEqual("+");
            var arg = ((UnaryExpressionNode)ast).Argument;
            arg.ShouldBeType<IdNode>();
            ((IdNode)arg).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_unaryExpression_with_Minus()
        {
            var ast = (AstNode)_parser.Parse("- B").Root.AstNode;
            ast.ShouldBeType<UnaryExpressionNode>();
            ((UnaryExpressionNode)ast).Op.ShouldEqual("-");
            var arg = ((UnaryExpressionNode)ast).Argument;
            arg.ShouldBeType<IdNode>();
            ((IdNode)arg).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_unaryExpression_with_Tilde()
        {
            var ast = (AstNode)_parser.Parse("~ B").Root.AstNode;
            ast.ShouldBeType<UnaryExpressionNode>();
            ((UnaryExpressionNode)ast).Op.ShouldEqual("~");
            var arg = ((UnaryExpressionNode)ast).Argument;
            arg.ShouldBeType<IdNode>();
            ((IdNode)arg).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_tuples()
        {
            var ast = (AstNode)_parser.Parse("A + (B - C)").Root.AstNode;
            ast.ShouldBeType<BinaryExpressionNode>();
            var rootOperation = (BinaryExpressionNode)ast;

            rootOperation.Op.ShouldEqual("+");
            rootOperation.Left.ShouldBeType<IdNode>();
            ((IdNode)rootOperation.Left).Symbol.ShouldEqual("A");

            rootOperation.Right.ShouldBeType<TupleNode>();

            var rightOperation = (TupleNode)rootOperation.Right;
            rightOperation.Expression.ShouldBeType<BinaryExpressionNode>();
            var rightExpression = (BinaryExpressionNode) rightOperation.Expression;
            rightExpression.Op.ShouldEqual("-");
            rightExpression.Left.ShouldBeType<IdNode>();
            ((IdNode)rightExpression.Left).Symbol.ShouldEqual("B");

            rightExpression.Right.ShouldBeType<IdNode>();
            ((IdNode)rightExpression.Right).Symbol.ShouldEqual("C");
        }

        [Test]
        public void Should_parse_IIF_function()
        {
            var ast = (AstNode)_parser.Parse("IIF(A,B,C)").Root.AstNode;
            ast.ShouldBeType<IifFunctionNode>();
            var functionNode = (IifFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("IIF");

            functionNode.Arguments.Count().ShouldEqual(3);
            functionNode.Arguments.All(a => a.GetType().IsAssignableFrom(typeof(IdNode))).ShouldBeTrue();
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
            ((IdNode)args[2]).Symbol.ShouldEqual("C");
        }

        [Test]
        public void Should_parse_ISNULL_function()
        {
            var ast = (AstNode)_parser.Parse("ISNULL(A)").Root.AstNode;
            ast.ShouldBeType<IsNullFunctionNode>();
            var functionNode = (IsNullFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("ISNULL");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();
            arg.ShouldBeType<IdNode>();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_BETWEEN_function()
        {
            var ast = (AstNode)_parser.Parse("BETWEEN(A,B,C)").Root.AstNode;
            ast.ShouldBeType<BetweenFunctionNode>();
            var functionNode = (BetweenFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("BETWEEN");

            functionNode.Arguments.Count().ShouldEqual(3);
            functionNode.Arguments.All(a => a.GetType().IsAssignableFrom(typeof(IdNode))).ShouldBeTrue();
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
            ((IdNode)args[2]).Symbol.ShouldEqual("C");
        }

        [Test]
        public void Should_parse_ABS_function()
        {
            var ast = (AstNode)_parser.Parse("ABS(A)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("ABS");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();
            arg.ShouldBeType<IdNode>();

            ((IdNode)arg).Symbol.ShouldEqual("A");
            
        }

        [Test]
        public void Should_parse_CEILING_function()
        {
            var ast = (AstNode)_parser.Parse("CEILING(A)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("CEILING");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();
            arg.ShouldBeType<IdNode>();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_FLOOR_function()
        {

            var ast = (AstNode)_parser.Parse("FLOOR(A)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("FLOOR");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();
            arg.ShouldBeType<IdNode>();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_POWER_function()
        {
            var ast = (AstNode)_parser.Parse("POWER(A,2)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("POWER");

            functionNode.Arguments.Count().ShouldEqual(2);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((LiteralNode)args[1]).Value.ShouldEqual(2);
        }

        [Test]
        public void Should_parse_Round_function()
        {
            var ast = (AstNode)_parser.Parse("ROUND(A,B)").Root.AstNode;
            ast.ShouldBeType<RoundFunctionNode>();
            var functionNode = (RoundFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("ROUND");

            functionNode.Arguments.Count().ShouldEqual(2);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_Round_function_with_3_args()
        {
            var ast = (AstNode)_parser.Parse("ROUND(A,B,C)").Root.AstNode;
            ast.ShouldBeType<RoundFunctionNode>();
            var functionNode = (RoundFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("ROUND");

            functionNode.Arguments.Count().ShouldEqual(3);
            functionNode.Arguments.All(a => a.GetType().IsAssignableFrom(typeof(IdNode))).ShouldBeTrue();
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
            ((IdNode)args[2]).Symbol.ShouldEqual("C");
            args[2].Role.ShouldEqual("truncateFunction");
        }

        [Test]
        public void Should_parse_SUBSTRING_function()
        {
            var ast = (AstNode)_parser.Parse("SUBSTRING(A,B,C)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("SUBSTRING");

            functionNode.Arguments.Count().ShouldEqual(3);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
            ((IdNode)args[2]).Symbol.ShouldEqual("C");
        }

        [Test]
        public void Should_parse_CONCAT_function()
        {
            var ast = (AstNode)_parser.Parse("CONCAT(A,B)").Root.AstNode;
            ast.ShouldBeType<ConcatFunctionNode>();
            var functionNode = (ConcatFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("CONCAT");

            functionNode.Arguments.Count().ShouldEqual(2);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_LEFT_function()
        {
            var ast = (AstNode)_parser.Parse("LEFT(A,B)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("LEFT");

            functionNode.Arguments.Count().ShouldEqual(2);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_RIGHT_function()
        {
            var ast = (AstNode)_parser.Parse("RIGHT(A,B)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("RIGHT");

            functionNode.Arguments.Count().ShouldEqual(2);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_CONTAINS_function()
        {
            var ast = (AstNode)_parser.Parse("CONTAINS(A,B)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("CONTAINS");

            functionNode.Arguments.Count().ShouldEqual(2);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
        }

        [Test]
        public void Should_parse_LEN_function()
        {
            var ast = (AstNode)_parser.Parse("LEN(A)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("LEN");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_TRIM_function()
        {
            var ast = (AstNode)_parser.Parse("TRIM(A)").Root.AstNode;
            ast.ShouldBeType<TrimFunctionNode>();
            var functionNode = (TrimFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("TRIM");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_TRIMEND_function()
        {
            var ast = (AstNode)_parser.Parse("TRIMEND(A)").Root.AstNode;
            ast.ShouldBeType<TrimEndFunctionNode>();
            var functionNode = (TrimEndFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("TRIMEND");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_TRIMSTART_function()
        {
            var ast = (AstNode)_parser.Parse("TRIMSTART(A)").Root.AstNode;
            ast.ShouldBeType<TrimStartFunctionNode>();
            var functionNode = (TrimStartFunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("TRIMSTART");

            functionNode.Arguments.Count().ShouldEqual(1);
            var arg = functionNode.Arguments.Single();

            ((IdNode)arg).Symbol.ShouldEqual("A");
        }

        [Test]
        public void Should_parse_REPLACE_function()
        {
            var ast = (AstNode)_parser.Parse("REPLACE(A,B,C)").Root.AstNode;
            ast.ShouldBeType<FunctionNode>();
            var functionNode = (FunctionNode)ast;
            functionNode.FunctionName.ShouldEqual("REPLACE");

            functionNode.Arguments.Count().ShouldEqual(3);
            var args = functionNode.Arguments.ToArray();

            ((IdNode)args[0]).Symbol.ShouldEqual("A");
            ((IdNode)args[1]).Symbol.ShouldEqual("B");
            ((IdNode)args[2]).Symbol.ShouldEqual("C");
        }

        [Test]
        public void Should_parse_system_fields()
        {
            var ast = _parser.Parse("${Field.Name}").Root.AstNode;
            ast.ShouldBeType<SystemFieldNode>();
            ((SystemFieldNode)ast).Name.ShouldEqual("Field.Name");
        }

        [Test]
        public void Should_parse_system_functions()
        {
            var ast = (AstNode)_parser.Parse("#{FunctionName(A,B)}").Root.AstNode;
            ast.ShouldBeType<SystemFunctionNode>();
            ((SystemFunctionNode)ast).Name.ShouldEqual("FunctionName");

            var left = ((SystemFunctionNode) ast).Arguments.First();
            var right = ((SystemFunctionNode) ast).Arguments.Last();

            left.ShouldBeType<IdNode>();
            right.ShouldBeType<IdNode>();
            ((IdNode)left).Symbol.ShouldEqual("A");
            ((IdNode)right).Symbol.ShouldEqual("B");
        }

        
    }
}