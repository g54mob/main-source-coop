using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct PfnQbhAAztkGebiJJBwStuolfJCF
{
	[FieldOffset(0)]
	private uint GphzTTmsyDDpqYVDeLxbbRgNgApr;

	[FieldOffset(0)]
	private ulong NSWuVRjmpsfFZkPPhKajeOIgmXrjB;

	[FieldOffset(0)]
	private IntPtr yMvdxEmXBjkerjTCXChJiYkspkXj;

	private static readonly bool OyQNRKMJdNgSffqdNLQWDPSWErBM;

	public static readonly int zIGwtSxpVwmfhNQkdgSwKTtVCvbu;

	static PfnQbhAAztkGebiJJBwStuolfJCF()
	{
		zIGwtSxpVwmfhNQkdgSwKTtVCvbu = IntPtr.Size;
		OyQNRKMJdNgSffqdNLQWDPSWErBM = zIGwtSxpVwmfhNQkdgSwKTtVCvbu == 8;
	}

	public static PfnQbhAAztkGebiJJBwStuolfJCF KvqDchPNtDgBpkKtRcTSZfrrhCdE(byte[] P_0, int P_1)
	{
		PfnQbhAAztkGebiJJBwStuolfJCF result = default(PfnQbhAAztkGebiJJBwStuolfJCF);
		if (OyQNRKMJdNgSffqdNLQWDPSWErBM)
		{
			result.NSWuVRjmpsfFZkPPhKajeOIgmXrjB = BitConverter.ToUInt64(P_0, P_1);
			result.yMvdxEmXBjkerjTCXChJiYkspkXj = new IntPtr((long)result.NSWuVRjmpsfFZkPPhKajeOIgmXrjB);
		}
		else
		{
			result.GphzTTmsyDDpqYVDeLxbbRgNgApr = BitConverter.ToUInt32(P_0, P_1);
			result.yMvdxEmXBjkerjTCXChJiYkspkXj = new IntPtr((int)result.GphzTTmsyDDpqYVDeLxbbRgNgApr);
		}
		return result;
	}

	[SpecialName]
	public static IntPtr vAfzFBfderhYbpTpLcGHnVNjHpdP(PfnQbhAAztkGebiJJBwStuolfJCF P_0)
	{
		return P_0.yMvdxEmXBjkerjTCXChJiYkspkXj;
	}

	[SpecialName]
	public static PfnQbhAAztkGebiJJBwStuolfJCF WpWHfjulCUuweammBUmNUoEZbQoH(IntPtr P_0)
	{
		PfnQbhAAztkGebiJJBwStuolfJCF result = new PfnQbhAAztkGebiJJBwStuolfJCF
		{
			yMvdxEmXBjkerjTCXChJiYkspkXj = P_0
		};
		if (OyQNRKMJdNgSffqdNLQWDPSWErBM)
		{
			result.NSWuVRjmpsfFZkPPhKajeOIgmXrjB = (ulong)P_0.ToInt64();
		}
		else
		{
			result.GphzTTmsyDDpqYVDeLxbbRgNgApr = (uint)P_0.ToInt32();
		}
		return result;
	}

	public string IHoMJzzrBstdUyiuBLDHYFdTgPRE()
	{
		if (OyQNRKMJdNgSffqdNLQWDPSWErBM)
		{
			return NSWuVRjmpsfFZkPPhKajeOIgmXrjB.ToString();
		}
		return GphzTTmsyDDpqYVDeLxbbRgNgApr.ToString();
	}

	public int CEvgadGvxvYSMaYrYtdcrsqgKzTLA()
	{
		if (OyQNRKMJdNgSffqdNLQWDPSWErBM)
		{
			return (int)NSWuVRjmpsfFZkPPhKajeOIgmXrjB;
		}
		return (int)GphzTTmsyDDpqYVDeLxbbRgNgApr;
	}
}
