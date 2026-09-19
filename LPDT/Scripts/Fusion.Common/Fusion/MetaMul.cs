using System;

namespace Fusion
{
	public static class MetaMul
	{
		internal const int PredefinedCount = 16;

		public const string TypeNameFormat = "Fusion.MetaMul{0}`1";

		internal static Type GetPredefined(int value)
		{
			if (value <= 0 || value > 16)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			return typeof(MetaMul).Assembly.GetType($"Fusion.MetaMul{value}`1", throwOnError: true);
		}

		internal static Type Get(int times, Type t)
		{
			return GetPredefined(times).MakeGenericType(t);
		}
	}
}
