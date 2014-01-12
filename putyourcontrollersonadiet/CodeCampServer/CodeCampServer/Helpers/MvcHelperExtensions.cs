using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeCampServerLite.Helpers
{
	public static class MvcHelperExtensions
	{
		public static string GetControllerName(this Type controllerType)
		{
			return controllerType.Name.Replace("Controller", string.Empty);
		}

		public static string GetActionName(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Method.Name;
		}

		public static MemberInfo GetMember(this LambdaExpression actionExpression)
		{
			var body = actionExpression.Body;
			if (body.NodeType == ExpressionType.Convert)
				body = ((UnaryExpression)body).Operand;

			var memberExpr = body as MemberExpression;

			if (memberExpr != null)
				return memberExpr.Member;

			var callExpr = body as MethodCallExpression;

			if (callExpr != null)
				return callExpr.Method;

			return null;
		}

		public static Type GetControllerType(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Object.Type;
		}

		public static string GetAreaName(this Type type)
		{
			string[] namespaces = type.Namespace.ToLowerInvariant().Split('.');
			int areaIndex = GetAreaIndex(namespaces);
			if (areaIndex < 0)
			{
				return null;
			}

			return namespaces[areaIndex + 1];
		}

		private static int GetAreaIndex(string[] namespaces)
		{
			for (int i = 0; i < namespaces.Length; i++)
			{
				if (namespaces[i] == "areas")
				{
					return i;
				}
			}

			return -1;
		}
	}
}