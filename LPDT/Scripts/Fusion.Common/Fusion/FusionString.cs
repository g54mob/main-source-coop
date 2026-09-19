using System;

namespace Fusion
{
	internal static class FusionString
	{
		public static int CompareOrdinal(string a, string b)
		{
			if (a != null && a.Length == 0 && b != null && b.Length == 0)
			{
				return 0;
			}
			return string.CompareOrdinal(a, b);
		}

		public static int Compare(string a, string b, StringComparison comparison)
		{
			if (comparison == StringComparison.Ordinal)
			{
				return CompareOrdinal(a, b);
			}
			return string.Compare(a, b, comparison);
		}
	}
}
