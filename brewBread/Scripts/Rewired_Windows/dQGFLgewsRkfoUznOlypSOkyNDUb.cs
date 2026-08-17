using System;
using System.Runtime.CompilerServices;

internal struct dQGFLgewsRkfoUznOlypSOkyNDUb : IEquatable<dQGFLgewsRkfoUznOlypSOkyNDUb>
{
	public static readonly dQGFLgewsRkfoUznOlypSOkyNDUb VlWNMKqECXRzdRysMFqtzPIFTroe = new dQGFLgewsRkfoUznOlypSOkyNDUb(0, 0);

	public int LeaAlLlLUVpUZEtsNQKSfcVaXRbL;

	public int nYgMBIIMubbuyaBAFyuFsqYaYOAtA;

	public dQGFLgewsRkfoUznOlypSOkyNDUb(int P_0, int P_1)
	{
		LeaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0;
		nYgMBIIMubbuyaBAFyuFsqYaYOAtA = P_1;
	}

	public bool Equals(dQGFLgewsRkfoUznOlypSOkyNDUb other)
	{
		if (other.LeaAlLlLUVpUZEtsNQKSfcVaXRbL == LeaAlLlLUVpUZEtsNQKSfcVaXRbL)
		{
			return other.nYgMBIIMubbuyaBAFyuFsqYaYOAtA == nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		}
		return false;
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if ((object)P_0.GetType() != typeof(dQGFLgewsRkfoUznOlypSOkyNDUb))
		{
			return false;
		}
		return Equals((dQGFLgewsRkfoUznOlypSOkyNDUb)P_0);
	}

	public int vpCtZDiWtrmaqwDCmMjniXrnNrOD()
	{
		return (LeaAlLlLUVpUZEtsNQKSfcVaXRbL * 397) ^ nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
	}

	[SpecialName]
	public static bool OUvbuOHwkfdwNYEjxHMoxEMbFyifA(dQGFLgewsRkfoUznOlypSOkyNDUb P_0, dQGFLgewsRkfoUznOlypSOkyNDUb P_1)
	{
		return P_0.Equals(P_1);
	}

	[SpecialName]
	public static bool cJRKgPQzpjtShcOMUgofqgaeyvHQ(dQGFLgewsRkfoUznOlypSOkyNDUb P_0, dQGFLgewsRkfoUznOlypSOkyNDUb P_1)
	{
		return !P_0.Equals(P_1);
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return $"({LeaAlLlLUVpUZEtsNQKSfcVaXRbL},{nYgMBIIMubbuyaBAFyuFsqYaYOAtA})";
	}
}
