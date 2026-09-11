namespace Zorro.Core
{
	public static class StringExtensions
	{
		public static string WithoutWhitespace(this string s)
		{
			return s.Replace(" ", "");
		}
	}
}
