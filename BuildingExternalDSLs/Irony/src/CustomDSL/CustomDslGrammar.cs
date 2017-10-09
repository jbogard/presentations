using System;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using CustomDsl.Ast;

namespace CustomDsl
{
    public class CustomDslGrammar : Grammar
    {
        public CustomDslGrammar()
            : base(false)
        {

            #region Non-terminals

            var Number = new NumberLiteral("number", NumberOptions.AllowSign, typeof (NumberNode))
            {
                DefaultFloatType = TypeCode.Decimal
            };

            var StringLiteral = new StringLiteral("string", "'", StringOptions.AllowsDoubledQuote, typeof (StringNode));

            var Comma = new NonTerminal("comma")
            {
                Rule = ToTerm(","),
                Flags = TermFlags.NoAstNode
            };
            var Not = new NonTerminal("not")
            {
                Rule = ToTerm("NOT"),
                Flags = TermFlags.NoAstNode
            };
            var LParen = new NonTerminal("lparen")
            {
                Rule = ToTerm("("),
                Flags = TermFlags.NoAstNode
            };
            var RParen = new NonTerminal("rparen")
            {
                Rule = ToTerm(")"),
                Flags = TermFlags.NoAstNode
            };

            var UnaryOperator = new NonTerminal("unaryOperator")
            {
                Rule = ToTerm("+") | "-" | Not,
                Flags = TermFlags.NoAstNode
            };

            var BinaryOperator = new NonTerminal("binaryOperator")
            {
                Rule = ToTerm("+") | "-" | "*" | "/" | "%" //arithmetic
                       | "&" | "|" | "^" //bit
                       | "=" | ">" | "<" | ">=" | "<=" | "<>"
                       | "AND" | "OR",
                Flags = TermFlags.NoAstNode
            };

            var date = new NonTerminal("date", typeof (DateNode));
            var boolean = new NonTerminal("boolean", typeof (BooleanNode));
            var nil = new NonTerminal("null", typeof (NilNode));

            var expression = new NonTerminal("expression");
            var unaryExpression = new NonTerminal("unaryExpression", typeof (UnaryExpressionNode));
            var binaryExpression = new NonTerminal("binaryExpression", typeof (BinaryExpressionNode));
            var functionCall = new NonTerminal("functionCall", typeof (FunctionNode));
            var terminal = new NonTerminal("term") {Flags = TermFlags.NoAstNode};
            var tuple = new NonTerminal("tuple", typeof (TupleNode));

            var logicFunction = new NonTerminal("logicFunction", typeof (FunctionNode));
            var mathFunction = new NonTerminal("mathFunction", typeof (FunctionNode));
            var stringFunction = new NonTerminal("stringFunction", typeof (FunctionNode));

            var iifFunction = new NonTerminal("iif", typeof (IifFunctionNode));
            var icaseFunction = new NonTerminal("icase", typeof (IcaseFunctionNode));
            var caseCondition = new NonTerminal("case", typeof (CaseConditionNode));
            var caseConditionList = new NonTerminal("caselist", typeof (CaseConditionListNode));

            var powerFunction = new NonTerminal("power", typeof (FunctionNode));
            var minFunction = new NonTerminal("min", typeof (MinFunctionNode));
            var maxFunction = new NonTerminal("max", typeof (MaxFunctionNode));

            var substringFunction = new NonTerminal("substring", typeof (FunctionNode));
            var concatFunction = new NonTerminal("concat", typeof (ConcatFunctionNode));
            var leftFunction = new NonTerminal("left", typeof (FunctionNode));
            var rightFunction = new NonTerminal("right", typeof (FunctionNode));

            var fieldName = new NonTerminal("fieldName") { Flags = TermFlags.NoAstNode };
            var expressionList = new NonTerminal("expressionList", typeof (ExpressionListNode));

            var today = new NonTerminal("today", typeof (TodayNode));

            var objectProperty = new NonTerminal("objectProperty", typeof (ObjectPropertyNode));


            #endregion

            #region Expression

            expression.Rule = terminal
                              | unaryExpression
                              | binaryExpression;


            expressionList.Rule = MakePlusRule(expressionList, Comma, expression);

            #endregion

            #region Literals

            date.Rule = ToTerm("Date") + LParen + StringLiteral + RParen;

            boolean.Rule = ToTerm("true")
                           | ToTerm("false")
                ;

            nil.Rule = ToTerm("null");

            #endregion

            #region Terminals and Expressions

            terminal.Rule = StringLiteral
                            | Number
                            | date
                            | boolean
                            | nil
                            | functionCall
                            | tuple
                            | today
                            | objectProperty
                ;

            tuple.Rule = LParen + expression + RParen;
            unaryExpression.Rule = UnaryOperator + expression;

            binaryExpression.Rule = expression + BinaryOperator + expression;

            #endregion

            #region Functions

            functionCall.Rule = logicFunction
                                | mathFunction
                                | stringFunction
                ;

            logicFunction.Rule = iifFunction
                                 | icaseFunction
                ;

            mathFunction.Rule = powerFunction
                                | minFunction
                                | maxFunction
                ;

            stringFunction.Rule = substringFunction
                                  | concatFunction
                                  | leftFunction
                                  | rightFunction
                ;

            #endregion

            #region Logic Functions

            iifFunction.Rule = ToTerm("IIF") + LParen + expression + Comma + expression + Comma + expression + RParen;
            icaseFunction.Rule = ToTerm("ICASE") + LParen + caseConditionList + (Comma + expression).Q() + RParen;
            caseCondition.Rule = expression + Comma + expression;
            caseConditionList.Rule = MakePlusRule(caseConditionList, Comma, caseCondition);

            #endregion

            #region Math Functions

            powerFunction.Rule = ToTerm("POWER") + LParen + expression + Comma + expression + RParen;
            minFunction.Rule = ToTerm("MIN") + LParen + expression + Comma + expression + RParen;
            maxFunction.Rule = ToTerm("MAX") + LParen + expression + Comma + expression + RParen;

            #endregion

            #region String Functions

            substringFunction.Rule = ToTerm("SUBSTRING") + LParen + expression + Comma + expression + Comma + expression +
                                     RParen;
            concatFunction.Rule = ToTerm("CONCAT") + LParen + expression + Comma + expression + RParen;
            leftFunction.Rule = ToTerm("LEFT") + LParen + expression + Comma + expression + RParen;
            rightFunction.Rule = ToTerm("RIGHT") + LParen + expression + Comma + expression + RParen;

            #endregion

            #region Special Functions

            today.Rule = ToTerm("#{TODAY}");
            fieldName.Rule = TerminalFactory.CreateCSharpIdentifier("id");
            objectProperty.Rule = ToTerm("$") + fieldName;

            #endregion

            #region Grammar Metadata

            LanguageFlags = LanguageFlags.CreateAst;

            RegisterOperators(10, "*", "/", "%");
            RegisterOperators(9, "+", "-");
            RegisterOperators(8, "=", ">", "<", ">=", "<=", "<>", "!=", "!<", "!>");
            RegisterOperators(7, "^", "&", "|");
            RegisterOperators(6, "NOT");
            RegisterOperators(5, "AND");
            RegisterOperators(4, "OR", "LIKE", "IN");

            MarkPunctuation(",", "(", ")", "}", "{", "${", ".", "?", "#{");

            MarkTransient(terminal, expression, functionCall, logicFunction, mathFunction, stringFunction);

            #endregion

            Root = expression;
        }
    }
}