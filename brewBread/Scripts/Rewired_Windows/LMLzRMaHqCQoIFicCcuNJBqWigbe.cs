using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct LMLzRMaHqCQoIFicCcuNJBqWigbe
{
	[FieldOffset(0)]
	private int TJTEouakOCJsBFyjboelzrjnjwVFc;

	[FieldOffset(0)]
	private long AzLZdDxUZhuWffduPSoIMdsBShvA;

	[FieldOffset(0)]
	private IntPtr GzzuDxSrgCzERPyQPpHtOFlKyZbD;

	private static readonly bool pcaCUIfFnGbEZJMlRSwWCANluoWDA;

	public static readonly int ZHEQuImTVvNEkKAXcGUIvuGvaloA;

	static LMLzRMaHqCQoIFicCcuNJBqWigbe()
	{
		ZHEQuImTVvNEkKAXcGUIvuGvaloA = IntPtr.Size;
		pcaCUIfFnGbEZJMlRSwWCANluoWDA = ZHEQuImTVvNEkKAXcGUIvuGvaloA == 8;
	}

	public static LMLzRMaHqCQoIFicCcuNJBqWigbe xkNfVhJUDncENtYwwMvLymmXVmAC(byte[] P_0, int P_1)
	{
		LMLzRMaHqCQoIFicCcuNJBqWigbe result = default(LMLzRMaHqCQoIFicCcuNJBqWigbe);
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			result.AzLZdDxUZhuWffduPSoIMdsBShvA = BitConverter.ToInt64(P_0, P_1);
			result.GzzuDxSrgCzERPyQPpHtOFlKyZbD = new IntPtr(result.AzLZdDxUZhuWffduPSoIMdsBShvA);
		}
		else
		{
			result.TJTEouakOCJsBFyjboelzrjnjwVFc = BitConverter.ToInt32(P_0, P_1);
			result.GzzuDxSrgCzERPyQPpHtOFlKyZbD = new IntPtr(result.TJTEouakOCJsBFyjboelzrjnjwVFc);
		}
		return result;
	}

	[SpecialName]
	public static LMLzRMaHqCQoIFicCcuNJBqWigbe zsNtDbJRpaGeYqGorVKjcaHZirNu(IntPtr P_0)
	{
		LMLzRMaHqCQoIFicCcuNJBqWigbe result = new LMLzRMaHqCQoIFicCcuNJBqWigbe
		{
			GzzuDxSrgCzERPyQPpHtOFlKyZbD = P_0
		};
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			result.AzLZdDxUZhuWffduPSoIMdsBShvA = P_0.ToInt64();
		}
		else
		{
			result.TJTEouakOCJsBFyjboelzrjnjwVFc = P_0.ToInt32();
		}
		return result;
	}

	[SpecialName]
	public static IntPtr zsNtDbJRpaGeYqGorVKjcaHZirNu(LMLzRMaHqCQoIFicCcuNJBqWigbe P_0)
	{
		return P_0.GzzuDxSrgCzERPyQPpHtOFlKyZbD;
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
