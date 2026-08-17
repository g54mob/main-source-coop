using System;
using Rewired;
using Rewired.Utils.Classes.Utility;

internal class gDqyZLpNcQdFqLgOmEFQbYwoWlEy
{
	private class QvuZazvDXdjAbnQpGLVnKrBQaYIQ
	{
		[Flags]
		private enum DxdiOHAUjttrgHzxWOtXKcpSkgtJ : byte
		{
			None = 0,
			IsOnPositive = 1,
			IsOnNegative = 2,
			WasOnPrevPositive = 4,
			WasOnPrevNegative = 8
		}

		private DxdiOHAUjttrgHzxWOtXKcpSkgtJ ckUkzuSDJGuJonmPceZrDLTKWbwc;

		private uint EipLXHCtbSnOfJeYxOjiYvXCSNin;

		private bool ZVIoGtYvPAqctbduSqUnwksmgLtq;

		public bool hWwhWjTwkSIDOAdcCXLkyWnSzKE => ZVIoGtYvPAqctbduSqUnwksmgLtq;

		public ButtonStateFlags NORjGqUfBfxkUGfuXkAqFhMzQdUl(bool P_0)
		{
			ButtonStateFlags buttonStateFlags = ButtonStateFlags.Off;
			if (P_0)
			{
				if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.IsOnPositive) != DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None)
				{
					buttonStateFlags |= ButtonStateFlags.On;
					if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.WasOnPrevPositive) == 0)
					{
						buttonStateFlags |= ButtonStateFlags.Down;
					}
				}
				else if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.WasOnPrevPositive) != DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None)
				{
					buttonStateFlags |= ButtonStateFlags.Up;
				}
			}
			else if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.IsOnNegative) != DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None)
			{
				buttonStateFlags |= ButtonStateFlags.On;
				if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.WasOnPrevNegative) == 0)
				{
					buttonStateFlags |= ButtonStateFlags.Down;
				}
			}
			else if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.WasOnPrevNegative) != DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None)
			{
				buttonStateFlags |= ButtonStateFlags.Up;
			}
			return buttonStateFlags;
		}

		public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
		{
			DxdiOHAUjttrgHzxWOtXKcpSkgtJ dxdiOHAUjttrgHzxWOtXKcpSkgtJ = DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None;
			if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.IsOnPositive) != DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None)
			{
				dxdiOHAUjttrgHzxWOtXKcpSkgtJ |= DxdiOHAUjttrgHzxWOtXKcpSkgtJ.WasOnPrevPositive;
			}
			if ((ckUkzuSDJGuJonmPceZrDLTKWbwc & DxdiOHAUjttrgHzxWOtXKcpSkgtJ.IsOnNegative) != DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None)
			{
				dxdiOHAUjttrgHzxWOtXKcpSkgtJ |= DxdiOHAUjttrgHzxWOtXKcpSkgtJ.WasOnPrevNegative;
			}
			ckUkzuSDJGuJonmPceZrDLTKWbwc = dxdiOHAUjttrgHzxWOtXKcpSkgtJ;
		}

		public void JFPDzpFPVYxyqXpnZhoBGcTRgDUr(uint P_0)
		{
			if (EipLXHCtbSnOfJeYxOjiYvXCSNin < P_0 - 1)
			{
				ZVIoGtYvPAqctbduSqUnwksmgLtq = false;
			}
		}

		public void JsyGTqGZCidFncSRrtsjWdJIoaNeb(bool P_0)
		{
			if (P_0)
			{
				ckUkzuSDJGuJonmPceZrDLTKWbwc |= DxdiOHAUjttrgHzxWOtXKcpSkgtJ.IsOnPositive;
			}
			else
			{
				ckUkzuSDJGuJonmPceZrDLTKWbwc |= DxdiOHAUjttrgHzxWOtXKcpSkgtJ.IsOnNegative;
			}
			EipLXHCtbSnOfJeYxOjiYvXCSNin = ReInput.currentFrame;
			if (!ZVIoGtYvPAqctbduSqUnwksmgLtq)
			{
				ZVIoGtYvPAqctbduSqUnwksmgLtq = true;
			}
		}

		public void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			ckUkzuSDJGuJonmPceZrDLTKWbwc = DxdiOHAUjttrgHzxWOtXKcpSkgtJ.None;
			EipLXHCtbSnOfJeYxOjiYvXCSNin = 0u;
			ZVIoGtYvPAqctbduSqUnwksmgLtq = false;
		}
	}

	[Serializable]
	private sealed class WuAeqTixYjZqLDtyppyVKuGAPmsx
	{
		public static readonly WuAeqTixYjZqLDtyppyVKuGAPmsx _003C_003E9 = new WuAeqTixYjZqLDtyppyVKuGAPmsx();

		public static Func<QvuZazvDXdjAbnQpGLVnKrBQaYIQ> _003C_003E9__19_0;

		internal gDqyZLpNcQdFqLgOmEFQbYwoWlEy SiHfGvEipEVLPoHJZivYQwNvWcQI()
		{
			return new gDqyZLpNcQdFqLgOmEFQbYwoWlEy();
		}

		internal void lRAygbnGJhLQBmiIoUqrWcXyQOwI(gDqyZLpNcQdFqLgOmEFQbYwoWlEy P_0)
		{
			P_0.SPGTRPyvIslcMdbPTItsewSLRPxx();
		}

		internal QvuZazvDXdjAbnQpGLVnKrBQaYIQ WCdcMtPQkNomCbFSsGYMpxARTDGP()
		{
			return new QvuZazvDXdjAbnQpGLVnKrBQaYIQ();
		}
	}

	private const int hHzDCZOLnqfvSKBtRvzXrwBJcsou = 20;

	private const int exawYMwmRnjANTGdMcrjlUOgzNyW = 10;

	private static ObjectPool<gDqyZLpNcQdFqLgOmEFQbYwoWlEy> HIUFvXJVSLxtRaIuFDaudWgaDapQA;

	private static gDqyZLpNcQdFqLgOmEFQbYwoWlEy[] DNRsvHKlICOSgLqxhtLjjiPQgtxiA;

	private static int SkjwzNJeFnRPObGbercEpJdlGvzAA;

	public int KMqdJxPVmllcTpNUrAIsXRbmYwSK;

	private UpdateLoopDataSet<QvuZazvDXdjAbnQpGLVnKrBQaYIQ> BKofuWBAfBjqNRctqQfPIEsrtHaJA;

	public bool hWwhWjTwkSIDOAdcCXLkyWnSzKE
	{
		get
		{
			int count = BKofuWBAfBjqNRctqQfPIEsrtHaJA.Count;
			for (int i = 0; i < count; i++)
			{
				if (BKofuWBAfBjqNRctqQfPIEsrtHaJA[i].hWwhWjTwkSIDOAdcCXLkyWnSzKE)
				{
					return true;
				}
			}
			return false;
		}
	}

	static gDqyZLpNcQdFqLgOmEFQbYwoWlEy()
	{
		HIUFvXJVSLxtRaIuFDaudWgaDapQA = new ObjectPool<gDqyZLpNcQdFqLgOmEFQbYwoWlEy>(20, WuAeqTixYjZqLDtyppyVKuGAPmsx._003C_003E9.SiHfGvEipEVLPoHJZivYQwNvWcQI, WuAeqTixYjZqLDtyppyVKuGAPmsx._003C_003E9.lRAygbnGJhLQBmiIoUqrWcXyQOwI);
		DNRsvHKlICOSgLqxhtLjjiPQgtxiA = new gDqyZLpNcQdFqLgOmEFQbYwoWlEy[20];
	}

	public static void jpwugzufXqktYbXkMYboQpqCbQgL()
	{
		SkjwzNJeFnRPObGbercEpJdlGvzAA = 0;
		Array.Clear(DNRsvHKlICOSgLqxhtLjjiPQgtxiA, 0, DNRsvHKlICOSgLqxhtLjjiPQgtxiA.Length);
	}

	public static gDqyZLpNcQdFqLgOmEFQbYwoWlEy lCdBOCgTcbASGHDUTNwUUvAYnbKnA(int P_0)
	{
		for (int i = 0; i < SkjwzNJeFnRPObGbercEpJdlGvzAA; i++)
		{
			if (DNRsvHKlICOSgLqxhtLjjiPQgtxiA[i] != null && DNRsvHKlICOSgLqxhtLjjiPQgtxiA[i].KMqdJxPVmllcTpNUrAIsXRbmYwSK == P_0)
			{
				return DNRsvHKlICOSgLqxhtLjjiPQgtxiA[i];
			}
		}
		return null;
	}

	public static gDqyZLpNcQdFqLgOmEFQbYwoWlEy ksUjSAMUrwIVaaryoEPEWmsYdJdS(int P_0)
	{
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy gDqyZLpNcQdFqLgOmEFQbYwoWlEy2 = lCdBOCgTcbASGHDUTNwUUvAYnbKnA(P_0);
		if (gDqyZLpNcQdFqLgOmEFQbYwoWlEy2 != null)
		{
			return gDqyZLpNcQdFqLgOmEFQbYwoWlEy2;
		}
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy2 = HIUFvXJVSLxtRaIuFDaudWgaDapQA.Get();
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy2.OPieIRwFQUNEFaYLpKUnHAyPmaUs(P_0);
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy2.BKofuWBAfBjqNRctqQfPIEsrtHaJA.SetUpdateLoop(ReInput.currentUpdateLoop);
		SQffxOQBsdRjMKVguuiPbQdxSsmL(gDqyZLpNcQdFqLgOmEFQbYwoWlEy2);
		return gDqyZLpNcQdFqLgOmEFQbYwoWlEy2;
	}

	public static void zFgskLGCZhfDtwaydFtWOrswGxzB(UpdateLoopType P_0)
	{
		for (int i = 0; i < SkjwzNJeFnRPObGbercEpJdlGvzAA; i++)
		{
			if (DNRsvHKlICOSgLqxhtLjjiPQgtxiA[i] != null)
			{
				DNRsvHKlICOSgLqxhtLjjiPQgtxiA[i].jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0);
			}
		}
	}

	public static void JFPDzpFPVYxyqXpnZhoBGcTRgDUr(UpdateLoopType P_0, uint P_1)
	{
		for (int num = SkjwzNJeFnRPObGbercEpJdlGvzAA - 1; num >= 0; num--)
		{
			if (DNRsvHKlICOSgLqxhtLjjiPQgtxiA[num] == null)
			{
				if (num == SkjwzNJeFnRPObGbercEpJdlGvzAA - 1)
				{
					SkjwzNJeFnRPObGbercEpJdlGvzAA--;
				}
			}
			else
			{
				DNRsvHKlICOSgLqxhtLjjiPQgtxiA[num].JFPDzpFPVYxyqXpnZhoBGcTRgDUr(P_1);
				if (!DNRsvHKlICOSgLqxhtLjjiPQgtxiA[num].hWwhWjTwkSIDOAdcCXLkyWnSzKE)
				{
					YtXOPEKSlnKLRIgpEGrTeanLmVws(num);
				}
			}
		}
	}

	private static void SQffxOQBsdRjMKVguuiPbQdxSsmL(gDqyZLpNcQdFqLgOmEFQbYwoWlEy P_0)
	{
		int num = AmLflJJFJCXJGTsYyKIvfacdthRf();
		if (num < 0)
		{
			if (SkjwzNJeFnRPObGbercEpJdlGvzAA == DNRsvHKlICOSgLqxhtLjjiPQgtxiA.Length)
			{
				gDqyZLpNcQdFqLgOmEFQbYwoWlEy[] dNRsvHKlICOSgLqxhtLjjiPQgtxiA = DNRsvHKlICOSgLqxhtLjjiPQgtxiA;
				DNRsvHKlICOSgLqxhtLjjiPQgtxiA = new gDqyZLpNcQdFqLgOmEFQbYwoWlEy[DNRsvHKlICOSgLqxhtLjjiPQgtxiA.Length + 10];
				Array.Copy(dNRsvHKlICOSgLqxhtLjjiPQgtxiA, DNRsvHKlICOSgLqxhtLjjiPQgtxiA, dNRsvHKlICOSgLqxhtLjjiPQgtxiA.Length);
			}
			num = SkjwzNJeFnRPObGbercEpJdlGvzAA;
			SkjwzNJeFnRPObGbercEpJdlGvzAA++;
		}
		DNRsvHKlICOSgLqxhtLjjiPQgtxiA[num] = P_0;
	}

	private static void YtXOPEKSlnKLRIgpEGrTeanLmVws(int P_0)
	{
		if (P_0 >= 0 && P_0 < SkjwzNJeFnRPObGbercEpJdlGvzAA)
		{
			gDqyZLpNcQdFqLgOmEFQbYwoWlEy gDqyZLpNcQdFqLgOmEFQbYwoWlEy2 = DNRsvHKlICOSgLqxhtLjjiPQgtxiA[P_0];
			if (gDqyZLpNcQdFqLgOmEFQbYwoWlEy2 != null)
			{
				HIUFvXJVSLxtRaIuFDaudWgaDapQA.Return(gDqyZLpNcQdFqLgOmEFQbYwoWlEy2);
				DNRsvHKlICOSgLqxhtLjjiPQgtxiA[P_0] = null;
			}
			if (P_0 == SkjwzNJeFnRPObGbercEpJdlGvzAA - 1)
			{
				SkjwzNJeFnRPObGbercEpJdlGvzAA--;
			}
		}
	}

	private static int AmLflJJFJCXJGTsYyKIvfacdthRf()
	{
		for (int i = 0; i < SkjwzNJeFnRPObGbercEpJdlGvzAA; i++)
		{
			if (DNRsvHKlICOSgLqxhtLjjiPQgtxiA[i] == null)
			{
				return i;
			}
		}
		if (SkjwzNJeFnRPObGbercEpJdlGvzAA >= DNRsvHKlICOSgLqxhtLjjiPQgtxiA.Length)
		{
			return -1;
		}
		int skjwzNJeFnRPObGbercEpJdlGvzAA = SkjwzNJeFnRPObGbercEpJdlGvzAA;
		SkjwzNJeFnRPObGbercEpJdlGvzAA++;
		return skjwzNJeFnRPObGbercEpJdlGvzAA;
	}

	public ButtonStateFlags NORjGqUfBfxkUGfuXkAqFhMzQdUl(bool P_0)
	{
		return BKofuWBAfBjqNRctqQfPIEsrtHaJA.Current.NORjGqUfBfxkUGfuXkAqFhMzQdUl(P_0);
	}

	public gDqyZLpNcQdFqLgOmEFQbYwoWlEy()
	{
		BKofuWBAfBjqNRctqQfPIEsrtHaJA = new UpdateLoopDataSet<QvuZazvDXdjAbnQpGLVnKrBQaYIQ>(ReInput.UserData.ConfigVars.updateLoop, WuAeqTixYjZqLDtyppyVKuGAPmsx._003C_003E9.WCdcMtPQkNomCbFSsGYMpxARTDGP);
		SPGTRPyvIslcMdbPTItsewSLRPxx();
	}

	public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
	{
		BKofuWBAfBjqNRctqQfPIEsrtHaJA.SetUpdateLoop(P_0);
		BKofuWBAfBjqNRctqQfPIEsrtHaJA.Current.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
	}

	public void JFPDzpFPVYxyqXpnZhoBGcTRgDUr(uint P_0)
	{
		BKofuWBAfBjqNRctqQfPIEsrtHaJA.Current.JFPDzpFPVYxyqXpnZhoBGcTRgDUr(P_0);
	}

	public void JsyGTqGZCidFncSRrtsjWdJIoaNeb(UpdateLoopType P_0, bool P_1)
	{
		BKofuWBAfBjqNRctqQfPIEsrtHaJA.Current.JsyGTqGZCidFncSRrtsjWdJIoaNeb(P_1);
	}

	private void OPieIRwFQUNEFaYLpKUnHAyPmaUs(int P_0)
	{
		KMqdJxPVmllcTpNUrAIsXRbmYwSK = P_0;
	}

	private void SPGTRPyvIslcMdbPTItsewSLRPxx()
	{
		KMqdJxPVmllcTpNUrAIsXRbmYwSK = -1;
		for (int i = 0; i < BKofuWBAfBjqNRctqQfPIEsrtHaJA.Count; i++)
		{
			BKofuWBAfBjqNRctqQfPIEsrtHaJA[i].SPGTRPyvIslcMdbPTItsewSLRPxx();
		}
	}
}
