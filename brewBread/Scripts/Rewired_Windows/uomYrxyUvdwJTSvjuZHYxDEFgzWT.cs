using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct uomYrxyUvdwJTSvjuZHYxDEFgzWT
{
	[FieldOffset(0)]
	private uint TJTEouakOCJsBFyjboelzrjnjwVFc;

	[FieldOffset(0)]
	private ulong AzLZdDxUZhuWffduPSoIMdsBShvA;

	[FieldOffset(0)]
	private IntPtr GzzuDxSrgCzERPyQPpHtOFlKyZbD;

	private static readonly bool pcaCUIfFnGbEZJMlRSwWCANluoWDA;

	public static readonly int ZHEQuImTVvNEkKAXcGUIvuGvaloA;

	static uomYrxyUvdwJTSvjuZHYxDEFgzWT()
	{
		ZHEQuImTVvNEkKAXcGUIvuGvaloA = IntPtr.Size;
		pcaCUIfFnGbEZJMlRSwWCANluoWDA = ZHEQuImTVvNEkKAXcGUIvuGvaloA == 8;
	}

	public static uomYrxyUvdwJTSvjuZHYxDEFgzWT xkNfVhJUDncENtYwwMvLymmXVmAC(byte[] P_0, int P_1)
	{
		uomYrxyUvdwJTSvjuZHYxDEFgzWT result = default(uomYrxyUvdwJTSvjuZHYxDEFgzWT);
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			result.AzLZdDxUZhuWffduPSoIMdsBShvA = BitConverter.ToUInt64(P_0, P_1);
			result.GzzuDxSrgCzERPyQPpHtOFlKyZbD = new IntPtr((long)result.AzLZdDxUZhuWffduPSoIMdsBShvA);
		}
		else
		{
			result.TJTEouakOCJsBFyjboelzrjnjwVFc = BitConverter.ToUInt32(P_0, P_1);
			result.GzzuDxSrgCzERPyQPpHtOFlKyZbD = new IntPtr((int)result.TJTEouakOCJsBFyjboelzrjnjwVFc);
		}
		return result;
	}

	[SpecialName]
	public static IntPtr zsNtDbJRpaGeYqGorVKjcaHZirNu(uomYrxyUvdwJTSvjuZHYxDEFgzWT P_0)
	{
		return P_0.GzzuDxSrgCzERPyQPpHtOFlKyZbD;
	}

	[SpecialName]
	public static uomYrxyUvdwJTSvjuZHYxDEFgzWT zsNtDbJRpaGeYqGorVKjcaHZirNu(IntPtr P_0)
	{
		uomYrxyUvdwJTSvjuZHYxDEFgzWT result = new uomYrxyUvdwJTSvjuZHYxDEFgzWT
		{
			GzzuDxSrgCzERPyQPpHtOFlKyZbD = P_0
		};
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			result.AzLZdDxUZhuWffduPSoIMdsBShvA = (ulong)P_0.ToInt64();
		}
		else
		{
			result.TJTEouakOCJsBFyjboelzrjnjwVFc = (uint)P_0.ToInt32();
		}
		return result;
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			return AzLZdDxUZhuWffduPSoIMdsBShvA.ToString();
		}
		return TJTEouakOCJsBFyjboelzrjnjwVFc.ToString();
	}

	public int mhSxqbLWgKfHpBtwQwTaYBFrbdsT()
	{
		if (pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			return (int)AzLZdDxUZhuWffduPSoIMdsBShvA;
		}
		return (int)TJTEouakOCJsBFyjboelzrjnjwVFc;
	}
}
