using System;

namespace Fusion
{
	public static class MetaConstant
	{
		public const int MaxValue = 65536;

		internal const int PredefinedCount = 256;

		public const string TypeNameFormat = "Fusion.MetaConstant{0}";

		internal static Type GetPredefined(int value)
		{
			if (value <= 0 || value > 256)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			return typeof(MetaConstant).Assembly.GetType($"Fusion.MetaConstant{value}", throwOnError: true);
		}

		public static Type Get(int size)
		{
			if (size < 0 || size >= 65536)
			{
				throw new ArgumentOutOfRangeException("size", "size must be greater than 0");
			}
			if (size <= 256)
			{
				return GetPredefined(size);
			}
			int num = ((size > 4096) ? 4096 : 256);
			int num2 = size / num;
			int num3 = size % num;
			Type type = Get(num);
			if (num2 == 1)
			{
				return (num3 == 0) ? type : MetaAdd.Get(type, Get(num3));
			}
			Type type2 = MetaMul.Get(num2, type);
			return (num3 == 0) ? type2 : MetaAdd.Get(type2, Get(num3));
		}
	}
}
