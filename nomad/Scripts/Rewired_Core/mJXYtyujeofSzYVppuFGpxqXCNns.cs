using System;
using System.Runtime.CompilerServices;
using Rewired;

internal struct mJXYtyujeofSzYVppuFGpxqXCNns : IEquatable<mJXYtyujeofSzYVppuFGpxqXCNns>
{
	public KeyboardKeyCode QKKGlRKAGDDivSyxqxATHASBqDXvA;

	public ModifierKey ZrYlectjIzguQqKdUISJiLcdcNbFb;

	public ModifierKey VeZERVErBhUGffitEHbPhBxSDjyxA;

	public ModifierKey DGagOoPspvxEPvHYEkbcTESnGoVfA;

	public mJXYtyujeofSzYVppuFGpxqXCNns(KeyboardKeyCode P_0, ModifierKey P_1, ModifierKey P_2, ModifierKey P_3)
	{
		QKKGlRKAGDDivSyxqxATHASBqDXvA = P_0;
		ZrYlectjIzguQqKdUISJiLcdcNbFb = P_1;
		VeZERVErBhUGffitEHbPhBxSDjyxA = P_2;
		DGagOoPspvxEPvHYEkbcTESnGoVfA = P_3;
	}

	public void sSBdDReNudVuCscxrOfCBWkDIyYKc()
	{
		if (QKKGlRKAGDDivSyxqxATHASBqDXvA != KeyboardKeyCode.None)
		{
			QKKGlRKAGDDivSyxqxATHASBqDXvA = KeyboardKeyCode.None;
		}
		if (ZrYlectjIzguQqKdUISJiLcdcNbFb != ModifierKey.None)
		{
			ZrYlectjIzguQqKdUISJiLcdcNbFb = ModifierKey.None;
		}
		if (VeZERVErBhUGffitEHbPhBxSDjyxA != ModifierKey.None)
		{
			VeZERVErBhUGffitEHbPhBxSDjyxA = ModifierKey.None;
		}
		if (DGagOoPspvxEPvHYEkbcTESnGoVfA != ModifierKey.None)
		{
			DGagOoPspvxEPvHYEkbcTESnGoVfA = ModifierKey.None;
		}
	}

	public bool Equals(mJXYtyujeofSzYVppuFGpxqXCNns other)
	{
		if (QKKGlRKAGDDivSyxqxATHASBqDXvA == other.QKKGlRKAGDDivSyxqxATHASBqDXvA && ZrYlectjIzguQqKdUISJiLcdcNbFb == other.ZrYlectjIzguQqKdUISJiLcdcNbFb && VeZERVErBhUGffitEHbPhBxSDjyxA == other.VeZERVErBhUGffitEHbPhBxSDjyxA)
		{
			return DGagOoPspvxEPvHYEkbcTESnGoVfA == other.DGagOoPspvxEPvHYEkbcTESnGoVfA;
		}
		return false;
	}

	bool IEquatable<mJXYtyujeofSzYVppuFGpxqXCNns>.Equals(mJXYtyujeofSzYVppuFGpxqXCNns other)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Equals
		return this.Equals(other);
	}

	public bool dMNgaTjYGtzoRdEEFySmTwDrxpZw(object P_0)
	{
		if (P_0 == null || !(P_0 is mJXYtyujeofSzYVppuFGpxqXCNns))
		{
			return false;
		}
		return Equals((mJXYtyujeofSzYVppuFGpxqXCNns)P_0);
	}

	public int gQdEIKQLeEjfDEQWqznDAqRNglArA()
	{
		return (((17 * 29 + QKKGlRKAGDDivSyxqxATHASBqDXvA.GetHashCode()) * 29 + ZrYlectjIzguQqKdUISJiLcdcNbFb.GetHashCode()) * 29 + VeZERVErBhUGffitEHbPhBxSDjyxA.GetHashCode()) * 29 + DGagOoPspvxEPvHYEkbcTESnGoVfA.GetHashCode();
	}

	[SpecialName]
	public static bool aZJuxmwvNbmnmeNOaNCvMIWznZwG(mJXYtyujeofSzYVppuFGpxqXCNns P_0, mJXYtyujeofSzYVppuFGpxqXCNns P_1)
	{
		if (P_0.QKKGlRKAGDDivSyxqxATHASBqDXvA == P_1.QKKGlRKAGDDivSyxqxATHASBqDXvA && P_0.ZrYlectjIzguQqKdUISJiLcdcNbFb == P_1.ZrYlectjIzguQqKdUISJiLcdcNbFb && P_0.VeZERVErBhUGffitEHbPhBxSDjyxA == P_1.VeZERVErBhUGffitEHbPhBxSDjyxA)
		{
			return P_0.DGagOoPspvxEPvHYEkbcTESnGoVfA == P_1.DGagOoPspvxEPvHYEkbcTESnGoVfA;
		}
		return false;
	}

	[SpecialName]
	public static bool yrtohYPPPYjtIbXrBRAWCEddhPWW(mJXYtyujeofSzYVppuFGpxqXCNns P_0, mJXYtyujeofSzYVppuFGpxqXCNns P_1)
	{
		return !aZJuxmwvNbmnmeNOaNCvMIWznZwG(P_0, P_1);
	}
}
