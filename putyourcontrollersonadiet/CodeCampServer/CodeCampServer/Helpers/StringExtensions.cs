using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace CodeCampServerLite.Helpers
{
	public static class StringExtensions
	{
		public static string ToSeparatedWords(this string value)
		{
			if (value != null)
				return Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
			return value;
		}
	}
}