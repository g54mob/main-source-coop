using System;

internal class KakZxmGobwkVwIrgqulzhGFHgHYGA : IDisposable
{
	private readonly yfacCQRxzUzvTKAYYfCLqujfuVRt hqjaEUASFjOXvvoGsKkZpjKAdZUFb;

	private bool[] RkodxLFaImPacDKDxnPGSNeATzuN;

	protected readonly int bgRlSBvtBJHBlxNpncSbSQTSmqqM;

	protected readonly int CTVzpgRhTiDxbgXTcpWPhKXADeUxA;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public int ZJtPhjawZNcUHfaeqLxpHuAZchyx => bgRlSBvtBJHBlxNpncSbSQTSmqqM;

	public int oRjNdVSumbQrnPpjzCOLcZisplBNA => CTVzpgRhTiDxbgXTcpWPhKXADeUxA;

	public bool[] PtETWOVQGVTRQhUVeSWqZEIdIQMS => RkodxLFaImPacDKDxnPGSNeATzuN ?? (RkodxLFaImPacDKDxnPGSNeATzuN = new bool[bgRlSBvtBJHBlxNpncSbSQTSmqqM]);

	public KakZxmGobwkVwIrgqulzhGFHgHYGA(int P_0, int P_1)
	{
		if (P_0 <= 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("entryBitSize");
		}
		CTVzpgRhTiDxbgXTcpWPhKXADeUxA = P_0;
		bgRlSBvtBJHBlxNpncSbSQTSmqqM = P_1;
		int num = P_0 * P_1;
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb = new yfacCQRxzUzvTKAYYfCLqujfuVRt(num / 8 + ((num % 8 != 0) ? 1 : 0));
	}

	public unsafe void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, byte* P_1, int P_2)
	{
		if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			throw new IndexOutOfRangeException("index");
		}
		if (P_1 == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (P_2 < bgRlSBvtBJHBlxNpncSbSQTSmqqM)
		{
			throw new Exception("Buffer is too small to hold the data. Must be at least " + bgRlSBvtBJHBlxNpncSbSQTSmqqM + " bits.");
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < bgRlSBvtBJHBlxNpncSbSQTSmqqM; i++)
		{
			JgTtmmTdsIawGfBJoeVgddCKVVMSB(P_0, i, out var num3, out var b);
			P_1[i] = (hqjaEUASFjOXvvoGsKkZpjKAdZUFb.sJZlbulSeIynpgEjmrtermPqGItB(num3, b) ? ((byte)(P_1[num] | (1 << num2))) : ((byte)(P_1[num] & ~(1 << num2))));
			num2++;
			if (num2 >= 8)
			{
				num++;
				num2 = 0;
			}
		}
	}

	public unsafe void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, IntPtr P_1, int P_2)
	{
		if (P_1 == IntPtr.Zero)
		{
			throw new ArgumentNullException("buffer");
		}
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, (byte*)(void*)P_1, P_2);
	}

	public unsafe void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out byte P_1)
	{
		byte b = 0;
		byte* ptr = &b;
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, ptr, 64);
		P_1 = b;
	}

	public void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out sbyte P_1)
	{
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, out byte b);
		P_1 = (sbyte)b;
	}

	public unsafe void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out short P_1)
	{
		short num = 0;
		byte* ptr = (byte*)(&num);
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, ptr, 64);
		P_1 = num;
	}

	public void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out ushort P_1)
	{
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, out short num);
		P_1 = (ushort)num;
	}

	public unsafe void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out int P_1)
	{
		int num = 0;
		byte* ptr = (byte*)(&num);
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, ptr, 64);
		P_1 = num;
	}

	public void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out uint P_1)
	{
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, out int num);
		P_1 = (uint)num;
	}

	public unsafe void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out long P_1)
	{
		long num = 0L;
		byte* ptr = (byte*)(&num);
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, ptr, 64);
		P_1 = num;
	}

	public void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, out ulong P_1)
	{
		bzntzGEOucjfXHIGhmznjiQKaxfE(P_0, out long num);
		P_1 = (ulong)num;
	}

	public void bzntzGEOucjfXHIGhmznjiQKaxfE(int P_0, bool[] P_1)
	{
		if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			throw new IndexOutOfRangeException("index");
		}
		if (P_1 == null)
		{
			throw new ArgumentNullException("valueBuffer");
		}
		if (P_1.Length < bgRlSBvtBJHBlxNpncSbSQTSmqqM)
		{
			throw new Exception("valueBuffer.Length must be >= " + bgRlSBvtBJHBlxNpncSbSQTSmqqM);
		}
		for (int i = 0; i < bgRlSBvtBJHBlxNpncSbSQTSmqqM; i++)
		{
			JgTtmmTdsIawGfBJoeVgddCKVVMSB(P_0, i, out var num, out var b);
			P_1[i] = hqjaEUASFjOXvvoGsKkZpjKAdZUFb.sJZlbulSeIynpgEjmrtermPqGItB(num, b);
		}
	}

	public unsafe void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, byte* P_1, int P_2)
	{
		if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			throw new IndexOutOfRangeException("index");
		}
		if (P_1 == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (P_2 <= 0)
		{
			throw new Exception("bufferSize must be >= 0");
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < bgRlSBvtBJHBlxNpncSbSQTSmqqM; i++)
		{
			JgTtmmTdsIawGfBJoeVgddCKVVMSB(P_0, i, out var num3, out var b);
			bool flag = i < P_2 && (flag = (P_1[num] & (1 << num2)) != 0);
			hqjaEUASFjOXvvoGsKkZpjKAdZUFb.GkZNmvrFnhcPewsFfMhhVnoBgzAF(num3, b, flag);
			num2++;
			if (num2 >= 8)
			{
				num++;
				num2 = 0;
			}
		}
	}

	public unsafe void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, IntPtr P_1, int P_2)
	{
		if (P_1 == IntPtr.Zero)
		{
			throw new ArgumentNullException("buffer");
		}
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, (byte*)(void*)P_1, P_2);
	}

	public unsafe void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, byte P_1)
	{
		byte* ptr = &P_1;
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, ptr, 8);
	}

	public void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, sbyte P_1)
	{
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, (byte)P_1);
	}

	public unsafe void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, short P_1)
	{
		byte* ptr = (byte*)(&P_1);
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, ptr, 16);
	}

	public void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, ushort P_1)
	{
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, (short)P_1);
	}

	public unsafe void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, int P_1)
	{
		byte* ptr = (byte*)(&P_1);
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, ptr, 32);
	}

	public void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, uint P_1)
	{
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, (int)P_1);
	}

	public unsafe void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, long P_1)
	{
		byte* ptr = (byte*)(&P_1);
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, ptr, 64);
	}

	public void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, ulong P_1)
	{
		RPzvZUnzFHprsOVaYJSDSYKBntfq(P_0, (long)P_1);
	}

	public void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, bool[] P_1)
	{
		if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			throw new IndexOutOfRangeException("index");
		}
		if (P_1 == null)
		{
			throw new ArgumentNullException("valueBuffer");
		}
		if (P_1.Length < bgRlSBvtBJHBlxNpncSbSQTSmqqM)
		{
			throw new Exception("valueBuffer.Length must be >= " + bgRlSBvtBJHBlxNpncSbSQTSmqqM);
		}
		for (int i = 0; i < bgRlSBvtBJHBlxNpncSbSQTSmqqM; i++)
		{
			JgTtmmTdsIawGfBJoeVgddCKVVMSB(P_0, i, out var num, out var b);
			hqjaEUASFjOXvvoGsKkZpjKAdZUFb.GkZNmvrFnhcPewsFfMhhVnoBgzAF(num, b, P_1[i]);
		}
	}

	private void JgTtmmTdsIawGfBJoeVgddCKVVMSB(int P_0, int P_1, out int P_2, out byte P_3)
	{
		if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			throw new IndexOutOfRangeException("entryIndex");
		}
		if (P_1 < 0 || P_1 >= bgRlSBvtBJHBlxNpncSbSQTSmqqM)
		{
			throw new ArgumentOutOfRangeException("bitOffset");
		}
		int num = P_0 * bgRlSBvtBJHBlxNpncSbSQTSmqqM + P_1;
		P_2 = num / bgRlSBvtBJHBlxNpncSbSQTSmqqM;
		P_3 = (byte)(num - P_2 * bgRlSBvtBJHBlxNpncSbSQTSmqqM);
	}

	private int VgwvoUNINzdPBHslSdNuLbQeRSnOA(int P_0, out byte P_1)
	{
		if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA * bgRlSBvtBJHBlxNpncSbSQTSmqqM)
		{
			throw new IndexOutOfRangeException("bitIndex");
		}
		int num = P_0 / bgRlSBvtBJHBlxNpncSbSQTSmqqM;
		P_1 = (byte)(P_0 - num * bgRlSBvtBJHBlxNpncSbSQTSmqqM);
		return num;
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
			if (P_0 && hqjaEUASFjOXvvoGsKkZpjKAdZUFb != null)
			{
				hqjaEUASFjOXvvoGsKkZpjKAdZUFb.Dispose();
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}
}
