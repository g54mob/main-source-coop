using System;
using System.Runtime.CompilerServices;

internal struct HFbakYckmPMHFrJDHgVdgDcJSJFQ
{
	private uint TJTEouakOCJsBFyjboelzrjnjwVFc;

	private ulong AzLZdDxUZhuWffduPSoIMdsBShvA;

	private static readonly bool pcaCUIfFnGbEZJMlRSwWCANluoWDA;

	public static readonly int ZHEQuImTVvNEkKAXcGUIvuGvaloA;

	static HFbakYckmPMHFrJDHgVdgDcJSJFQ()
	{
		pcaCUIfFnGbEZJMlRSwWCANluoWDA = IntPtr.Size == 8;
		ZHEQuImTVvNEkKAXcGUIvuGvaloA = (pcaCUIfFnGbEZJMlRSwWCANluoWDA ? 8 : 4);
	}

	public static HFbakYckmPMHFrJDHgVdgDcJSJFQ xkNfVhJUDncENtYwwMvLymmXVmAC(byte[] P_0, int P_1)
	{
		HFbakYckmPMHFrJDHgVdgDcJSJFQ result = default(HFbakYckmPMHFrJDHgVdgDcJSJFQ);
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			result.AzLZdDxUZhuWffduPSoIMdsBShvA = BitConverter.ToUInt64(P_0, P_1);
		}
		else
		{
			result.TJTEouakOCJsBFyjboelzrjnjwVFc = BitConverter.ToUInt32(P_0, P_1);
		}
		return result;
	}

	[SpecialName]
	public static uint zsNtDbJRpaGeYqGorVKjcaHZirNu(HFbakYckmPMHFrJDHgVdgDcJSJFQ P_0)
	{
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			return (uint)P_0.AzLZdDxUZhuWffduPSoIMdsBShvA;
		}
		return P_0.TJTEouakOCJsBFyjboelzrjnjwVFc;
	}

	[SpecialName]
	public static ulong zsNtDbJRpaGeYqGorVKjcaHZirNu(HFbakYckmPMHFrJDHgVdgDcJSJFQ P_0)
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
