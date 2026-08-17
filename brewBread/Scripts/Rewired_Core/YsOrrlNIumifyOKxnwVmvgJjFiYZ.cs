using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

internal class YsOrrlNIumifyOKxnwVmvgJjFiYZ
{
	public class wraDGMFxQURPTbuMUtaFmiiYUDpGA
	{
		private class eTaEbojUFFkcnGyOhZiLQDhEZYzg : ExpandableArray_DataContainer<eTaEbojUFFkcnGyOhZiLQDhEZYzg>.qARAlhhogsFTixiCVGpIfnVauIuJb, IComparable<eTaEbojUFFkcnGyOhZiLQDhEZYzg>
		{
			public KeyboardKeyCode qwkeeArhjFVfINdRhYgfKXVRHPQW;

			public ModifierKeyFlags vrAzjOhpYDFTGkdqfiArqwGYMEZC;

			public void MPIdpLqhJeCJHWkSZORdXwngeGgr(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
			{
				qwkeeArhjFVfINdRhYgfKXVRHPQW = P_0;
				vrAzjOhpYDFTGkdqfiArqwGYMEZC = P_1;
			}

			public void Set(eTaEbojUFFkcnGyOhZiLQDhEZYzg P_0)
			{
				qwkeeArhjFVfINdRhYgfKXVRHPQW = P_0.qwkeeArhjFVfINdRhYgfKXVRHPQW;
				vrAzjOhpYDFTGkdqfiArqwGYMEZC = P_0.vrAzjOhpYDFTGkdqfiArqwGYMEZC;
			}

			public bool Equals(eTaEbojUFFkcnGyOhZiLQDhEZYzg P_0)
			{
				if (qwkeeArhjFVfINdRhYgfKXVRHPQW == P_0.qwkeeArhjFVfINdRhYgfKXVRHPQW && vrAzjOhpYDFTGkdqfiArqwGYMEZC == P_0.vrAzjOhpYDFTGkdqfiArqwGYMEZC)
				{
					return true;
				}
				return false;
			}

			public void Clear()
			{
				qwkeeArhjFVfINdRhYgfKXVRHPQW = KeyboardKeyCode.None;
				vrAzjOhpYDFTGkdqfiArqwGYMEZC = ModifierKeyFlags.None;
			}

			public int CompareTo(eTaEbojUFFkcnGyOhZiLQDhEZYzg other)
			{
				return 0;
			}
		}

		private enum eYPpWADyuEgsYLCCJLldOMfVCmgx
		{
			Map = 0,
			ActiveSet = 1
		}

		private ModifierKeyFlags SfQFopZOPGDmrUQKsDfbKoIHmrKHA;

		private ExpandableArray_DataContainer<eTaEbojUFFkcnGyOhZiLQDhEZYzg> TpuDzHJHNoINnYUBNeatknSKkEQf;

		private ExpandableArray_DataContainer<eTaEbojUFFkcnGyOhZiLQDhEZYzg> GMlRZdBXMcgsiDAKoJdhlvWrpgjp;

		private Keyboard OlktKbOjbgGrTJEsAkUzsiYeMHYb;

		public wraDGMFxQURPTbuMUtaFmiiYUDpGA(Keyboard P_0)
		{
			OlktKbOjbgGrTJEsAkUzsiYeMHYb = P_0;
			SfQFopZOPGDmrUQKsDfbKoIHmrKHA = ModifierKeyFlags.None;
			TpuDzHJHNoINnYUBNeatknSKkEQf = new ExpandableArray_DataContainer<eTaEbojUFFkcnGyOhZiLQDhEZYzg>(132, false, 132);
			GMlRZdBXMcgsiDAKoJdhlvWrpgjp = new ExpandableArray_DataContainer<eTaEbojUFFkcnGyOhZiLQDhEZYzg>(5, false, 5);
		}

		public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
		{
			SfQFopZOPGDmrUQKsDfbKoIHmrKHA = ModifierKeyFlags.None;
			TpuDzHJHNoINnYUBNeatknSKkEQf.Clear();
			for (int num = GMlRZdBXMcgsiDAKoJdhlvWrpgjp.Length - 1; num >= 0; num--)
			{
				eTaEbojUFFkcnGyOhZiLQDhEZYzg eTaEbojUFFkcnGyOhZiLQDhEZYzg2 = GMlRZdBXMcgsiDAKoJdhlvWrpgjp[num];
				if (!OlktKbOjbgGrTJEsAkUzsiYeMHYb.NqnspTmKVTuOenOXgaESJYqWWgyMA(eTaEbojUFFkcnGyOhZiLQDhEZYzg2.qwkeeArhjFVfINdRhYgfKXVRHPQW))
				{
					GMlRZdBXMcgsiDAKoJdhlvWrpgjp.RemoveAt(num);
				}
			}
		}

		public void gQybQLjzPnJAxBYJzNuzJhkJhbfi(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				SfQFopZOPGDmrUQKsDfbKoIHmrKHA |= P_0.modifierKeyFlags;
				TpuDzHJHNoINnYUBNeatknSKkEQf.injector.MPIdpLqhJeCJHWkSZORdXwngeGgr(P_0._keyboardKeyCode, P_0.modifierKeyFlags);
				TpuDzHJHNoINnYUBNeatknSKkEQf.Inject();
			}
		}

		public bool wghgqtZlwhrUKjEdoGfqoHsvyNGQ(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
		{
			if (SfQFopZOPGDmrUQKsDfbKoIHmrKHA == ModifierKeyFlags.None && P_1 == ModifierKeyFlags.None)
			{
				return false;
			}
			int num = Keyboard.HVRdbMGJASvNJRKWhACVREVAjGDe(P_1);
			if (wghgqtZlwhrUKjEdoGfqoHsvyNGQ(TpuDzHJHNoINnYUBNeatknSKkEQf, P_0, P_1, num, eYPpWADyuEgsYLCCJLldOMfVCmgx.Map))
			{
				return true;
			}
			if (wghgqtZlwhrUKjEdoGfqoHsvyNGQ(GMlRZdBXMcgsiDAKoJdhlvWrpgjp, P_0, P_1, num, eYPpWADyuEgsYLCCJLldOMfVCmgx.ActiveSet))
			{
				return true;
			}
			if (P_1 != ModifierKeyFlags.None)
			{
				GMlRZdBXMcgsiDAKoJdhlvWrpgjp.injector.MPIdpLqhJeCJHWkSZORdXwngeGgr(P_0, P_1);
				GMlRZdBXMcgsiDAKoJdhlvWrpgjp.InjectIfUnique();
			}
			return false;
		}

		private bool wghgqtZlwhrUKjEdoGfqoHsvyNGQ(ExpandableArray_DataContainer<eTaEbojUFFkcnGyOhZiLQDhEZYzg> P_0, KeyboardKeyCode P_1, ModifierKeyFlags P_2, int P_3, eYPpWADyuEgsYLCCJLldOMfVCmgx P_4)
		{
			bool flag = Keyboard.trZafsVxHDJMaoTvWPAzFzJbrlbl(P_1);
			int length = P_0.Length;
			for (int i = 0; i < length; i++)
			{
				eTaEbojUFFkcnGyOhZiLQDhEZYzg eTaEbojUFFkcnGyOhZiLQDhEZYzg2 = P_0[i];
				bool flag2 = eTaEbojUFFkcnGyOhZiLQDhEZYzg2.qwkeeArhjFVfINdRhYgfKXVRHPQW == P_1;
				if ((!flag2 || eTaEbojUFFkcnGyOhZiLQDhEZYzg2.vrAzjOhpYDFTGkdqfiArqwGYMEZC != P_2) && (flag2 || Keyboard.ModifierKeyFlagsContain(eTaEbojUFFkcnGyOhZiLQDhEZYzg2.vrAzjOhpYDFTGkdqfiArqwGYMEZC, (KeyCode)P_1) || MathTools.oASDHnqDjINJxGdPOwEdtRXNiGll((int)eTaEbojUFFkcnGyOhZiLQDhEZYzg2.vrAzjOhpYDFTGkdqfiArqwGYMEZC, (int)P_2)) && (flag || eTaEbojUFFkcnGyOhZiLQDhEZYzg2.qwkeeArhjFVfINdRhYgfKXVRHPQW == P_1) && Keyboard.HVRdbMGJASvNJRKWhACVREVAjGDe(eTaEbojUFFkcnGyOhZiLQDhEZYzg2.vrAzjOhpYDFTGkdqfiArqwGYMEZC) > P_3)
				{
					if (P_4 != eYPpWADyuEgsYLCCJLldOMfVCmgx.Map)
					{
						return true;
					}
					if (OlktKbOjbgGrTJEsAkUzsiYeMHYb.ngSbgQwwHNlDRfcEWdGhCacxQsPyA(eTaEbojUFFkcnGyOhZiLQDhEZYzg2.qwkeeArhjFVfINdRhYgfKXVRHPQW, eTaEbojUFFkcnGyOhZiLQDhEZYzg2.vrAzjOhpYDFTGkdqfiArqwGYMEZC))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			SfQFopZOPGDmrUQKsDfbKoIHmrKHA = ModifierKeyFlags.None;
			TpuDzHJHNoINnYUBNeatknSKkEQf.Clear();
			GMlRZdBXMcgsiDAKoJdhlvWrpgjp.Clear();
		}
	}

	private readonly wraDGMFxQURPTbuMUtaFmiiYUDpGA[] BKofuWBAfBjqNRctqQfPIEsrtHaJA;

	private UpdateLoopType IykDTZdiEUdjfaMPasESCgLSFroIc;

	private readonly Keyboard OlktKbOjbgGrTJEsAkUzsiYeMHYb;

	private wraDGMFxQURPTbuMUtaFmiiYUDpGA SgJVPHrjFyHgCSEpxtgUijWhNNop;

	public YsOrrlNIumifyOKxnwVmvgJjFiYZ(UpdateLoopSetting P_0, Keyboard P_1)
	{
		OlktKbOjbgGrTJEsAkUzsiYeMHYb = P_1;
		BKofuWBAfBjqNRctqQfPIEsrtHaJA = new wraDGMFxQURPTbuMUtaFmiiYUDpGA[3];
		int num = 0;
		using TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3);
		List<UpdateLoopType> list = tList.list;
		EnumConverter.ToUpdateLoopTypes(P_0, list);
		for (int i = 0; i < list.Count; i++)
		{
			wraDGMFxQURPTbuMUtaFmiiYUDpGA wraDGMFxQURPTbuMUtaFmiiYUDpGA2 = new wraDGMFxQURPTbuMUtaFmiiYUDpGA(P_1);
			BKofuWBAfBjqNRctqQfPIEsrtHaJA[(int)list[i]] = wraDGMFxQURPTbuMUtaFmiiYUDpGA2;
			num++;
			if (num == 1)
			{
				SgJVPHrjFyHgCSEpxtgUijWhNNop = wraDGMFxQURPTbuMUtaFmiiYUDpGA2;
			}
		}
	}

	public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
	{
		if (IykDTZdiEUdjfaMPasESCgLSFroIc != P_0)
		{
			IykDTZdiEUdjfaMPasESCgLSFroIc = P_0;
			SgJVPHrjFyHgCSEpxtgUijWhNNop = BKofuWBAfBjqNRctqQfPIEsrtHaJA[(int)P_0];
		}
		SgJVPHrjFyHgCSEpxtgUijWhNNop.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
	}

	public void BSeGhuIEzhduphsRnoNEHUwZyfSw(KeyboardMap P_0)
	{
		if (P_0 == null)
		{
			return;
		}
		AList<ActionElementMap> aList = P_0.cFGJUwYhcfUZYvBnprhEpVaWEPom;
		int count = aList._count;
		for (int i = 0; i < count; i++)
		{
			ActionElementMap actionElementMap = aList._items[i];
			if (actionElementMap.hasModifiers)
			{
				SgJVPHrjFyHgCSEpxtgUijWhNNop.gQybQLjzPnJAxBYJzNuzJhkJhbfi(actionElementMap);
			}
		}
	}

	public bool wghgqtZlwhrUKjEdoGfqoHsvyNGQ(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
	{
		return SgJVPHrjFyHgCSEpxtgUijWhNNop.wghgqtZlwhrUKjEdoGfqoHsvyNGQ(P_0, P_1);
	}

	public void HPdhUycvZEuKWkzUokzqofIAeNbfb()
	{
		for (int i = 0; i < BKofuWBAfBjqNRctqQfPIEsrtHaJA.Length; i++)
		{
			if (BKofuWBAfBjqNRctqQfPIEsrtHaJA[i] != null)
			{
				BKofuWBAfBjqNRctqQfPIEsrtHaJA[i].SPGTRPyvIslcMdbPTItsewSLRPxx();
			}
		}
	}
}
