using System;

internal static class FwDgckZMFTcSVcrWDjGBiayeyZyTA
{
	public static readonly int[] PnwNQVdyQmLjmDwXHINVBlfoishh = new int[72]
	{
		3, 7, 11, 17, 23, 29, 37, 47, 59, 71,
		89, 107, 131, 163, 197, 239, 293, 353, 431, 521,
		631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371,
		4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023,
		25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363,
		156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403,
		968897, 1162687, 1395263, 1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559,
		5999471, 7199369
	};

	public const int OEnDvMXQtqQvcaxpsEgDZEIHASxn = 101;

	public const int jMfHnZXwmrdcRhwslfftNlrkCInU = 2146435069;

	public static bool AsJCvxBLSryiIrGiKfJILayXgpPGb(int P_0)
	{
		if ((P_0 & 1) != 0)
		{
			int num = (int)Math.Sqrt(P_0);
			for (int i = 3; i <= num; i += 2)
			{
				if (P_0 % i == 0)
				{
					return false;
				}
			}
			return true;
		}
		return P_0 == 2;
	}

	public static int YysFyNpCxhfwuaLNxoQFhwWzMyGt(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentException("Arg_HTCapacityOverflow");
		}
		for (int i = 0; i < PnwNQVdyQmLjmDwXHINVBlfoishh.Length; i++)
		{
			int num = PnwNQVdyQmLjmDwXHINVBlfoishh[i];
			if (num >= P_0)
			{
				return num;
			}
		}
		for (int j = P_0 | 1; j < 2147483647; j += 2)
		{
			if (AsJCvxBLSryiIrGiKfJILayXgpPGb(j) && (j - 1) % 101 != 0)
			{
				return j;
			}
		}
		return P_0;
	}

	public static int frhsfBNPjQQTHrsvuaCrAkbnLSUb()
	{
		return PnwNQVdyQmLjmDwXHINVBlfoishh[0];
	}

	public static int iBDFGvcHTeHUtZgDEIwsPMRLLNShb(int P_0)
	{
		int num = 2 * P_0;
		if ((uint)num > 2146435069u && 2146435069 > P_0)
		{
			return 2146435069;
		}
		return YysFyNpCxhfwuaLNxoQFhwWzMyGt(num);
	}
}
