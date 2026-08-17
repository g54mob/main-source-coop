using System;

namespace QFSW.QC.Utilities
{
	public static class StringExtensions
	{
		public static bool ContainsCaseInsensitive(this string source, string value)
		{
			if (!string.IsNullOrEmpty(source))
			{
				return source.Contains(value, StringComparison.OrdinalIgnoreCase);
			}
			return string.IsNullOrEmpty(value);
		}

		public static bool Contains(this string source, string value, StringComparison comp)
		{
			if (source == null)
			{
				return false;
			}
			return source.IndexOf(value, comp) >= 0;
		}

		public static int CountFromIndex(this string source, char target, int index)
		{
			int num = 0;
			for (int i = index; i < source.Length; i++)
			{
				if (source[i] == target)
				{
					num++;
				}
			}
			return num;
		}
	}
}
