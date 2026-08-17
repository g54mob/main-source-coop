namespace PlayEveryWare.Common.Extensions
{
	public static class StringExtensions
	{
		public static ulong ToUlong(this string value, ulong defaultValue = 0uL)
		{
			ulong result = defaultValue;
			if (ulong.TryParse(value, out var result2))
			{
				result = result2;
			}
			return result;
		}
	}
}
