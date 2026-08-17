using System;

namespace EvilCore.Extensions
{
	public static class ComparableExtensions
	{
		public static bool IsBetween<T>(this T value, T a, T b, bool aInclusive = true, bool bInclusive = true) where T : IComparable
		{
			if (a.CompareTo(b) == 1)
			{
				T val = b;
				T val2 = a;
				a = val;
				b = val2;
				bool num = bInclusive;
				bool flag = aInclusive;
				aInclusive = num;
				bInclusive = flag;
			}
			bool num2;
			if (!aInclusive)
			{
				object obj = a;
				num2 = value.CompareTo(obj) == 1;
			}
			else
			{
				object obj2 = a;
				num2 = value.CompareTo(obj2).EqualsToAny(0, 1);
			}
			if (num2)
			{
				if (!bInclusive)
				{
					return value.CompareTo(b) == -1;
				}
				return value.CompareTo(b).EqualsToAny(-1, 0);
			}
			return false;
		}
	}
}
