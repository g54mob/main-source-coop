using UnityEngine;

namespace Ami.Extension
{
	public static class FlagsExtension
	{
		public enum FlagsRangeType
		{
			Included = 0,
			Excluded = 1
		}

		public static bool ContainsFlag(this int flags, int targetFlag)
		{
			return (flags & targetFlag) != 0;
		}

		public static void AddFlag(ref int flags, int add)
		{
			flags |= add;
		}

		public static void RemoveFlag(ref int flags, int remove)
		{
			flags &= ~remove;
		}

		public static void OverwriteFlag(ref int flags, int targetFlag, int overwriteValue)
		{
			flags = (flags & ~targetFlag) | overwriteValue;
		}

		public static int GetFlagsOnCount(int flags)
		{
			int num = 0;
			while (flags != 0)
			{
				flags &= flags - 1;
				num++;
				if (num > 32)
				{
					Debug.LogError("count flags is failed");
					break;
				}
			}
			return num;
		}

		public static int GetFlagsRange(int minIndex, int maxIndex, FlagsRangeType rangeType)
		{
			int num = 0;
			for (int i = minIndex; i <= maxIndex; i++)
			{
				num += 1 << i;
			}
			return rangeType switch
			{
				FlagsRangeType.Included => num, 
				FlagsRangeType.Excluded => ~num, 
				_ => 0, 
			};
		}

		public static int GetFirstFlag(int flags)
		{
			if (flags <= 0)
			{
				return flags;
			}
			int num = 1;
			while ((flags & num) == 0)
			{
				num <<= 1;
			}
			return num;
		}
	}
}
