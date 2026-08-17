using System;
using System.Runtime.CompilerServices;

internal struct PlquNopAsLVjFdhNJigpacCXRrgV : IEquatable<PlquNopAsLVjFdhNJigpacCXRrgV>
{
	public static readonly PlquNopAsLVjFdhNJigpacCXRrgV VlWNMKqECXRzdRysMFqtzPIFTroe = new PlquNopAsLVjFdhNJigpacCXRrgV(0f, 0f);

	public static readonly PlquNopAsLVjFdhNJigpacCXRrgV qRTPfFtBywBRObYgLHmMABaHLKxMA = VlWNMKqECXRzdRysMFqtzPIFTroe;

	public float yjjazKvArXuHaXesRtCkdAFWpISD;

	public float WdzQAWxDFDDqlDmFnWEDiviFeUqnA;

	public PlquNopAsLVjFdhNJigpacCXRrgV(float P_0, float P_1)
	{
		yjjazKvArXuHaXesRtCkdAFWpISD = P_0;
		WdzQAWxDFDDqlDmFnWEDiviFeUqnA = P_1;
	}

	public bool Equals(PlquNopAsLVjFdhNJigpacCXRrgV other)
	{
		if (other.yjjazKvArXuHaXesRtCkdAFWpISD == yjjazKvArXuHaXesRtCkdAFWpISD)
		{
			return other.WdzQAWxDFDDqlDmFnWEDiviFeUqnA == WdzQAWxDFDDqlDmFnWEDiviFeUqnA;
		}
		return false;
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if ((object)P_0.GetType() != typeof(PlquNopAsLVjFdhNJigpacCXRrgV))
		{
			return false;
		}
		return Equals((PlquNopAsLVjFdhNJigpacCXRrgV)P_0);
	}

	public int vpCtZDiWtrmaqwDCmMjniXrnNrOD()
	{
		return (yjjazKvArXuHaXesRtCkdAFWpISD.GetHashCode() * 397) ^ WdzQAWxDFDDqlDmFnWEDiviFeUqnA.GetHashCode();
	}

	[SpecialName]
	public static bool OUvbuOHwkfdwNYEjxHMoxEMbFyifA(PlquNopAsLVjFdhNJigpacCXRrgV P_0, PlquNopAsLVjFdhNJigpacCXRrgV P_1)
	{
		return P_0.Equals(P_1);
	}

	[SpecialName]
	public static bool cJRKgPQzpjtShcOMUgofqgaeyvHQ(PlquNopAsLVjFdhNJigpacCXRrgV P_0, PlquNopAsLVjFdhNJigpacCXRrgV P_1)
	{
		return !P_0.Equals(P_1);
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return $"({yjjazKvArXuHaXesRtCkdAFWpISD},{WdzQAWxDFDDqlDmFnWEDiviFeUqnA})";
	}
}
