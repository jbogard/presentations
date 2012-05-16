using System;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using CustomDsl.Ast;

namespace CustomDsl
{
    public class CustomDslGrammar : Grammar
    {
        public CustomDslGrammar() : base(false)
        {
            LanguageFlags = LanguageFlags.CreateAst;

            // Terminals
            var Number = new NumberLiteral("number", NumberOptions.AllowSign,typeof(NumberNode));
            Number.DefaultFloatType = TypeCode.Decimal;
            Number.AddSuffix("m", TypeCode.Decimal);

            var StringLiteral = new StringLiteral("string", "'", StringOptions.AllowsDoubledQuote,typeof(StringNode));

            var Dot = ToTerm(".");
            var Comma = ToTerm(",");
            var Not = ToTerm("NOT");
            var LParen = ToTerm("(");
            var RParen = ToTerm(")");

            var UnaryOperator = ToTerm("+") | "-" | "~";

            var BinaryOperator = ToTerm("+") | "-" | "*" | "/" | "%" //arithmetic
                                  | "&" | "|" | "^" //bit
                                  | "=" | ">" | "<" | ">=" | "<=" | "<>"
                                  | "AND" | "OR" | "LIKE" | Not + "LIKE";

            var ID = TerminalFactory.CreateSqlExtIdentifier(this, "id");


            // Non-terminals
            var identifier = new NonTerminal("identifier", typeof(IdNode));

            var date = new NonTerminal("date", typeof(DateNode));
            var boolean = new NonTerminal("boolean", typeof(BooleanNode));
            var nil = new NonTerminal("null", typeof(NilNode));

            var expression = new NonTerminal("expression");
            var unaryExpression = new NonTerminal("unaryExpression", typeof(UnaryExpressionNode));
            var binaryExpression = new NonTerminal("binaryExpression", typeof(BinaryExpressionNode));
            var functionCall = new NonTerminal("functionCall", typeof(FunctionNode));
            var term = new NonTerminal("term");
            var tuple = new NonTerminal("tuple", typeof(TupleNode));
            var nullCoalescingExpression = new NonTerminal("nullCoalescing", typeof(NullCoalescingNode));

            var logicFunction = new NonTerminal("logicFunction", typeof(FunctionNode));
            var mathFunction = new NonTerminal("mathFunction", typeof(FunctionNode));
            var stringFunction = new NonTerminal("stringFunction", typeof(FunctionNode));

            var iifFunction = new NonTerminal("iif", typeof(IifFunctionNode));
            var isNullFunction = new NonTerminal("isNull", typeof(IsNullFunctionNode));
            var betweenFunction = new NonTerminal("between", typeof(BetweenFunctionNode));

            var absFunction = new NonTerminal("abs", typeof(FunctionNode));
            var ceilingFunction = new NonTerminal("ceiling", typeof(FunctionNode));
            var floorFunction = new NonTerminal("floor", typeof(FunctionNode));
            var powerFunction = new NonTerminal("power", typeof(FunctionNode));
            var roundFunction = new NonTerminal("round", typeof(RoundFunctionNode));

            var substringFunction = new NonTerminal("substring", typeof(FunctionNode));
            var concatFunction = new NonTerminal("concat", typeof(ConcatFunctionNode));
            var leftFunction = new NonTerminal("left", typeof(FunctionNode));
            var rightFunction = new NonTerminal("right", typeof(FunctionNode));
            var containsFunction = new NonTerminal("contains", typeof(FunctionNode));
            var lenFunction = new NonTerminal("len", typeof(FunctionNode));
            var trimFunction = new NonTerminal("trim", typeof(TrimFunctionNode));
            var trimEndFunction = new NonTerminal("trimEnd", typeof(TrimEndFunctionNode));
            var trimStartFunction = new NonTerminal("trimStart", typeof(TrimStartFunctionNode));
            var replaceFunction = new NonTerminal("replace", typeof(FunctionNode));

            var field = new NonTerminal("field", typeof(SystemFieldNode));
            var fieldName = new NonTerminal("fieldName");
            var systemFunction = new NonTerminal("systemFunction", typeof(SystemFunctionNode));
            var expressionList = new NonTerminal("expressionList", typeof(ExpressionListNode));

            this.Root = expression;


            // Expression
            expression.Rule = term
                              | unaryExpression
                              | binaryExpression
                              | nullCoalescingExpression;

            expressionList.Rule = MakePlusRule(expressionList, Comma, expression);

            //Literals
            date.Rule = ToTerm("Date") + LParen + StringLiteral + RParen;

            boolean.Rule = ToTerm("true")
                           | ToTerm("false")
                ;

            nil.Rule = ToTerm("null");

            //Operations
            term.Rule = identifier
                        | StringLiteral
                        | Number
                        | date
                        | boolean
                        | nil
                        | functionCall
                        | tuple
                        | field
                ;

            tuple.Rule = "(" + expression + ")";
            unaryExpression.Rule = UnaryOperator + term;

            binaryExpression.Rule = expression + BinaryOperator + expression 
                                    | Not + expression;


            nullCoalescingExpression.Rule = expression + ToTerm("??") + expression;

            //Functions
            functionCall.Rule = logicFunction
                                | mathFunction
                                | stringFunction
                                | systemFunction
                ;

            logicFunction.Rule = iifFunction
                                 | isNullFunction
                                 | betweenFunction
                ;

            mathFunction.Rule = absFunction
                                | ceilingFunction
                                | floorFunction
                                | powerFunction
                                | roundFunction
                ;

            stringFunction.Rule = substringFunction
                                  | concatFunction
                                  | leftFunction
                                  | rightFunction
                                  | containsFunction
                                  | lenFunction
                                  | trimFunction
                                  | trimStartFunction
                                  | trimEndFunction
                                  | replaceFunction
                ;

            //logic Functions
            iifFunction.Rule = ToTerm("IIF") + LParen + expression + Comma + expression + Comma + expression + RParen;
            isNullFunction.Rule = ToTerm("ISNULL") + LParen + expression + RParen;
            betweenFunction.Rule = ToTerm("BETWEEN") + LParen + expression + Comma + expression + Comma + expression +
                                   RParen;

            //math Functions
            absFunction.Rule = ToTerm("ABS") + LParen + expression + RParen;
            ceilingFunction.Rule = ToTerm("CEILING") + LParen + expression + RParen;
            floorFunction.Rule = ToTerm("FLOOR") + LParen + expression + RParen;
            powerFunction.Rule = ToTerm("POWER") + LParen + expression + Comma + expression + RParen;
            roundFunction.Rule = ToTerm("ROUND") + LParen + expression + Comma + expression + (Comma + expression).Q() + RParen;

            //string functions
            substringFunction.Rule = ToTerm("SUBSTRING") + LParen + expression + Comma + expression + Comma + expression + RParen;
            concatFunction.Rule = ToTerm("CONCAT") + LParen + expression + Comma + expression + RParen;
            leftFunction.Rule = ToTerm("LEFT") + LParen + expression + Comma + expression + RParen;
            rightFunction.Rule = ToTerm("RIGHT") + LParen + expression + Comma + expression + RParen;
            containsFunction.Rule = ToTerm("CONTAINS") + LParen + expression + Comma + expression + RParen;
            lenFunction.Rule = ToTerm("LEN") + LParen + expression + RParen;
            trimFunction.Rule = ToTerm("TRIM") + LParen + expression + RParen;
            trimEndFunction.Rule = ToTerm("TRIMEND") + LParen + expression + RParen;
            trimStartFunction.Rule = ToTerm("TRIMSTART") + LParen + expression + RParen;
            replaceFunction.Rule = ToTerm("REPLACE") + LParen + expression + Comma + expression + Comma + expression + RParen;

            field.Rule = ToTerm("${") + MakePlusRule(fieldName, Dot, fieldName) + "}";
            systemFunction.Rule = ToTerm("#{") + identifier + LParen + MakeStarRule(expressionList, Comma, expression) + RParen + "}";

            fieldName.Rule = identifier;

            identifier.Rule = ID;

            //Grammar Metadata
            RegisterOperators(11, "??");
            RegisterOperators(10, "*", "/", "%");
            RegisterOperators(9, "+", "-");
            RegisterOperators(8, "=", ">", "<", ">=", "<=", "<>", "!=", "!<", "!>");
            RegisterOperators(7, "^", "&", "|");
            RegisterOperators(6, "NOT");
            RegisterOperators(5, "AND");
            RegisterOperators(4, "OR", "LIKE", "IN");

            MarkPunctuation(",", "(", ")", "}", "{", "${", "#{");

            MarkTransient(term, expression, functionCall, logicFunction, mathFunction, stringFunction);

        }
    }
}
