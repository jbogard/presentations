using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{
    public class CustomDslCSharpExpressionVisitor : ICustomDslVisitor
    {
        private Dictionary<Type, Func<AstNode, Expression>> dispatchTable;

        private Expression<Func<object>> _expression;
        public CustomDslCSharpExpressionVisitor()
        {
            dispatchTable = new Dictionary<Type, Func<AstNode, Expression>>()
            {
                {typeof (StringNode), node => VisitStringNode(node)},
                {typeof (NumberNode), node => VisitNumberNode(node)},
                {typeof (DateNode), node => VisitDateNode(node)},
                {typeof (BooleanNode), node => VisitBooleanNode(node)},
                {typeof (NilNode), node => VisitNullNode(node)},
                {typeof (BinaryExpressionNode), node => VisitBinaryExpressionNode(node)},
                {typeof(TupleNode), node => Build((TupleNode)node)},
            };
        }

        public void Visit(AstNode rootNode)
        {
            var expression = InnerVisit(rootNode);

            var outer = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)));

            _expression = outer;
        }

        public Expression<Func<object>> GetCShaprExpression()
        {
            return _expression;
        }

        private Expression InnerVisit(AstNode node)
        {
            Func<AstNode, Expression> func;
            dispatchTable.TryGetValue(node.GetType(), out func);

            if (func != null)
            {
                return func(node);
            }

            return null;
        }

        public Expression VisitStringNode(AstNode node)
        {
            var castedNode = ((StringNode)node);
            return Expression.Constant(castedNode.Value);
        }

        public Expression VisitNumberNode(AstNode node)
        {
            var castedNode = ((NumberNode)node);

            return Expression.Constant(castedNode.Value);
        }

        private Expression VisitDateNode(AstNode node)
        {
            var castedNode = (DateNode)node;
            return Expression.Constant(castedNode.Value);
        }

        private Expression VisitBooleanNode(AstNode node)
        {
            var castedNode = (BooleanNode)node;
            return Expression.Constant(castedNode.Value);
        }

        private Expression VisitNullNode(AstNode node)
        {
            return Expression.Constant(null);
        }

        public Expression VisitBinaryExpressionNode(AstNode node)
        {
            var castedNode = ((BinaryExpressionNode)node);
            Expression expression;

            switch (castedNode.Op)
            {
                case "+":
                    expression = Expression.Add(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "-":
                    expression = Expression.Subtract(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "*":
                    expression = Expression.Multiply(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "/":
                    expression = Expression.Divide(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "%":
                    expression = Expression.Modulo(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "AND":
                    expression = Expression.AndAlso(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "OR":
                    expression = Expression.OrElse(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "=":
                    expression = Expression.Equal(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                case "<>":
                    expression = Expression.NotEqual(InnerVisit(castedNode.Left), InnerVisit(castedNode.Right));
                    break;
                default:
                    expression = null;
                    break;
            }
            return expression;
        }

        private Expression Build(TupleNode node)
        {
            return InnerVisit(node.Expression);
        }
    }
}