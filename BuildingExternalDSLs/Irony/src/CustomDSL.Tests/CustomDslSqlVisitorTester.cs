using Irony.Interpreter.Ast;
using Irony.Parsing;
using NUnit.Framework;
using CustomDsl.Ast;
using Should;

namespace CustomDsl.Tests
{
    [TestFixture]
    public class CustomDslSqlVisitorTester
    {
        private Parser _parser;
        private CustomDslSqlVisitor _visitor;

        [SetUp]
        public void SetUp()
        {
            var grammar = new CustomDslGrammar();
            var language = new LanguageData(grammar);
            _parser = new Parser(language);
            _visitor = new CustomDslSqlVisitor();
        }

        [Test]
        public void Should_evaluate_string_literal()
        {
            var expectedValue = "'Some string literal value'";
            var tree = _parser.Parse(expectedValue);
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual(expectedValue);
        }

        [Test]
        public void Should_evaluate_integer_literal()
        {
            var expectedValue = "123";
            var tree = _parser.Parse(expectedValue);
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual(expectedValue);
        }

        [Test]
        public void Should_evaluate_negative_integer_literal()
        {
            var expectedValue = "-123";
            var tree = _parser.Parse(expectedValue);
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual(expectedValue);
        }

        [Test]
        public void Should_evaluate_date_literals()
        {
            var tree = _parser.Parse("Date('7/21/2011')");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("CONVERT(DATETIME,'7/21/2011',101)");
        }

        [Test] 
        public void Should_evaluate_a_decimal()
        {
            var tree = _parser.Parse("10.5");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("10.5");
        }

        [Test]
        public void Should_parse_a_boolean()
        {
            var tree = _parser.Parse("true");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("CASE 'True' WHEN 'true' THEN 1 ELSE 0 END");

            _visitor = new CustomDslSqlVisitor();
            tree = _parser.Parse("false");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("CASE 'False' WHEN 'true' THEN 1 ELSE 0 END");
        }

        [Test]
        public void Should_evaluate_NULL()
        {
            var tree = _parser.Parse("null");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("NULL");
        }

       [Test]
        public void Should_evaluate_ADD()
        {
            var tree = _parser.Parse("A + 3");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A + 3");
        }

        [Test]
        public void Should_evaluate_MINUS()
        {
            var tree = _parser.Parse("A - 3");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A - 3");
        }

        [Test]
        public void Should_evaluate_MULTIPLY()
        {
            var tree = _parser.Parse("A * 3");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A * 3");
        }

        [Test]
        public void Should_evaluate_DIVIDE()
        {
            var tree = _parser.Parse("A / 3");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A / 3");
        }

        [Test]
        public void Should_evaluate_MODULUS()
        {
            var tree = _parser.Parse("123 % 5");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("123 % 5");
        }

        [Test]
        public void Should_evaluate_AND()
        {
            var tree = _parser.Parse("A AND B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A AND B");
        }

        [Test]
        public void Should_evaluate_OR()
        {
            var tree = _parser.Parse("A OR B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A OR B");
        }

        [Test]
        public void Should_evaluate_EQUAL()
        {
            var tree = _parser.Parse("A = B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A = B");
        }

        [Test]
        public void Should_evaluate_NOT_EQUAL()
        {
            var tree = _parser.Parse("A <> B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A <> B");
        }

        [Test]
        public void Should_evaluate_NULL_comparison()
        {
            var tree = _parser.Parse("A = null");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A IS NULL");
        }

        [Test]
        public void Should_evaluate_NOT_EQUAL_NULL_COMPARISON()
        {
            var tree = _parser.Parse("A <> null");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A IS NOT NULL");
        }

        [Test]
        public void Should_evaluate_Greather_Than()
        {
            var tree = _parser.Parse("A > B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A > B");
        }

        [Test]
        public void Should_evaluate_Greather_Than_Equal()
        {
            var tree = _parser.Parse("A >= B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A >= B");
        }

        [Test]
        public void Should_evaluate_Less_Than()
        {
            var tree = _parser.Parse("A < B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A < B");
        }

        [Test]
        public void Should_evaluate_Less_Than_Equal()
        {
            var tree = _parser.Parse("A <= B");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("A <= B");
        }

        [Test]
        public void Should_evaluate_NULL_coalescing()
        {
            var tree = _parser.Parse("${FieldA} ?? (${FieldB}+${FieldC})");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("( CASE WHEN  FieldA  IS NULL THEN ( FieldB + FieldC ) ELSE  FieldA  END AS  FieldA )");
        }

        [Test]
        public void Should_evaluate_BinaryExpression_with_NOT()
        {
            var tree = _parser.Parse("NOT 1 > 2");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("NOT ( 1 > 2 )");
        }

        [Test]
        public void Should_evaluate_UnaryExpression_with_PLUS()
        {
            var tree = _parser.Parse("+1");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("1");
        }

        [Test]
        public void Should_evaluate_UnaryExpression_with_MINUS()
        {
            var tree = _parser.Parse("-(1+2)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("(-( 1 + 2 ))");
        }

        [Test]
        public void Should_evaluate_UnaryExpression_with_TILDE()
        {
            var tree = _parser.Parse("~1");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("(~ 1 )");
        }

        [Test]
        public void Should_evaluate_Tuples()
        {
            var tree = _parser.Parse("2*(A+B)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("2 *( A + B )");
        }

        [Test]
        public void Should_evaluate_IIF_function()
        {
            var tree = _parser.Parse("IIF(A,B,C)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("( CASE WHEN ( A ) THEN ( B ) ELSE ( C ) END )");
        }

        [Test]
        public void Should_evaluate_ISNULL_function()
        {
            var tree = _parser.Parse("ISNULL(A)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("(  A  IS NULL )");
        }

        [Test]
        public void Should_evaluate_BETWEEN_function()
        {
            var tree = _parser.Parse("BETWEEN(A,B,C)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("( A BETWEEN B AND C )");
        }

        [Test]
        public void Should_evaluate_ABS_function()
        {
            var tree = _parser.Parse("ABS(30.5)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("ABS( 30.5 )");
        }

        [Test]
        public void Should_evaluate_CEILING_function()
        {
            var tree = _parser.Parse("CEILING(30.5)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("CEILING( 30.5 )");
        }

        [Test]
        public void Should_evaluate_FLOOR_function()
        {
            var tree = _parser.Parse("FLOOR(30.5)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("FLOOR( 30.5 )");
        }

        [Test]
        public void Should_evaluate_POWER_function()
        {
            var tree = _parser.Parse("POWER(30.5,4)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("POWER( 30.5 , 4 )");
        }

        [Test]
        public void Should_evaluate_ROUND_function()
        {
            var tree = _parser.Parse("ROUND(30.54568,2)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("ROUND( 30.54568 , 2 )");
        }

        [Test]
        public void Should_evaluate_ROUND_function_with_truncate()
        {
            var tree = _parser.Parse("ROUND(30.54568,2,1)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("ROUND( 30.54568 , 2 , 1 )");
        }

        [Test]
        public void Should_evaluate_SUBSTRING_function_with_truncate()
        {
            var tree = _parser.Parse("SUBSTRING('Some String',2,1)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("SUBSTRING( 'Some String' , 2 , 1 )");
        }

        [Test]
        public void Should_evaluate_CONCAT_function()
        {
            var tree = _parser.Parse("CONCAT(A,B)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("( A + B )");
        }

        [Test]
        public void Should_evaluate_LEFT_function()
        {
            var tree = _parser.Parse("LEFT('some string',5)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("LEFT( 'some string' , 5 )");
        }

        [Test]
        public void Should_evaluate_RIGHT_function()
        {
            var tree = _parser.Parse("RIGHT('some string',5)");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("RIGHT( 'some string' , 5 )");
        }

        [Test]
        public void Should_evaluate_CONTAINS_function()
        {
            var tree = _parser.Parse("CONTAINS(${Field},'some string')");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("CONTAINS( Field , 'some string' )");
        }

        [Test]
        public void Should_evaluate_LEN_function()
        {
            var tree = _parser.Parse("LEN(${Field})");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("LEN( Field )");
        }

        [Test]
        public void Should_evaluate_TRIM_function()
        {
            var tree = _parser.Parse("TRIM(${Field})");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("RTRIM(LTRIM( Field ))");
        }

        [Test]
        public void Should_evaluate_TRIMSTART_function()
        {
            var tree = _parser.Parse("TRIMSTART(${Field})");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("LTRIM( Field )");
        }

        [Test]
        public void Should_evaluate_TRIMEND_function()
        {
            var tree = _parser.Parse("TRIMEND(${Field})");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("RTRIM( Field )");
        }
        
        [Test]
        public void Should_evaluate_REPLACE_function()
        {
            var tree = _parser.Parse("REPLACE(${Expression},'Pattern','Replacement')");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("REPLACE( Expression , 'Pattern' , 'Replacement' )");
        }

        [Test]
        public void Should_evaluate_System_Fields()
        {
            var tree = _parser.Parse("${Expression}");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("Expression");
        }

        [Test]
        public void Should_evaluate_expression_regardless_of_casing()
        {
            var tree = _parser.Parse("tRImEnD(${Field})");
            _visitor.Visit((AstNode)tree.Root.AstNode);
            _visitor.GetSqlExpression().Trim().ShouldEqual("RTRIM( Field )");
        }
    }

}