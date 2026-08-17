using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rewired;

[DefaultMember("Item")]
internal struct AgvfXhluxZViVrRcEjHJVwIOpUnB : IEquatable<AgvfXhluxZViVrRcEjHJVwIOpUnB>
{
	public ModifierKey ZIgWhlWHlCupODaLTBErrOtIhMJq;

	public ModifierKey sXZLWCudfSiLvhftzVVhdByYMHYB;

	public ModifierKey LVOXLQdqhieMhYyKMfbprcWmfUyv;

	private ModifierKey KOFNsPXmmmwxConMwxpDphVwipQX
	{
		get
		{
			if (P_0 <= 0)
			{
				return ZIgWhlWHlCupODaLTBErrOtIhMJq;
			}
			if (P_0 == 1)
			{
				return sXZLWCudfSiLvhftzVVhdByYMHYB;
			}
			if (P_0 >= 2)
			{
				return LVOXLQdqhieMhYyKMfbprcWmfUyv;
			}
			return ZIgWhlWHlCupODaLTBErrOtIhMJq;
		}
		set
		{
			if (num <= 0)
			{
				ZIgWhlWHlCupODaLTBErrOtIhMJq = modifierKey;
			}
			if (num == 1)
			{
				sXZLWCudfSiLvhftzVVhdByYMHYB = modifierKey;
			}
			if (num >= 2)
			{
				LVOXLQdqhieMhYyKMfbprcWmfUyv = modifierKey;
			}
		}
	}

	public AgvfXhluxZViVrRcEjHJVwIOpUnB(ModifierKey P_0, ModifierKey P_1, ModifierKey P_2)
	{
		ZIgWhlWHlCupODaLTBErrOtIhMJq = P_0;
		sXZLWCudfSiLvhftzVVhdByYMHYB = P_1;
		LVOXLQdqhieMhYyKMfbprcWmfUyv = P_2;
	}

	public void ahFEewaoBlbizUcORzHHNljyJdrt()
	{
		if (ZIgWhlWHlCupODaLTBErrOtIhMJq != ModifierKey.None)
		{
			ZIgWhlWHlCupODaLTBErrOtIhMJq = ModifierKey.None;
		}
		if (sXZLWCudfSiLvhftzVVhdByYMHYB != ModifierKey.None)
		{
			sXZLWCudfSiLvhftzVVhdByYMHYB = ModifierKey.None;
		}
		if (LVOXLQdqhieMhYyKMfbprcWmfUyv != ModifierKey.None)
		{
			LVOXLQdqhieMhYyKMfbprcWmfUyv = ModifierKey.None;
		}
	}

	public static AgvfXhluxZViVrRcEjHJVwIOpUnB BDmZsItjZWREiqJsCZQghFuSRSdY(ModifierKeyFlags P_0)
	{
		AgvfXhluxZViVrRcEjHJVwIOpUnB result = default(AgvfXhluxZViVrRcEjHJVwIOpUnB);
		int num = 0;
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Control))
		{
			result.CuJtMuaKYZgVpxmFTtNczjzwvjcT(num++, ModifierKey.Control);
		}
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Command))
		{
			result.CuJtMuaKYZgVpxmFTtNczjzwvjcT(num++, ModifierKey.Command);
		}
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Alt))
		{
			result.CuJtMuaKYZgVpxmFTtNczjzwvjcT(num++, ModifierKey.Alt);
		}
		if (num >= 3)
		{
			return result;
		}
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Shift))
		{
			result.CuJtMuaKYZgVpxmFTtNczjzwvjcT(num++, ModifierKey.Shift);
		}
		return result;
	}

	public bool Equals(AgvfXhluxZViVrRcEjHJVwIOpUnB other)
	{
		if (ZIgWhlWHlCupODaLTBErrOtIhMJq == other.ZIgWhlWHlCupODaLTBErrOtIhMJq && sXZLWCudfSiLvhftzVVhdByYMHYB == other.sXZLWCudfSiLvhftzVVhdByYMHYB)
		{
			return LVOXLQdqhieMhYyKMfbprcWmfUyv == other.LVOXLQdqhieMhYyKMfbprcWmfUyv;
		}
		return false;
	}

	bool IEquatable<AgvfXhluxZViVrRcEjHJVwIOpUnB>.Equals(AgvfXhluxZViVrRcEjHJVwIOpUnB other)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Equals
		return this.Equals(other);
	}

	public bool GBgfLLDuPIesIqCsylutYLOVhcXX(object P_0)
	{
		if (P_0 == null || !(P_0 is AgvfXhluxZViVrRcEjHJVwIOpUnB))
		{
			return false;
		}
		return Equals((AgvfXhluxZViVrRcEjHJVwIOpUnB)P_0);
	}

	public int yhVJuwrpUsDmwZDEUfcSFFOOaTKi()
	{
		return ((17 * 29 + ZIgWhlWHlCupODaLTBErrOtIhMJq.GetHashCode()) * 29 + sXZLWCudfSiLvhftzVVhdByYMHYB.GetHashCode()) * 29 + LVOXLQdqhieMhYyKMfbprcWmfUyv.GetHashCode();
	}

	[SpecialName]
	public static bool YcBAknjBNlystxYXFVgpNJFKSaMe(AgvfXhluxZViVrRcEjHJVwIOpUnB P_0, AgvfXhluxZViVrRcEjHJVwIOpUnB P_1)
	{
		if (P_0.ZIgWhlWHlCupODaLTBErrOtIhMJq == P_1.ZIgWhlWHlCupODaLTBErrOtIhMJq && P_0.sXZLWCudfSiLvhftzVVhdByYMHYB == P_1.sXZLWCudfSiLvhftzVVhdByYMHYB)
		{
			return P_0.LVOXLQdqhieMhYyKMfbprcWmfUyv == P_1.LVOXLQdqhieMhYyKMfbprcWmfUyv;
		}
		return false;
	}

	[SpecialName]
	public static bool lLjUPWdXiBUrJSzfGVRWPCFMjpXgA(AgvfXhluxZViVrRcEjHJVwIOpUnB P_0, AgvfXhluxZViVrRcEjHJVwIOpUnB P_1)
	{
		return !YcBAknjBNlystxYXFVgpNJFKSaMe(P_0, P_1);
	}
}
