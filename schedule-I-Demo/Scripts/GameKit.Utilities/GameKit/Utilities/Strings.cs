using System;

namespace GameKit.Utilities
{
	public static class Strings
	{
		public static string ReturnModifySuffix(string text, string suffix, bool addExtension)
		{
			if (text.Length > suffix.Length + 1)
			{
				if (addExtension)
				{
					if (!text.Substring(text.Length - suffix.Length).Contains(suffix, StringComparison.CurrentCultureIgnoreCase))
					{
						return text + suffix;
					}
					return text;
				}
				if (text.Substring(text.Length - suffix.Length).Contains(suffix, StringComparison.CurrentCultureIgnoreCase))
				{
					return text.Substring(0, text.Length - suffix.Length);
				}
				return text;
			}
			return text;
		}

		public static bool Contains(this string s, string contains, StringComparison comp)
		{
			return s.IndexOf(contains, comp) >= 0;
		}
	}
}
