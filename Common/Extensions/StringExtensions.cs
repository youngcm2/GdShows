using System.Globalization;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringExtensions
	{
		public static string ToCamelCase(this string originalString)
		{
			if (string.IsNullOrEmpty(originalString))
			{
				return originalString;
			}

			if (!char.IsUpper(originalString [0]))
			{
				return originalString;
			}

			var camelCase = char.ToLower(originalString [0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
			if (originalString.Length > 1)
			{
				camelCase += originalString.Substring(1);
			}
			return camelCase;
		}
	}
}
