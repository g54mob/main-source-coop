using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 4)]
internal struct DBRasKQfdBHalDCRoBBuaVANJUde : IEquatable<DBRasKQfdBHalDCRoBBuaVANJUde>
{
	private int sjMQXthpMbLZjeFEznySQKgMgEzV;

	public DBRasKQfdBHalDCRoBBuaVANJUde(bool P_0)
	{
		sjMQXthpMbLZjeFEznySQKgMgEzV = (P_0 ? 1 : 0);
	}

	public bool Equals(DBRasKQfdBHalDCRoBBuaVANJUde other)
	{
		return sjMQXthpMbLZjeFEznySQKgMgEzV == other.sjMQXthpMbLZjeFEznySQKgMgEzV;
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_0 is DBRasKQfdBHalDCRoBBuaVANJUde)
		{
			return Equals((DBRasKQfdBHalDCRoBBuaVANJUde)P_0);
		}
		return false;
	}

	public int vpCtZDiWtrmaqwDCmMjniXrnNrOD()
	{
		return sjMQXthpMbLZjeFEznySQKgMgEzV;
	}

	[SpecialName]
	public static bool OUvbuOHwkfdwNYEjxHMoxEMbFyifA(DBRasKQfdBHalDCRoBBuaVANJUde P_0, DBRasKQfdBHalDCRoBBuaVANJUde P_1)
	{
		return P_0.Equals(P_1);
	}

	[SpecialName]
	public static bool cJRKgPQzpjtShcOMUgofqgaeyvHQ(DBRasKQfdBHalDCRoBBuaVANJUde P_0, DBRasKQfdBHalDCRoBBuaVANJUde P_1)
	{
		return !P_0.Equals(P_1);
	}

	[SpecialName]
	public static bool zsNtDbJRpaGeYqGorVKjcaHZirNu(DBRasKQfdBHalDCRoBBuaVANJUde P_0)
	{
		return P_0.sjMQXthpMbLZjeFEznySQKgMgEzV != 0;
	}

	[SpecialName]
	public static DBRasKQfdBHalDCRoBBuaVANJUde zsNtDbJRpaGeYqGorVKjcaHZirNu(bool P_0)
	{
		return new DBRasKQfdBHalDCRoBBuaVANJUde(P_0);
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return $"{sjMQXthpMbLZjeFEznySQKgMgEzV != 0}";
	}
}
