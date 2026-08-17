using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Rewired.Utils;

[DefaultMember("Item")]
internal class TTdJxMSsCjAYTpeOzOTXLPWCondx : IDisposable
{
	private readonly byte[] hqjaEUASFjOXvvoGsKkZpjKAdZUFb;

	public readonly int oRjNdVSumbQrnPpjzCOLcZisplBNA;

	private GCHandle sPWJXsqtYIuJLbmDLIEpCuOYdAsEA;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public bool DjhBcmIQnJGTrZRpamVIbKHTeNMC => sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.IsAllocated;

	public byte muarktpyJSWZctSRxmEoxarAcZUv
	{
		get
		{
			return hqjaEUASFjOXvvoGsKkZpjKAdZUFb[P_0];
		}
		set
		{
			hqjaEUASFjOXvvoGsKkZpjKAdZUFb[num] = b;
		}
	}

	public TTdJxMSsCjAYTpeOzOTXLPWCondx(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("size must be > 0");
		}
		oRjNdVSumbQrnPpjzCOLcZisplBNA = P_0;
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb = new byte[P_0];
	}

	public IntPtr iEEyFGozuFoprItZJcIBbOpLFVyC()
	{
		if (sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.IsAllocated)
		{
			return sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.AddrOfPinnedObject();
		}
		sPWJXsqtYIuJLbmDLIEpCuOYdAsEA = GCHandle.Alloc(hqjaEUASFjOXvvoGsKkZpjKAdZUFb, GCHandleType.Pinned);
		return sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.AddrOfPinnedObject();
	}

	public void FQTmLgtKoLEQwCOqJzCqrzEOUTBaA()
	{
		if (sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.IsAllocated)
		{
			sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.Free();
		}
	}

	public string ybrizWrFCnjXcDbmkuxAHpTbCMcn()
	{
		string text = "";
		for (int i = 0; i < oRjNdVSumbQrnPpjzCOLcZisplBNA; i++)
		{
			text = text + hqjaEUASFjOXvvoGsKkZpjKAdZUFb[i].ToString("x2") + " ";
		}
		return text;
	}

	public bool sJZlbulSeIynpgEjmrtermPqGItB(int P_0, byte P_1)
	{
		if (1 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("byteIndex");
		}
		if (P_1 >= 8)
		{
			throw new ArgumentOutOfRangeException("bit");
		}
		return (hqjaEUASFjOXvvoGsKkZpjKAdZUFb[P_0] & (1 << (int)P_1)) != 0;
	}

	public byte GwzzmqKUcyhFPCqvgrcRvKXmlpyS(int P_0)
	{
		if (1 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return hqjaEUASFjOXvvoGsKkZpjKAdZUFb[P_0];
	}

	public unsafe short ZVsmEUVDicEHJHFouaPgBQCHtFqZA(int P_0)
	{
		if (2 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			return *(short*)(ptr + P_0);
		}
	}

	public unsafe ushort FmKYczWJMFEgnmBSqDJRgvJLDYJz(int P_0)
	{
		if (2 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			return *(ushort*)(ptr + P_0);
		}
	}

	public unsafe int AIEAPAcQsYfUvwtoIfuaELJRIsdF(int P_0)
	{
		if (4 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			return *(int*)(ptr + P_0);
		}
	}

	public unsafe uint qUacqrYmABYuGmrfJseQCwAFEYJh(int P_0)
	{
		if (4 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			return *(uint*)(ptr + P_0);
		}
	}

	public unsafe long bgjdJVrxSmVfuRAWZbluKQCupRRm(int P_0)
	{
		if (8 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			return *(long*)(ptr + P_0);
		}
	}

	public unsafe ulong juVCQpcZqFIHIRNitiGrFzxueJXnA(int P_0)
	{
		if (8 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			return *(ulong*)(ptr + P_0);
		}
	}

	public void pkpJIXUPRvEEdtemqnOqHyayGnzb(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("bytes");
		}
		int num = P_0.Length;
		if (num <= 0)
		{
			throw new ArgumentOutOfRangeException("bytes.Length must be > 0.");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead must be > 0");
		}
		if (P_1 > num)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead must be <= bufferLength.");
		}
		if (P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead must be <= Length.");
		}
		if (P_3 >= num)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be < bufferLength.");
		}
		if (P_3 < 0)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
		}
		if (P_2 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be < Length.");
		}
		if (P_2 < 0)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
		}
		if (P_3 + P_1 > num)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex + numBytesToRead must be < bufferLength.");
		}
		if (P_1 + P_2 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead + readStartIndex must be < Length.");
		}
		Array.Copy(hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_2, P_0, P_3, P_1);
	}

	public void pkpJIXUPRvEEdtemqnOqHyayGnzb(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == IntPtr.Zero)
		{
			throw new ArgumentNullException("bytes");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferLength must be > 0.");
		}
		if (P_2 <= 0)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead must be > 0");
		}
		if (P_2 > P_1)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead must be <= bufferLength.");
		}
		if (P_2 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead must be <= Length.");
		}
		if (P_4 >= P_1)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be < bufferLength.");
		}
		if (P_4 < 0)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
		}
		if (P_3 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be < Length.");
		}
		if (P_3 < 0)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
		}
		if (P_4 + P_2 > P_1)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex + numBytesToRead must be < bufferLength.");
		}
		if (P_2 + P_3 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead + readStartIndex must be < Length.");
		}
		NativeTools.CopyMemory(hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_0, P_3, P_4, P_2);
	}

	public int mAdXQxWuvrDlLbnADJNQEnsupYsm(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
	{
		if (P_0 == null || P_1 <= 0)
		{
			return 0;
		}
		int num = P_0.Length;
		if (num == 0)
		{
			return 0;
		}
		if (P_2 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			return 0;
		}
		if (P_3 >= num)
		{
			return 0;
		}
		if (P_2 < 0)
		{
			P_2 = 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_2 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			P_1 = oRjNdVSumbQrnPpjzCOLcZisplBNA - P_2;
		}
		if (P_3 + P_1 > num)
		{
			P_1 = num - P_3;
		}
		if (P_1 == 0)
		{
			return 0;
		}
		Array.Copy(hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_2, P_0, P_3, P_1);
		return P_1;
	}

	public int mAdXQxWuvrDlLbnADJNQEnsupYsm(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == IntPtr.Zero || P_2 <= 0)
		{
			return 0;
		}
		if (P_3 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			return 0;
		}
		if (P_4 >= P_1)
		{
			return 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_4 < 0)
		{
			P_4 = 0;
		}
		if (P_3 + P_2 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			P_2 = oRjNdVSumbQrnPpjzCOLcZisplBNA - P_3;
		}
		if (P_4 + P_2 > P_1)
		{
			P_2 = P_1 - P_4;
		}
		NativeTools.CopyMemory(hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_0, P_3, P_4, P_2);
		return P_2;
	}

	public void GkZNmvrFnhcPewsFfMhhVnoBgzAF(int P_0, byte P_1, bool P_2)
	{
		if (1 + P_0 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("byteIndex");
		}
		if (P_1 >= 8)
		{
			throw new ArgumentOutOfRangeException("bit");
		}
		if (P_2)
		{
			hqjaEUASFjOXvvoGsKkZpjKAdZUFb[P_0] |= (byte)(1 << (int)P_1);
		}
		else
		{
			hqjaEUASFjOXvvoGsKkZpjKAdZUFb[P_0] &= (byte)(~(1 << (int)P_1));
		}
	}

	public void KjttXAEzsRtECEzfsLykXNbscFEq(byte P_0, int P_1)
	{
		if (1 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb[P_1] = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(short P_0, int P_1)
	{
		if (2 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			*(short*)(ptr + P_1) = P_0;
		}
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(ushort P_0, int P_1)
	{
		if (2 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			*(ushort*)(ptr + P_1) = P_0;
		}
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(int P_0, int P_1)
	{
		if (4 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			*(int*)(ptr + P_1) = P_0;
		}
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(uint P_0, int P_1)
	{
		if (4 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			*(uint*)(ptr + P_1) = P_0;
		}
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(long P_0, int P_1)
	{
		if (8 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			*(long*)(ptr + P_1) = P_0;
		}
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(ulong P_0, int P_1)
	{
		if (8 + P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
		{
			*(ulong*)(ptr + P_1) = P_0;
		}
	}

	public void KjttXAEzsRtECEzfsLykXNbscFEq(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("bytes");
		}
		int num = P_0.Length;
		if (num <= 0)
		{
			throw new ArgumentOutOfRangeException("bytes.Length must be > 0.");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite must be > 0");
		}
		if (P_1 > num)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite must be <= bufferLength.");
		}
		if (P_1 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite must be <= Length.");
		}
		if (P_3 >= num)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be < bufferLength.");
		}
		if (P_3 < 0)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
		}
		if (P_2 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be < Length.");
		}
		if (P_2 < 0)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
		}
		if (P_3 + P_1 > num)
		{
			throw new ArgumentOutOfRangeException("readStartIndex + numBytesToWrite must be < bufferLength.");
		}
		if (P_1 + P_2 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite + writeStartIndex must be < Length.");
		}
		Array.Copy(P_0, P_3, hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_2, P_1);
	}

	public void KjttXAEzsRtECEzfsLykXNbscFEq(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == IntPtr.Zero)
		{
			throw new ArgumentNullException("bytes");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferLength must be > 0.");
		}
		if (P_2 <= 0)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite must be > 0");
		}
		if (P_2 > P_1)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite must be <= bufferLength.");
		}
		if (P_2 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite must be <= Length.");
		}
		if (P_4 >= P_1)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be < bufferLength.");
		}
		if (P_4 < 0)
		{
			throw new ArgumentOutOfRangeException("readStartIndex must be >= 0.");
		}
		if (P_3 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be < Length.");
		}
		if (P_3 < 0)
		{
			throw new ArgumentOutOfRangeException("writeStartIndex must be >= 0.");
		}
		if (P_4 + P_2 > P_1)
		{
			throw new ArgumentOutOfRangeException("readStartIndex + numBytesToWrite must be < bufferLength.");
		}
		if (P_2 + P_3 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite + writeStartIndex must be < Length.");
		}
		NativeTools.CopyMemory(P_0, hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_4, P_3, P_2);
	}

	public int APcdoAbGdHyHwKEUCXlikgFfSStdA(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
	{
		if (P_0 == null)
		{
			return 0;
		}
		int num = P_0.Length;
		if (num == 0 || P_1 <= 0 || P_3 >= num || P_2 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			return 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_2 < 0)
		{
			P_2 = 0;
		}
		if (P_3 + P_1 > num)
		{
			P_1 = num - P_3;
		}
		if (P_1 + P_2 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			P_1 = oRjNdVSumbQrnPpjzCOLcZisplBNA - P_2;
		}
		Array.Copy(P_0, P_3, hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_2, P_1);
		return P_1;
	}

	public int APcdoAbGdHyHwKEUCXlikgFfSStdA(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == IntPtr.Zero || P_1 <= 0 || P_2 <= 0 || P_4 >= P_1 || P_3 >= oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			return 0;
		}
		if (P_4 < 0)
		{
			P_4 = 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_4 + P_2 > P_1)
		{
			P_2 = P_1 - P_4;
		}
		if (P_2 + P_3 > oRjNdVSumbQrnPpjzCOLcZisplBNA)
		{
			P_2 = oRjNdVSumbQrnPpjzCOLcZisplBNA - P_3;
		}
		NativeTools.CopyMemory(P_0, hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_4, P_3, P_2);
		return P_2;
	}

	public void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		Array.Clear(hqjaEUASFjOXvvoGsKkZpjKAdZUFb, 0, oRjNdVSumbQrnPpjzCOLcZisplBNA);
	}

	public virtual string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		string text = "";
		for (int i = 0; i < oRjNdVSumbQrnPpjzCOLcZisplBNA; i++)
		{
			text = text + this.odusCDdstdDtOyBggtazUFaPCYwF(i).ToString("x2") + " ";
		}
		return text;
	}

	public void Dispose()
	{
		lDxnsjCDTQrmresvWgbliNUVruIc(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
	{
		try
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			if (sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.IsAllocated)
			{
				sPWJXsqtYIuJLbmDLIEpCuOYdAsEA.Free();
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	public static void OMzUIlvLGUEzvhPTLqSeKvddGQke(TTdJxMSsCjAYTpeOzOTXLPWCondx P_0, TTdJxMSsCjAYTpeOzOTXLPWCondx P_1, int P_2)
	{
		Array.Copy(P_0.hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_1.hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_2);
	}

	public static void OMzUIlvLGUEzvhPTLqSeKvddGQke(TTdJxMSsCjAYTpeOzOTXLPWCondx P_0, int P_1, TTdJxMSsCjAYTpeOzOTXLPWCondx P_2, int P_3, int P_4)
	{
		Array.Copy(P_0.hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_1, P_2.hqjaEUASFjOXvvoGsKkZpjKAdZUFb, P_3, P_4);
	}
}
