using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomDsl.ExpressionBuilding
{
	public static class CSharpExpressionHelperMethodResolver
	{
		private static readonly IDictionary<string, IGrouping<string, MethodInfo>> HelperMethodTable;
		private static readonly IDictionary<Type, IEnumerable<Type>> ImplicitConversions;

		static CSharpExpressionHelperMethodResolver()
		{
			HelperMethodTable = typeof(CSharpExpressionHelper)
				.GetMethods()
				.GroupBy(x => x.Name)
				.ToDictionary(x => x.Key);

		    ImplicitConversions = new Dictionary<Type, IEnumerable<Type>>
		    {
		        {typeof (string), new[] {typeof (string), typeof (object)}},
		        {typeof (double), new[] {typeof (decimal), typeof (object)}},
		        {typeof (decimal), new[] {typeof (decimal), typeof (object)}},
		        {typeof (long), new[] {typeof (long), typeof (decimal), typeof (object)}},
		        {typeof (int), new[] {typeof (int), typeof (long), typeof (decimal), typeof (object)}},
		        {typeof (bool), new[] {typeof (bool), typeof (object)}},
		        {typeof (DateTime), new[] {typeof (DateTime), typeof (object)}},
		        {
		            typeof (Null),
		            new[]
		            {
		                typeof (object), typeof (string), typeof (decimal), typeof (long), typeof (int), typeof (bool),
		                typeof (DateTime)
		            }
		            },
		    };
		}

		public static MethodInfo GetMethod(string name, params Type[] parameterTypes)
		{
			var possibleParameterTypes = ComputeCartesianProduct(parameterTypes.Select(x => x ?? typeof(Null)).ToArray()).Select(x => x.ToArray());

			IGrouping<string, MethodInfo> methods;
			MethodInfo compatibleMethod = null;

			if (HelperMethodTable.TryGetValue(name, out methods))
			{
				compatibleMethod = (from types in possibleParameterTypes
				                    from method in methods
				                    let parameters = method.GetParameters().Select(x => x.ParameterType).ToArray()
				                    where IsCompatibleSignature(parameters, types)
				                    select method).FirstOrDefault();
			}

			if (compatibleMethod == null)
				throw new ApplicationException("Could not find a compatible operator: " + name + "(" + string.Join(", ", parameterTypes.Select(x => x.Name).ToArray()) + ")");

			return compatibleMethod;
		}

		private static bool IsCompatibleSignature(IList<Type> parameterTypes, IList<Type> types)
		{
			if (parameterTypes.Count != types.Count)
				return false;

			for (var i = 0; i < parameterTypes.Count; ++i)
				if (parameterTypes[i] != types[i] && (!types[i].IsValueType || parameterTypes[i] != typeof(Nullable<>).MakeGenericType(types[i])))
					return false;

			return true;
		}

		private static IEnumerable<IEnumerable<Type>> ComputeCartesianProduct(IList<Type> parameterTypes)
		{
			IEnumerable<IEnumerable<Type>> possibleTypes;

			if (parameterTypes.Count > 0)
			{
				possibleTypes = ImplicitConversions[parameterTypes[0]].Select(x => new[] {x});

				for (var i = 1; i < parameterTypes.Count; ++i)
				{
					var index = i;

					possibleTypes = possibleTypes.SelectMany(x => ImplicitConversions[parameterTypes[index]].Select(y => x.Concat(Enumerable.Repeat(y, 1))));
				}
			}
			else
			{
				possibleTypes = new[] {new Type[0]};
			}

			return possibleTypes;
		}

		private class Null { }
	}
}