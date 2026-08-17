using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils;

[DefaultMember("Item")]
internal class yfacCQRxzUzvTKAYYfCLqujfuVRt : IDisposable
{
	private unsafe byte* UwuHDIqixLebpujhcqnVmliUjFRjA;

	private int yzHmKrgfGycYJStOocuNHTLVBjpo;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public unsafe byte* fnWHBwPbDOywAQmHRedrtIMAlkKC => UwuHDIqixLebpujhcqnVmliUjFRjA;

	public unsafe IntPtr qhEAlcCfypKkEIlVORqfPYQDtvaj => (IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA;

	public int oRjNdVSumbQrnPpjzCOLcZisplBNA => yzHmKrgfGycYJStOocuNHTLVBjpo;

	public unsafe byte muarktpyJSWZctSRxmEoxarAcZUv
	{
		get
		{
			if (P_0 < 0 || P_0 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
			{
				throw new IndexOutOfRangeException();
			}
			return UwuHDIqixLebpujhcqnVmliUjFRjA[P_0];
		}
		set
		{
			if (num < 0 || num >= yzHmKrgfGycYJStOocuNHTLVBjpo)
			{
				throw new IndexOutOfRangeException();
			}
			UwuHDIqixLebpujhcqnVmliUjFRjA[num] = b;
		}
	}

	public yfacCQRxzUzvTKAYYfCLqujfuVRt(int P_0)
	{
		wZhBNjzUhcxMVUfUPRlDRkEipJDq(P_0);
	}

	public unsafe IntPtr BsAAwuYuChqUlaCpaXMrtHWPukNX(int P_0 = 0)
	{
		if (P_0 == 0)
		{
			return (IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA;
		}
		if (P_0 < 0 || P_0 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		return (IntPtr)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe string ybrizWrFCnjXcDbmkuxAHpTbCMcn()
	{
		string text = "";
		for (int i = 0; i < yzHmKrgfGycYJStOocuNHTLVBjpo; i++)
		{
			text = text + UwuHDIqixLebpujhcqnVmliUjFRjA[i].ToString("x2") + " ";
		}
		return text;
	}

	public unsafe bool sJZlbulSeIynpgEjmrtermPqGItB(int P_0, byte P_1)
	{
		if (1 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("byteIndex");
		}
		if (P_1 >= 8)
		{
			throw new ArgumentOutOfRangeException("bit");
		}
		return (UwuHDIqixLebpujhcqnVmliUjFRjA[P_0] & (1 << (int)P_1)) != 0;
	}

	public unsafe byte GwzzmqKUcyhFPCqvgrcRvKXmlpyS(int P_0)
	{
		if (1 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return UwuHDIqixLebpujhcqnVmliUjFRjA[P_0];
	}

	public unsafe short ZVsmEUVDicEHJHFouaPgBQCHtFqZA(int P_0)
	{
		if (2 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return *(short*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe ushort FmKYczWJMFEgnmBSqDJRgvJLDYJz(int P_0)
	{
		if (2 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return *(ushort*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe int AIEAPAcQsYfUvwtoIfuaELJRIsdF(int P_0)
	{
		if (4 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return *(int*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe uint qUacqrYmABYuGmrfJseQCwAFEYJh(int P_0)
	{
		if (4 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return *(uint*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe long bgjdJVrxSmVfuRAWZbluKQCupRRm(int P_0)
	{
		if (8 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return *(long*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe ulong juVCQpcZqFIHIRNitiGrFzxueJXnA(int P_0)
	{
		if (8 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return *(ulong*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_0);
	}

	public unsafe void pkpJIXUPRvEEdtemqnOqHyayGnzb(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
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
		if (P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_2 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_1 + P_2 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead + readStartIndex must be < Length.");
		}
		NativeTools.CopyMemory((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_2, P_3, P_1);
	}

	public unsafe void pkpJIXUPRvEEdtemqnOqHyayGnzb(byte* P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == null)
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
		if (P_2 > yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_3 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_2 + P_3 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			throw new ArgumentOutOfRangeException("numBytesToRead + readStartIndex must be < Length.");
		}
		EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_3, P_4, P_2);
	}

	public unsafe void pkpJIXUPRvEEdtemqnOqHyayGnzb(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		pkpJIXUPRvEEdtemqnOqHyayGnzb((byte*)(void*)P_0, P_1, P_2, P_3, P_4);
	}

	public unsafe int mAdXQxWuvrDlLbnADJNQEnsupYsm(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
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
		if (P_2 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_2 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			P_1 = yzHmKrgfGycYJStOocuNHTLVBjpo - P_2;
		}
		if (P_3 + P_1 > num)
		{
			P_1 = num - P_3;
		}
		if (P_1 == 0)
		{
			return 0;
		}
		NativeTools.CopyMemory((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_2, P_3, P_1);
		return P_1;
	}

	public unsafe int mAdXQxWuvrDlLbnADJNQEnsupYsm(byte* P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == null || P_2 <= 0)
		{
			return 0;
		}
		if (P_3 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_3 + P_2 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			P_2 = yzHmKrgfGycYJStOocuNHTLVBjpo - P_3;
		}
		if (P_4 + P_2 > P_1)
		{
			P_2 = P_1 - P_4;
		}
		EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_3, P_4, P_2);
		return P_2;
	}

	public unsafe int mAdXQxWuvrDlLbnADJNQEnsupYsm(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == IntPtr.Zero)
		{
			return 0;
		}
		return mAdXQxWuvrDlLbnADJNQEnsupYsm((byte*)(void*)P_0, P_1, P_2, P_3, P_4);
	}

	public unsafe void GkZNmvrFnhcPewsFfMhhVnoBgzAF(int P_0, byte P_1, bool P_2)
	{
		if (1 + P_0 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("byteIndex");
		}
		if (P_1 >= 8)
		{
			throw new ArgumentOutOfRangeException("bit");
		}
		if (P_2)
		{
			byte* num = UwuHDIqixLebpujhcqnVmliUjFRjA + P_0;
			*num |= (byte)(1 << (int)P_1);
		}
		else
		{
			byte* num2 = UwuHDIqixLebpujhcqnVmliUjFRjA + P_0;
			*num2 &= (byte)(~(1 << (int)P_1));
		}
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(byte P_0, int P_1)
	{
		if (1 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		UwuHDIqixLebpujhcqnVmliUjFRjA[P_1] = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(short P_0, int P_1)
	{
		if (2 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		*(short*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_1) = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(ushort P_0, int P_1)
	{
		if (2 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		*(ushort*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_1) = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(int P_0, int P_1)
	{
		if (4 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		*(int*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_1) = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(uint P_0, int P_1)
	{
		if (4 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		*(uint*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_1) = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(long P_0, int P_1)
	{
		if (8 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		*(long*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_1) = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(ulong P_0, int P_1)
	{
		if (8 + P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo || P_1 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		*(ulong*)(UwuHDIqixLebpujhcqnVmliUjFRjA + P_1) = P_0;
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
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
		if (P_1 > yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_2 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_1 + P_2 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite + writeStartIndex must be < Length.");
		}
		NativeTools.CopyMemory(P_0, (IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_3, P_2, P_1);
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(byte* P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == null)
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
		if (P_2 > yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_3 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_2 + P_3 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			throw new ArgumentOutOfRangeException("numBytesToWrite + writeStartIndex must be < Length.");
		}
		EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(P_0, UwuHDIqixLebpujhcqnVmliUjFRjA, P_4, P_3, P_2);
	}

	public unsafe void KjttXAEzsRtECEzfsLykXNbscFEq(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		KjttXAEzsRtECEzfsLykXNbscFEq((byte*)(void*)P_0, P_1, P_2, P_3, P_4);
	}

	public unsafe int APcdoAbGdHyHwKEUCXlikgFfSStdA(byte[] P_0, int P_1, int P_2 = 0, int P_3 = 0)
	{
		if (P_0 == null)
		{
			return 0;
		}
		int num = P_0.Length;
		if (num == 0 || P_1 <= 0 || P_3 >= num || P_2 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_1 + P_2 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			P_1 = yzHmKrgfGycYJStOocuNHTLVBjpo - P_2;
		}
		NativeTools.CopyMemory(P_0, (IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_3, P_2, P_1);
		return P_1;
	}

	public unsafe int APcdoAbGdHyHwKEUCXlikgFfSStdA(byte* P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == null || P_1 <= 0 || P_2 <= 0 || P_4 >= P_1 || P_3 >= yzHmKrgfGycYJStOocuNHTLVBjpo)
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
		if (P_2 + P_3 > yzHmKrgfGycYJStOocuNHTLVBjpo)
		{
			P_2 = yzHmKrgfGycYJStOocuNHTLVBjpo - P_3;
		}
		EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(P_0, UwuHDIqixLebpujhcqnVmliUjFRjA, P_4, P_3, P_2);
		return P_2;
	}

	public unsafe int APcdoAbGdHyHwKEUCXlikgFfSStdA(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		return APcdoAbGdHyHwKEUCXlikgFfSStdA((byte*)(void*)P_0, P_1, P_2, P_3, P_4);
	}

	public unsafe bool wZhBNjzUhcxMVUfUPRlDRkEipJDq(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("size");
		}
		if (yzHmKrgfGycYJStOocuNHTLVBjpo == P_0)
		{
			return true;
		}
		jbPdmCiDrOiUcqFlaQIItUCXQejs();
		if (P_0 == 0)
		{
			return true;
		}
		yzHmKrgfGycYJStOocuNHTLVBjpo = P_0;
		UwuHDIqixLebpujhcqnVmliUjFRjA = (byte*)(void*)Marshal.AllocHGlobal(P_0);
		ZrbFhGEbWRbTzVxxQimUntnkwisKA();
		return true;
	}

	public unsafe void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		if (yzHmKrgfGycYJStOocuNHTLVBjpo != 0)
		{
			EigdSMgqkBzyFeSpQPsmKNepnjhE.PnrdEHoiFUoVjeoZDBZbwGDJGFrGA(UwuHDIqixLebpujhcqnVmliUjFRjA, yzHmKrgfGycYJStOocuNHTLVBjpo);
		}
	}

	public unsafe void jbPdmCiDrOiUcqFlaQIItUCXQejs()
	{
		if (yzHmKrgfGycYJStOocuNHTLVBjpo == 0)
		{
			return;
		}
		try
		{
			if (UwuHDIqixLebpujhcqnVmliUjFRjA != null)
			{
				Marshal.FreeHGlobal(qhEAlcCfypKkEIlVORqfPYQDtvaj);
			}
		}
		catch
		{
		}
		UwuHDIqixLebpujhcqnVmliUjFRjA = null;
		yzHmKrgfGycYJStOocuNHTLVBjpo = 0;
	}

	public virtual string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		string text = "";
		for (int i = 0; i < yzHmKrgfGycYJStOocuNHTLVBjpo; i++)
		{
			text = text + GwzzmqKUcyhFPCqvgrcRvKXmlpyS(i).ToString("x2") + " ";
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
			jbPdmCiDrOiUcqFlaQIItUCXQejs();
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	[SpecialName]
	public unsafe static IntPtr zsNtDbJRpaGeYqGorVKjcaHZirNu(yfacCQRxzUzvTKAYYfCLqujfuVRt P_0)
	{
		if (P_0 == null)
		{
			return IntPtr.Zero;
		}
		return (IntPtr)P_0.UwuHDIqixLebpujhcqnVmliUjFRjA;
	}

	[SpecialName]
	public unsafe static void* zsNtDbJRpaGeYqGorVKjcaHZirNu(yfacCQRxzUzvTKAYYfCLqujfuVRt P_0)
	{
		if (P_0 == null)
		{
			return null;
		}
		return P_0.UwuHDIqixLebpujhcqnVmliUjFRjA;
	}

	public unsafe static bool OMzUIlvLGUEzvhPTLqSeKvddGQke(yfacCQRxzUzvTKAYYfCLqujfuVRt P_0, yfacCQRxzUzvTKAYYfCLqujfuVRt P_1)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("source");
		}
		if (P_1 == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (P_0.yzHmKrgfGycYJStOocuNHTLVBjpo == 0)
		{
			P_1.jbPdmCiDrOiUcqFlaQIItUCXQejs();
			return true;
		}
		if (P_1.wZhBNjzUhcxMVUfUPRlDRkEipJDq(P_0.yzHmKrgfGycYJStOocuNHTLVBjpo))
		{
			P_1.KjttXAEzsRtECEzfsLykXNbscFEq(P_0.UwuHDIqixLebpujhcqnVmliUjFRjA, P_0.yzHmKrgfGycYJStOocuNHTLVBjpo, P_0.yzHmKrgfGycYJStOocuNHTLVBjpo);
			return true;
		}
		return false;
	}
}
