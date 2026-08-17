using System;

namespace NWH.Common.Utility
{
	public static class ArrayExtensions
	{
		public static void Fill<T>(this T[] destinationArray, params T[] value)
		{
			int num = destinationArray.Length;
			if (num != 0)
			{
				int num2 = value.Length;
				Array.Copy(value, destinationArray, num2);
				int num3 = num / 2;
				int num4;
				for (num4 = num2; num4 < num3; num4 <<= 1)
				{
					Array.Copy(destinationArray, 0, destinationArray, num4, num4);
				}
				Array.Copy(destinationArray, 0, destinationArray, num4, num - num4);
			}
		}
	}
}
