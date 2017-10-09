using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CustomDsl.Ast;
using Irony.Interpreter.Ast;

namespace CustomDsl.ExpressionBuilding
{
    public class ExpressionTypeResolver : IExpressionTypeResolver
    {

        private readonly Dictionary<Type, Func<AstNode, Type>> _dispatchTable;
        private readonly Dictionary<string, string> _methodNames;
        private readonly IDictionary<AstNode, Type> _typeCache;
        private readonly bool _cachingIsEnabled;
        private Type _objectType;

        public ExpressionTypeResolver()
        {
            _dispatchTable = new Dictionary<Type, Func<AstNode, Type>>
                            {
                                {typeof (StringNode), node => typeof(string)},    
                                {typeof (NumberNode), node => ((NumberNode)node).Value.GetType()},
                                {typeof(DateNode), node => typeof(DateTime)},
                                {typeof(BooleanNode), node => typeof(bool)},
                                {typeof(NilNode), node => null},
                                {typeof(BinaryExpressionNode), node => Build((BinaryExpressionNode)node)},
                                {typeof(UnaryExpressionNode), node => Build((UnaryExpressionNode)node)},
                                {typeof(TupleNode), node => Build((TupleNode)node)},
                                {typeof(IifFunctionNode), node => Build((IifFunctionNode)node)},
                                {typeof(FunctionNode), node => Build((FunctionNode)node)},
                                {typeof(ConcatFunctionNode), node => typeof(string)},
                                {typeof(MinFunctionNode), node => Build((MinFunctionNode)node)},
                                {typeof(MaxFunctionNode), node => Build((MaxFunctionNode)node)},
                                {typeof(IcaseFunctionNode), node => Build((IcaseFunctionNode)node)},
                                {typeof(TodayNode), node => Build((TodayNode)node)},
                                {typeof(ObjectPropertyNode), node => Build((ObjectPropertyNode)node)},
                            };

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
    		               };

            _typeCache = new Dictionary<AstNode, Type>(new ObjectReferenceEqualityComparerer<AstNode>());
            _cachingIsEnabled = true;
        }

        public Type GetTypeFor(AstNode rootNode, Type objectType)
        {
            _objectType = objectType;
            Type type;

            if (!_typeCache.TryGetValue(rootNode, out type))
            {
                type = InnerBuild(rootNode);

                if (_cachingIsEnabled)
                    _typeCache.Add(rootNode, type);
            }

            return type;
        }

        protected Type InnerBuild(AstNode node)
        {
            Func<AstNode, Type> func;
            _dispatchTable.TryGetValue(node.GetType(), out func);

            return func?.Invoke(node);
        }

        private Type Build(BinaryExpressionNode node)
        {
            string methodName;

            if (!_methodNames.TryGetValue(node.Op, out methodName))
                return null;

            var leftType = InnerBuild(node.Left);
            var rightType = InnerBuild(node.Right);
            var method = CSharpExpressionHelperMethodResolver.GetMethod(methodName, leftType, rightType);

            return method.ReturnType;
        }

        private Type Build(UnaryExpressionNode node)
        {
            if (node.Op == "NOT")
                return typeof(bool);
            else
                return InnerBuild(node.Argument);
        }

        private Type Build(TupleNode node)
        {
            return InnerBuild(node.Expression);
        }

        private Type Build(IifFunctionNode node)
        {
            var ifTrueType = InnerBuild(node.IfTrue);
            var ifFalseType = InnerBuild(node.IfFalse);

            var method = CSharpExpressionHelperMethodResolver.GetMethod("Iif", typeof(bool), ifTrueType, ifFalseType);

            return GetUnderlyingType(method.ReturnType);
        }

        private Type Build(FunctionNode node)
        {
            var functionName = node.FunctionName.ToLower();

            switch (functionName)
            {
                case "abs":
                    var parameterTypes = node.Arguments.Select(InnerBuild).ToArray();
                    var method = CSharpExpressionHelperMethodResolver.GetMethod("Abs", parameterTypes);

                    return method.ReturnType;
                case "ceiling":
                case "floor":
                    return typeof(decimal);
                case "power":
                    return typeof(double);
                case "substring":
                case "left":
                case "right":
                case "replace":
                    return typeof(string);
                case "contains":
                    return typeof(bool);
                case "len":
                    return typeof(int);
                default:
                    return null;
            }
        }

        private Type Build(MinFunctionNode node)
        {
            var parameterTypes = node.Arguments.Select(InnerBuild).ToArray();
            var method = CSharpExpressionHelperMethodResolver.GetMethod("Min", parameterTypes);

            return method.ReturnType;
        }

        private Type Build(MaxFunctionNode node)
        {
            var parameterTypes = node.Arguments.Select(InnerBuild).ToArray();
            var method = CSharpExpressionHelperMethodResolver.GetMethod("Max", parameterTypes);

            return method.ReturnType;
        }

        private Type Build(ObjectPropertyNode node)
        {
            var targetType = _objectType.GetProperty(node.VariableName, BindingFlags.Instance | BindingFlags.Public)?.PropertyType;

            return GetUnderlyingType(targetType);
        }

        //TODO: implement with CSharpExpressionHelperMethodResolver in order to get auto type conversion
        private Type Build(IcaseFunctionNode node)
        {
            // just picked one argument. I'm assuming the others are going to be the same.
            return InnerBuild(node.CaseConditionNodes.First().ResultNode);
        }

        private Type Build(TodayNode node)
        {
            return typeof(DateTime);
        }

        private Type GetUnderlyingType(Type targetType)
        {
            var targetTypeIsNullable = targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (targetTypeIsNullable)
            {
                targetType = Nullable.GetUnderlyingType(targetType);
            }
            return targetType;
        }
    }

    public class ObjectReferenceEqualityComparerer<T> : EqualityComparer<T> where T : class
    {
        public override bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
