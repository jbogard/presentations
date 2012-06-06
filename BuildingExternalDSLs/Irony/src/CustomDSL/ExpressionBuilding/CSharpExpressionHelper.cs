using System;

namespace CustomDsl.ExpressionBuilding
{
	public static class CSharpExpressionHelper
	{
		public static string Left(string value, int length)
		{
			return value.Substring(0, length);
		}

        public static string Right(string value, int length)
        {
            var startIndex = value.Length - length;

            return value.Substring(startIndex, length);
        }

        public static string Trim(string value)
        {
            return value.Trim();
        }

        public static string TrimStart(string value)
        {
            return value.TrimStart();
        }

        public static string TrimEnd(string value)
        {
            return value.TrimEnd();
        }

        public static string Replace(string value, string pattern, string replacement)
        {
            return value.Replace(pattern, replacement);
        }

		public static string ToString(decimal value)
		{
			return value.ToString();
		}

		public static string ToString(decimal value, int length)
		{
			return value.ToString().PadLeft(length);
		}

		public static string ToString(decimal value, int length, int decimals)
		{
			return value.ToString("F" + decimals).PadLeft(length);
		}

        public static string Add(string left, string right)
        {
        	return left + right;
        }

		public static string Add(object left, string right)
		{
			return left + right;
		}

		public static string Add(string left, object right)
		{
			return left + right;
		}

        public static decimal Add(decimal left, decimal right)
        {
        	return left + right;
        }

        public static int Add(int left, int right)
        {
        	return left + right;
		}

        public static long Add(long left, long right)
        {
        	return left + right;
		}

        public static DateTime Add(DateTime left, int right)
        {
            return left.AddDays(right);
        }

        public static DateTime Add(DateTime left, long right)
        {
            return left.AddDays(right);
        }

        public static decimal Subtract(decimal left, decimal right)
		{
			return left - right;
		}

		public static int Subtract(int left, int right)
		{
			return left - right;
		}

		public static long Subtract(long left, long right)
		{
			return left - right;
		}

        public static DateTime Subtract(DateTime left, int right)
        {
            return left.AddDays(right*-1);
        }

        public static DateTime Subtract(DateTime left, long right)
        {
            return left.AddDays(right*-1);
        }

		public static decimal Multiply(decimal left, decimal right)
		{
			return left*right;
		}

		public static int Multiply(int left, int right)
		{
			return left*right;
		}

		public static long Multiply(long left, long right)
		{
			return left*right;
		}

		public static decimal Divide(decimal left, decimal right)
		{
			return left/right;
		}

		public static decimal Divide(int left, int right)
		{
			return left/right;
		}

		public static decimal Divide(long left, long right)
		{
			return left/right;
		}

		public static decimal Modulus(decimal left, decimal right)
		{
			return left%right;
		}

		public static int Modulus(int left, int right)
		{
			return left%right;
		}

		public static long Modulus(long left, long right)
		{
			return left%right;
		}

		public static decimal Negate(decimal value)
		{
			return -value;
		}

		public static int Negate(int value)
		{
			return -value;
		}

		public static long Negate(long value)
		{
			return -value;
		}

        public static bool AndAlso(bool left, bool right)
        {
            return left && right;
        }

        public static bool OrElse(bool left, bool right)
        {
            return left || right;
		}

		public static bool Equal(string left, string right)
		{
			return left == right;
		}

		public static bool Equal(decimal left, decimal right)
		{
			return left == right;
		}

		public static bool Equal(int left, int right)
		{
			return left == right;
		}

		public static bool Equal(long left, long right)
		{
			return left == right;
		}

		public static bool Equal(DateTime left, DateTime right)
		{
			return left == right;
		}

		public static bool NotEqual(string left, string right)
		{
			return left != right;
		}

		public static bool NotEqual(decimal left, decimal right)
		{
			return left != right;
		}

		public static bool NotEqual(int left, int right)
		{
			return left != right;
		}

		public static bool NotEqual(long left, long right)
		{
			return left != right;
		}

		public static bool NotEqual(DateTime left, DateTime right)
		{
			return left != right;
		}

		public static bool GreaterThan(string left, string right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool GreaterThan(decimal left, decimal right)
		{
			return left > right;
		}

		public static bool GreaterThan(int left, int right)
		{
			return left > right;
		}

		public static bool GreaterThan(long left, long right)
		{
			return left > right;
		}

		public static bool GreaterThan(DateTime left, DateTime right)
		{
			return left > right;
		}

		public static bool GreaterThanOrEqual(string left, string right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool GreaterThanOrEqual(decimal left, decimal right)
		{
			return left >= right;
		}

		public static bool GreaterThanOrEqual(int left, int right)
		{
			return left >= right;
		}

		public static bool GreaterThanOrEqual(long left, long right)
		{
			return left >= right;
		}

		public static bool GreaterThanOrEqual(DateTime left, DateTime right)
		{
			return left >= right;
		}

		public static bool LessThan(string left, string right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool LessThan(decimal left, decimal right)
		{
			return left < right;
		}

		public static bool LessThan(int left, int right)
		{
			return left < right;
		}

		public static bool LessThan(long left, long right)
		{
			return left < right;
		}

		public static bool LessThan(DateTime left, DateTime right)
		{
			return left < right;
		}

		public static bool LessThanOrEqual(string left, string right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool LessThanOrEqual(decimal left, decimal right)
		{
			return left <= right;
		}

		public static bool LessThanOrEqual(int left, int right)
		{
			return left <= right;
		}

		public static bool LessThanOrEqual(long left, long right)
		{
			return left <= right;
		}

		public static bool LessThanOrEqual(DateTime left, DateTime right)
		{
			return left <= right;
		}

		public static bool Between(string value, string left, string right)
		{
			return (value.CompareTo(left) >= 0 && value.CompareTo(right) <= 0) || (value.CompareTo(left) <= 0 && value.CompareTo(right) >= 0);
		}

		public static bool Between(decimal value, decimal left, decimal right)
		{
			return (value >= left && value <= right) || (value <= left && value >= right);
		}

		public static bool Between(int value, int left, int right)
		{
			return (value >= left && value <= right) || (value <= left && value >= right);
		}

		public static bool Between(long value, long left, long right)
		{
			return (value >= left && value <= right) || (value <= left && value >= right);
		}

		public static bool Between(DateTime value, DateTime left, DateTime right)
		{
			return (value >= left && value <= right) || (value <= left && value >= right);
		}

		public static decimal Abs(decimal value)
		{
			return Math.Abs(value);
		}

		public static int Abs(int value)
		{
			return Math.Abs(value);
		}

		public static long Abs(long value)
		{
			return Math.Abs(value);
		}

        public static decimal Ceiling(decimal value)
        {
            return Math.Ceiling(value);
        }

        public static decimal Floor(decimal value)
        {
            return Math.Floor(value);
        }

        public static decimal Pow(decimal value, decimal power)
        {
            var result = Math.Pow((double) value, (double) power);

            return (decimal) result;
		}

		public static decimal Min(decimal left, decimal right)
		{
			return Math.Min(left, right);
		}

		public static int Min(int left, int right)
		{
			return Math.Min(left, right);
		}

		public static long Min(long left, long right)
		{
			return Math.Min(left, right);
		}

		public static decimal Max(decimal left, decimal right)
		{
			return Math.Max(left, right);
		}

		public static int Max(int left, int right)
		{
			return Math.Max(left, right);
		}

		public static long Max(long left, long right)
		{
			return Math.Max(left, right);
		}

        public static decimal Round(decimal value, int decimals)
        {
			//This is to ensure a behavior equivalent to T-SQL Round Function
            // http://msdn.microsoft.com/en-us/library/ms175003.aspx))
            if (decimals >= 0)
                return Math.Round(value, decimals);

            var factor = (decimal) Math.Pow(10, decimals);

        	return Math.Round(value*factor)/factor;
        }

        public static decimal Truncate(decimal value, int decimalPlaces)
        {
            var factor = (decimal) Math.Pow(10, decimalPlaces);

            return Math.Truncate(value*factor)/factor;
        }

        public static bool Not(bool value)
        {
            return !value;
        }

        public static string Iif(bool condition, string left, string right)
        {
            return condition ? left : right;
        }

        public static decimal? Iif(bool condition, decimal? left, decimal? right)
        {
            return condition ? left : right;
        }

        public static int? Iif(bool condition, int? left, int? right)
        {
            return condition ? left : right;
        }

        public static long? Iif(bool condition, long? left, long? right)
        {
            return condition ? left : right;
        }

        public static bool? Iif(bool condition, bool? left, bool? right)
        {
            return condition ? left : right;
        }

        public static DateTime? Iif(bool condition, DateTime? left, DateTime? right)
        {
            return condition ? left : right;
        }

        public static string Substring(string value, int startIndex, int length)
        {
            return value.Substring(startIndex, length);
        }

        public static string Concat(string string1, string string2)
        {
            return String.Concat(string1, string2);
        }

        public static bool Contains(string string1, string string2)
        {
            return string1.Contains(string2);
        }

        public static int Length(string value)
        {
            return value.Length;
        }

        public static string Nvl(string value, string ifNull)
        {
            return value ?? ifNull;
        }

		public static decimal Nvl(decimal? value, decimal ifNull)
        {
            return value ?? ifNull;
        }

		public static int Nvl(int? value, int ifNull)
        {
            return value ?? ifNull;
        }

		public static long Nvl(long? value, long ifNull)
        {
            return value ?? ifNull;
        }

		public static bool Nvl(bool? value, bool ifNull)
        {
            return value ?? ifNull;
        }

		public static DateTime Nvl(DateTime? value, DateTime ifNull)
        {
            return value ?? ifNull;
        }

        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null)
                return null;

            Type safeType = Nullable.GetUnderlyingType(conversionType) ?? conversionType;

            return Convert.ChangeType(value, safeType);
        }
    }
}
