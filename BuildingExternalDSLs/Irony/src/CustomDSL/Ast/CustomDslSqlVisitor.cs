using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter.Ast;

namespace CustomDsl.Ast
{

    public class CustomDslSqlVisitor : ICustomDslVisitor
    {
        StringBuilder _sb = new StringBuilder();
        private Dictionary<Type, Action<AstNode>> dispatchTable;
        public CustomDslSqlVisitor()
        {
            dispatchTable = new Dictionary<Type, Action<AstNode>>()
                                {
                                    {typeof(BinaryExpressionNode),node => VisitBinaryExpressionNode(node)},
                                    {typeof(IdNode),node => VisitIdNode(node)},
                                    {typeof(LiteralNode),node => VisitLiteralNode(node)},
                                    {typeof(NumberNode),node => VisitNumberNode(node)},
                                    {typeof(StringNode),node => VisitStringNode(node)},
                                    {typeof(TupleNode),node => VisitTupleNode(node)},
                                    {typeof(UnaryExpressionNode),node => VisitUnaryExpressionNode(node)},
                                    {typeof(NullCoalescingNode),node => VisitNullCoalescingNode(node)},
                                    {typeof(BooleanNode),node => VisitBooleanNode(node)},
                                    {typeof(DateNode),node => VisitDateNode(node)},
                                    {typeof(NilNode),node => VisitNilNode(node)},
                                    {typeof(FunctionNode),node => VisitFunctionNode(node)},
                                    {typeof(IifFunctionNode),node => VisitIffFunctionNode(node)},
                                    {typeof(IsNullFunctionNode),node => VisitIsNullFunctionNode(node)},
                                    {typeof(BetweenFunctionNode),node => VisitBetweenFunctionNode(node)},
                                    {typeof(RoundFunctionNode),node => VisitFunctionNode(node)},
                                    {typeof(ConcatFunctionNode),node => VisitConcatFunctionNode(node)},
                                    {typeof(TrimStartFunctionNode),node => VisitTrimStartFunctionNode(node)},
                                    {typeof(TrimEndFunctionNode),node => VisitTrimEndFunctionNode(node)},
                                    {typeof(TrimFunctionNode),node => VisitTrimFunctionNode(node)},
                                    {typeof(SystemFieldNode),node => VisitSystemFieldNode(node)},
                                    {typeof(SystemFunctionNode),node =>{throw new NotSupportedException("System Functions are not supported to generate SQL");}},
                                };
        }
        public void Visit(AstNode rootNode)
        {
            Action<AstNode> action;
            dispatchTable.TryGetValue(rootNode.GetType(), out action);

            if (action != null)
                action(rootNode);
        }

        public string GetSqlExpression()
        {
            return _sb.ToString();
        }

        public void VisitBinaryExpressionNode(AstNode node)
        {
            var castedNode = (BinaryExpressionNode)node;

            if(castedNode.Op == "NOT")
            {
                _sb.Append(" NOT (");
                Visit(castedNode.Left);
                _sb.Append(")");
            }
            else
            {
                VisitRegularBinaryExpressionNode(castedNode);    
            }
        }

        private void VisitRegularBinaryExpressionNode(BinaryExpressionNode castedNode)
        {
            Visit(castedNode.Left);

            var rightLeafIsNullNode = castedNode.Right.GetType().IsAssignableFrom(typeof(NilNode));

            if (rightLeafIsNullNode && castedNode.Op == "=")
            {
                _sb.Append("IS");
            }
            else if (rightLeafIsNullNode && castedNode.Op == "<>")
            {
                _sb.Append("IS NOT");
            }
            else
            {
                _sb.Append(castedNode.Op);
            }

            Visit(castedNode.Right);
        }

        public void VisitIdNode(AstNode node)
        {
            var value = string.Format(" {0} ", ((IdNode)node).Symbol);
            _sb.Append(value);
        }

        public void VisitLiteralNode(AstNode node)
        {
            var value = string.Format(" {0} ", ((LiteralNode)node).Value);
            _sb.Append(value);
        }

        public void VisitNumberNode(AstNode node)
        {
            var value = string.Format(" {0} ", ((NumberNode)node).Value);
            _sb.Append(value);
        }

        public void VisitStringNode(AstNode node)
        {
            var value = string.Format(" '{0}' ", ((StringNode)node).Value);
            _sb.Append(value);
        }

        public void VisitTupleNode(AstNode node)
        {
            _sb.Append("(");
            Visit(((TupleNode)node).Expression);
            _sb.Append(")");
        }

        public void VisitUnaryExpressionNode(AstNode node)
        {
            var castedNode = (UnaryExpressionNode)node;
            _sb.Append("(");
            var op = castedNode.Op;
            _sb.Append(op);
            Visit(castedNode.Argument);
            _sb.Append(")");
        }

        public void VisitNullCoalescingNode(AstNode node)
        {
            var castedNode = ((NullCoalescingNode)node);
            _sb.Append("( CASE WHEN ");
            Visit(castedNode.Value);
            _sb.Append(" IS NULL THEN ");
            Visit(castedNode.DefaultValue);
            _sb.Append(" ELSE ");
            Visit(castedNode.Value);
            _sb.Append(" END AS ");
            Visit(castedNode.Value);
            _sb.Append(")");
        }

        public void VisitBooleanNode(AstNode node)
        {
            var value = string.Format(" CASE '{0}' WHEN 'true' THEN 1 ELSE 0 END ", ((BooleanNode)node).Value);
            _sb.Append(value);
        }

        public void VisitDateNode(AstNode node)
        {
            var value = string.Format(" CONVERT(DATETIME,'{0}',101) ", node);
            _sb.Append(value);
        }

        public void VisitNilNode(AstNode node)
        {
            _sb.Append(" NULL ");
        }

        public void VisitFunctionNode(AstNode node)
        {
            var functionNode = ((FunctionNode)node);
            _sb.Append(functionNode.FunctionName);
            _sb.Append("(");

            var args = functionNode.Arguments.ToArray();
            for (int i = 0; i < args.Length; i++)
            {
                if (i != 0) { _sb.Append(","); }
                Visit(args[i]);
            }
            _sb.Append(")");
        }

        public void VisitIffFunctionNode(AstNode node)
        {
            var functionNode = ((IifFunctionNode)node);
            var args = functionNode.Arguments.ToArray();
            _sb.Append("( CASE WHEN (");
            Visit(args[0]);
            _sb.Append(") THEN (");
            Visit(args[1]);
            _sb.Append(") ELSE (");
            Visit(args[2]);
            _sb.Append(") END )");
        }

        public void VisitIsNullFunctionNode(AstNode node)
        {
            var functionNode = ((IsNullFunctionNode)node);
            
            _sb.Append("( ");
            Visit(functionNode.Arguments.First());
            _sb.Append(" IS NULL )");
        }

        public void VisitBetweenFunctionNode(AstNode node)
        {
            var functionNode = ((BetweenFunctionNode)node);
            var args = functionNode.Arguments.ToArray();
            _sb.Append("(");
            Visit(args[0]);
            _sb.Append("BETWEEN");
            Visit(args[1]);
            _sb.Append("AND");
            Visit(args[2]);
            _sb.Append(")");
        }

        public void VisitConcatFunctionNode(AstNode node)
        {
            var functionNode = ((ConcatFunctionNode) node);
            var args = functionNode.Arguments.ToArray();
            _sb.Append("(");
            Visit(args[0]);
            _sb.Append("+");
            Visit(args[1]);
            _sb.Append(")");
        }

        public void VisitTrimStartFunctionNode(AstNode node)
        {
            var functionNode = ((TrimStartFunctionNode)node);
            var args = functionNode.Arguments.ToArray();
            _sb.Append("LTRIM(");
            Visit(args[0]);
            _sb.Append(")");
        }

        public void VisitTrimEndFunctionNode(AstNode node)
        {
            var functionNode = ((TrimEndFunctionNode)node);
            var args = functionNode.Arguments.ToArray();
            _sb.Append("RTRIM(");
            Visit(args[0]);
            _sb.Append(")");
        }

        public void VisitTrimFunctionNode(AstNode node)
        {
            var functionNode = ((TrimFunctionNode)node);
            var args = functionNode.Arguments.ToArray();
            _sb.Append("RTRIM(LTRIM(");
            Visit(args[0]);
            _sb.Append("))");
        }

        public void VisitSystemFieldNode(AstNode node)
        {
            var value = string.Format(" {0} ", ((SystemFieldNode)node).Name);
            _sb.Append(value);
        }
    }
}