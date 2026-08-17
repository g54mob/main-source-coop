namespace PlayEveryWare.EpicOnlineServices.Utility
{
	public static class SafeTranslatorUtility
	{
		public static bool TryConvert(int value, out uint output)
		{
			output = (uint)value;
			return value >= 0;
		}

		public static bool TryConvert(uint value, out int output)
		{
			output = (int)value;
			return value <= 2147483647;
		}

		public static bool TryConvert(ulong value, out long output)
		{
			output = (long)value;
			return value <= 9223372036854775807L;
		}

		public static bool TryConvert(long value, out ulong output)
		{
			output = (ulong)value;
			return value >= 0;
		}
	}
}
