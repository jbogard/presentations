using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Irony.Interpreter.Ast;
using CustomDsl.Ast;
using static System.Linq.Expressions.Expression;

namespace CustomDsl.ExpressionBuilding
{
    public interface ICSharpExpressionBuilder
    {
        Func<T, object> Build<T>(AstNode rootNode);
        Func<object> Build(AstNode rootNode);
    }

    public class CSharpExpressionBuilder : ICSharpExpressionBuilder
    {
        private readonly Dictionary<string, string> _methodNames;

        private ParameterExpression _expressionContext;
        private Type _type;
        private readonly IExpressionTypeResolver _expressionTypeResolver;

        public CSharpExpressionBuilder()
        {
            _expressionTypeResolver = new ExpressionTypeResolver();

            _methodNames = new Dictionary<string, string>
            {
                {"+", "Add"},
                {"-", "Subtract"},
                {"*", "Multiply"},
                {"/", "Divide"},
                {"%", "Modulus"},
                {"AND", "AndAlso"},
                {"OR", "OrElse"},
                {"=", "Equal"},
                {"<>", "NotEqual"},
                {">", "GreaterThan"},
                {">=", "GreaterThanOrEqual"},
                {"<", "LessThan"},
                {"<=", "LessThanOrEqual"},
                {"abs", "Abs"},
                {"ceiling", "Ceiling"},
                {"floor", "Floor"},
                {"power", "Pow"},
                {"substring", "Substring"},
                {"contains", "Contains"},
                {"left", "Left"},
                {"right", "Right"},
                {"len", "Length"},
                {"replace", "Replace"},
            };
        }

        public Func<T, object> Build<T>(AstNode rootNode)
        {
            Expression<Func<object, int>> foo = o => 1;

            Func<object, int> foobar = o => 1;

            Func<object, int> compile = foo.Compile();
            compile(null);

            _expressionContext = Parameter(typeof(T), "object");
            _type = typeof(T);
            var expr = InnerBuild(rootNode);
            var expression = Lambda<Func<T, object>>(
                Convert(expr, typeof(object)), _expressionContext);
            var func = expression.Compile();
            return ctxt =>
            {
                try
                {
                    return func(ctxt);
                }
                catch (Exception e)
                {
                    return e;
                }
            };
        }

        public Func<object> Build(AstNode rootNode)
        {
            var result = Build<object>(rootNode);

            return () => result(null);
        }

        private Expression InnerBuild(AstNode node)
        {
            if (node == null)
                return null;

            switch (node)
            {
                case LiteralNode n: return Build(n);
                case DateNode n: return Build(n);
                case BooleanNode n: return Build(n);
                case NilNode n: return Build(n);
                case BinaryExpressionNode n: return Build(n);
                case UnaryExpressionNode n: return Build(n);
                case TupleNode n: return Build(n);
                case IifFunctionNode n: return Build(n);
                case ConcatFunctionNode n: return Build(n);
                case MinFunctionNode n: return Build(n);
                case MaxFunctionNode n: return Build(n);
                case FunctionNode n: return Build(n);
                case IcaseFunctionNode n: return Build(n);
                case TodayNode n: return Build(n);
                case ObjectPropertyNode n: return Build(n);
                default: return null;
            }
        }

        private Expression Build(LiteralNode node) => Constant(node.Value);

        private Expression Build(DateNode node) => Constant(node.Value);

        private Expression Build(BooleanNode node) => Constant(node.Value);

        private Expression Build(NilNode node) => Constant(null);

        private Expression Build(BinaryExpressionNode node)
        {
            string methodName;

            if (!_methodNames.TryGetValue(node.Op, out methodName))
                return null;

            Expression expression;

            if (methodName == "Equal" || methodName == "NotEqual")
                expression = GetExpression(methodName, new NodeInfo(node.Left, false), new NodeInfo(node.Right, false));
            else
                expression = GetExpression(methodName, node.Left, node.Right);

            return expression;
        }

        private Expression Build(UnaryExpressionNode node)
        {
            Expression expression;

            switch (node.Op)
            {
                case "-":
                    expression = GetExpression("Negate", node.Argument);
                    break;
                case "+":
                    expression = InnerBuild(node.Argument);
                    break;
                case "NOT":
                    expression = GetExpression("Not", node.Argument);
                    break;
                default:
                    expression = null;
                    break;
            }
            return expression;
        }

        private Expression Build(TupleNode node) => InnerBuild(node.Expression);

        private Expression Build(IifFunctionNode node)
        {
            var expression = GetExpression("Iif", node.Test, new NodeInfo(node.IfTrue, false), new NodeInfo(node.IfFalse, false));

            return expression;
        }

        private Expression Build(FunctionNode node)
        {
            string methodName;

            if (!_methodNames.TryGetValue(node.FunctionName.ToLower(), out methodName))
                return null;

            var expression = GetExpression(methodName, node.Arguments);

            return expression;
        }

        private Expression Build(ConcatFunctionNode node)
        {
            var expression = GetExpression("Concat", node.FirstValue, node.LastValue);

            return expression;
        }

        private Expression Build(MinFunctionNode node)
        {
            var expression = GetExpression("Min", node.Arguments);

            return expression;
        }

        private Expression Build(MaxFunctionNode node)
        {
            var expression = GetExpression("Max", node.Arguments);

            return expression;
        }

        private Expression Build(IcaseFunctionNode node)
        {
            Expression ifFalseExpr = Convert(InnerBuild(node.ElseResultNode), typeof(object));
            Expression conditionExpr = null;
            foreach (var caseConditionNode in node.CaseConditionNodes.Reverse())
            {
                var conditionNode = InnerBuild(caseConditionNode.ConditionNode);

                if (conditionNode.NodeType == ExpressionType.Constant && ((ConstantExpression)conditionNode).Value == null)
                    return Constant(null);

                var testExpr = Convert(conditionNode, typeof(bool));
                var ifTrueExpr = Convert(InnerBuild(caseConditionNode.ResultNode), typeof(object));
                conditionExpr = Condition(testExpr, ifTrueExpr, ifFalseExpr, typeof(object));
                ifFalseExpr = conditionExpr;
            }
            return conditionExpr;
        }

        private Expression Build(TodayNode node)
        {
            var mi = typeof(DateTime).GetProperty("Now", BindingFlags.Public | BindingFlags.Static).GetGetMethod();
            var contextCall = Call(mi);

            return contextCall;
        }

        private Expression Build(ObjectPropertyNode node)
        {
            var mi = _type.GetProperty(node.VariableName, BindingFlags.Public | BindingFlags.Instance)
                .GetGetMethod();
            var contextCall = Call(_expressionContext, mi);
            return contextCall;
        }

        private Expression GetExpression(string methodName, IEnumerable<AstNode> nodes)
        {
            return GetExpression(methodName, nodes.Select(x => new NodeInfo(x, true)).ToArray());
        }

        private Expression GetExpression(string methodName, params NodeInfo[] nodes)
        {
            var expressions = nodes.Select(x => new { Expression = InnerBuild(x.Node), Type = _expressionTypeResolver.GetTypeFor(x.Node, _type), x.ReturnIfNull });

            var nullExpression = Constant(null);

            Expression returnNullTest = Constant(false);
            returnNullTest = expressions.Where(x => x.ReturnIfNull).Select(x => x.Expression).Aggregate(returnNullTest, (current, exp) => Or(current, ReferenceEqual(Convert(exp, typeof(object)), nullExpression)));

            var method = CSharpExpressionHelperMethodResolver.GetMethod(methodName, expressions.Select(x => x.Type).ToArray());

            if (method == null)
                return null;

            var parameterTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();

            var arguments = expressions.Select((x, i) => GetArgument(x.Expression, parameterTypes[i])).ToArray();

            var methodCall = Convert(Call(method, arguments), typeof(object));

            return Condition(returnNullTest, nullExpression, methodCall);
        }

        private static Expression GetArgument(Expression expression, Type type)
        {
            var convertToObject = Convert(expression, typeof(object));
            var compareToNull = ReferenceEqual(convertToObject, Constant(null));
            var convertMethod = typeof(CSharpExpressionHelper).GetMethod("ChangeType");
            var convertToType = Convert(Call(convertMethod, convertToObject, Constant(type)), type);

            return Condition(compareToNull, Default(type), convertToType);
        }

        private class NodeInfo
        {
            public NodeInfo(AstNode node, bool returnIfNull)
            {
                Node = node;
                ReturnIfNull = returnIfNull;
            }

            public AstNode Node { get; }
            public bool ReturnIfNull { get; }

            public static implicit operator NodeInfo(AstNode node)
            {
                return new NodeInfo(node, true);
            }
        }
    }
}
