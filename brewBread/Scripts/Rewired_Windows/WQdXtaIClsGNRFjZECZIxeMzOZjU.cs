using System;
using System.Runtime.CompilerServices;

internal struct WQdXtaIClsGNRFjZECZIxeMzOZjU
{
	private int TJTEouakOCJsBFyjboelzrjnjwVFc;

	private long AzLZdDxUZhuWffduPSoIMdsBShvA;

	private static readonly bool pcaCUIfFnGbEZJMlRSwWCANluoWDA;

	public static readonly int ZHEQuImTVvNEkKAXcGUIvuGvaloA;

	static WQdXtaIClsGNRFjZECZIxeMzOZjU()
	{
		pcaCUIfFnGbEZJMlRSwWCANluoWDA = IntPtr.Size == 8;
		ZHEQuImTVvNEkKAXcGUIvuGvaloA = (pcaCUIfFnGbEZJMlRSwWCANluoWDA ? 8 : 4);
	}

	public static WQdXtaIClsGNRFjZECZIxeMzOZjU xkNfVhJUDncENtYwwMvLymmXVmAC(byte[] P_0, int P_1)
	{
		WQdXtaIClsGNRFjZECZIxeMzOZjU result = default(WQdXtaIClsGNRFjZECZIxeMzOZjU);
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			result.AzLZdDxUZhuWffduPSoIMdsBShvA = BitConverter.ToInt64(P_0, P_1);
		}
		else
		{
			result.TJTEouakOCJsBFyjboelzrjnjwVFc = BitConverter.ToInt32(P_0, P_1);
		}
		return result;
	}

	[SpecialName]
	public static int zsNtDbJRpaGeYqGorVKjcaHZirNu(WQdXtaIClsGNRFjZECZIxeMzOZjU P_0)
	{
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			return (int)P_0.AzLZdDxUZhuWffduPSoIMdsBShvA;
		}
		return P_0.TJTEouakOCJsBFyjboelzrjnjwVFc;
	}

	[SpecialName]
	public static long zsNtDbJRpaGeYqGorVKjcaHZirNu(WQdXtaIClsGNRFjZECZIxeMzOZjU P_0)
	{
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			return P_0.AzLZdDxUZhuWffduPSoIMdsBShvA;
		}
		return P_0.TJTEouakOCJsBFyjboelzrjnjwVFc;
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			return AzLZdDxUZhuWffduPSoIMdsBShvA.ToString();
		}
		return TJTEouakOCJsBFyjboelzrjnjwVFc.ToString();
	}
}
