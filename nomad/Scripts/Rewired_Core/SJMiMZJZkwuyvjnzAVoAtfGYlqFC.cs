using System;
using Rewired;
using Rewired.Utils.Classes.Utility;

internal class SJMiMZJZkwuyvjnzAVoAtfGYlqFC
{
	private class NnStAFvJFztDXFWhitjoRpttZUOM
	{
		private ButtonStateFlags hVTKcAVcEDWUWvpAhIATrqcXCJ;

		private ButtonStateFlags JbGJIcKPLBciYCEzHcHLoQoJGmqM;

		private ButtonStateFlags XIsFGAXoOEAWhgUbbFXVObbbhJWl;

		private ButtonStateFlags LuQsMQvMoJPgtppURWqatjokbDeh;

		private uint tcBDHZicWrBBOFtFekRiNDjEwTLUA;

		private bool yyAlurPPQVYGDKQNiIVQnexyZZVJ;

		private bool NOIDHCJmQnhrCbXKxJRMTCycoDtS;

		private bool CVDjfqofBtYpDdHWoGEKgXKqccyB;

		private qBGoeVnKVifhULDhTYEtKsmyCkrB lOOwPoWWvantMijueqiZxjRmgCaG;

		public bool bvvxtQZWzadcrxCMXVxPBbmodlSCA => yyAlurPPQVYGDKQNiIVQnexyZZVJ;

		public bool wAWJcfmjrslRaPdnMgbwPqRYIBLw
		{
			get
			{
				return NOIDHCJmQnhrCbXKxJRMTCycoDtS;
			}
			set
			{
				NOIDHCJmQnhrCbXKxJRMTCycoDtS = nOIDHCJmQnhrCbXKxJRMTCycoDtS;
			}
		}

		public ButtonStateFlags RGdwlckFyfWAyonxShzfJfkbCrv(bool P_0)
		{
			bool flag;
			bool flag2;
			ButtonStateFlags buttonStateFlags;
			if (P_0)
			{
				flag = (hVTKcAVcEDWUWvpAhIATrqcXCJ & ButtonStateFlags.On) != 0;
				flag2 = (JbGJIcKPLBciYCEzHcHLoQoJGmqM & ButtonStateFlags.On) != 0;
				buttonStateFlags = ((!NOIDHCJmQnhrCbXKxJRMTCycoDtS) ? hVTKcAVcEDWUWvpAhIATrqcXCJ : ButtonStateFlags.Off);
			}
			else
			{
				flag = (XIsFGAXoOEAWhgUbbFXVObbbhJWl & ButtonStateFlags.On) != 0;
				flag2 = (LuQsMQvMoJPgtppURWqatjokbDeh & ButtonStateFlags.On) != 0;
				buttonStateFlags = ((!NOIDHCJmQnhrCbXKxJRMTCycoDtS) ? XIsFGAXoOEAWhgUbbFXVObbbhJWl : ButtonStateFlags.Off);
			}
			if (flag)
			{
				if (NOIDHCJmQnhrCbXKxJRMTCycoDtS)
				{
					if (flag2 && !CVDjfqofBtYpDdHWoGEKgXKqccyB && lOOwPoWWvantMijueqiZxjRmgCaG.ERNnDUTmwxeiuEOOuKXIzmfWhxXbb)
					{
						buttonStateFlags = ButtonStateFlags.Up;
					}
					return buttonStateFlags;
				}
				if (CVDjfqofBtYpDdHWoGEKgXKqccyB && lOOwPoWWvantMijueqiZxjRmgCaG.ERNnDUTmwxeiuEOOuKXIzmfWhxXbb)
				{
					buttonStateFlags |= ButtonStateFlags.Down;
				}
				if (!flag2)
				{
					buttonStateFlags |= ButtonStateFlags.Down;
				}
			}
			else if (flag2 && !NOIDHCJmQnhrCbXKxJRMTCycoDtS && !CVDjfqofBtYpDdHWoGEKgXKqccyB)
			{
				buttonStateFlags |= ButtonStateFlags.Up;
			}
			return buttonStateFlags;
		}

		public void UXSUXgfzSYHzPCxwHKdiuPYETeEH()
		{
			JbGJIcKPLBciYCEzHcHLoQoJGmqM = hVTKcAVcEDWUWvpAhIATrqcXCJ;
			LuQsMQvMoJPgtppURWqatjokbDeh = XIsFGAXoOEAWhgUbbFXVObbbhJWl;
			CVDjfqofBtYpDdHWoGEKgXKqccyB = NOIDHCJmQnhrCbXKxJRMTCycoDtS;
			hVTKcAVcEDWUWvpAhIATrqcXCJ = ButtonStateFlags.Off;
			XIsFGAXoOEAWhgUbbFXVObbbhJWl = ButtonStateFlags.Off;
		}

		public void AABTGPwEJkVthVaVrMaHzwVzPGTL(uint P_0)
		{
			if (tcBDHZicWrBBOFtFekRiNDjEwTLUA < P_0 - 1)
			{
				yyAlurPPQVYGDKQNiIVQnexyZZVJ = false;
			}
		}

		public void uFZMClqOwjLcOwqXLHtSaUaVFRro(bool P_0)
		{
			xRfOFiokYEhrDboTFCLCuaRymUwL((P_0 ? hVTKcAVcEDWUWvpAhIATrqcXCJ : XIsFGAXoOEAWhgUbbFXVObbbhJWl) | ButtonStateFlags.On, P_0);
		}

		public void xRfOFiokYEhrDboTFCLCuaRymUwL(ButtonStateFlags P_0, bool P_1)
		{
			if (P_1)
			{
				hVTKcAVcEDWUWvpAhIATrqcXCJ = P_0;
			}
			else
			{
				XIsFGAXoOEAWhgUbbFXVObbbhJWl = P_0;
			}
			tcBDHZicWrBBOFtFekRiNDjEwTLUA = ReInput.currentFrame;
			if (!yyAlurPPQVYGDKQNiIVQnexyZZVJ)
			{
				yyAlurPPQVYGDKQNiIVQnexyZZVJ = true;
			}
		}

		public void SlFlDLLBcMCnCSlnHplBXCIaGJyo(ref qBGoeVnKVifhULDhTYEtKsmyCkrB P_0)
		{
			lOOwPoWWvantMijueqiZxjRmgCaG = P_0;
			NOIDHCJmQnhrCbXKxJRMTCycoDtS = P_0.BTTSOiPbusxOBSfmzcMRKHiSiSzQ;
			CVDjfqofBtYpDdHWoGEKgXKqccyB = P_0.BTTSOiPbusxOBSfmzcMRKHiSiSzQ;
		}

		public void WrpKPdiWPfalLbMnyfUBCiRsEqTb()
		{
			hVTKcAVcEDWUWvpAhIATrqcXCJ = ButtonStateFlags.Off;
			JbGJIcKPLBciYCEzHcHLoQoJGmqM = ButtonStateFlags.Off;
			XIsFGAXoOEAWhgUbbFXVObbbhJWl = ButtonStateFlags.Off;
			LuQsMQvMoJPgtppURWqatjokbDeh = ButtonStateFlags.Off;
			tcBDHZicWrBBOFtFekRiNDjEwTLUA = 0u;
			yyAlurPPQVYGDKQNiIVQnexyZZVJ = false;
			NOIDHCJmQnhrCbXKxJRMTCycoDtS = false;
			CVDjfqofBtYpDdHWoGEKgXKqccyB = false;
		}
	}

	public struct qBGoeVnKVifhULDhTYEtKsmyCkrB
	{
		public bool ERNnDUTmwxeiuEOOuKXIzmfWhxXbb;

		public bool BTTSOiPbusxOBSfmzcMRKHiSiSzQ;

		public static qBGoeVnKVifhULDhTYEtKsmyCkrB vsiWpEnoWvWwmqMbAGEQSoRtujMl => default(qBGoeVnKVifhULDhTYEtKsmyCkrB);
	}

	[Serializable]
	private sealed class SRSaglzjriSaTwOOqUPGfdYkdFoh
	{
		public static readonly SRSaglzjriSaTwOOqUPGfdYkdFoh _003C_003E9 = new SRSaglzjriSaTwOOqUPGfdYkdFoh();

		public static Func<NnStAFvJFztDXFWhitjoRpttZUOM> _003C_003E9__22_0;

		internal SJMiMZJZkwuyvjnzAVoAtfGYlqFC IpyQnmuUeICLzpHJlbNAVbWEwKpM()
		{
			return new SJMiMZJZkwuyvjnzAVoAtfGYlqFC();
		}

		internal void cSBAnNBAhrmmYhxVzPbNsnNKwdpU(SJMiMZJZkwuyvjnzAVoAtfGYlqFC P_0)
		{
			P_0.PMvHcwoyzyOnRbxnkRkhUpZkoCTC();
		}

		internal NnStAFvJFztDXFWhitjoRpttZUOM jtUshkOFCsmblKBlSPVYaPZdFbGy()
		{
			return new NnStAFvJFztDXFWhitjoRpttZUOM();
		}
	}

	private const int rxAoYevgfLToAjSWfHXtprPqNQli = 20;

	private const int ObGuXKgnQScDGoJpLbTaQeHmgPB = 10;

	private static ObjectPool<SJMiMZJZkwuyvjnzAVoAtfGYlqFC> rPROqxAJnIdpNkoAFTXrjQgDpQpE;

	private static SJMiMZJZkwuyvjnzAVoAtfGYlqFC[] jJrmatTGIoWmdwnsuVQAUlJRogSr;

	private static int ovFDCHCnxntjTDgnDGsWbUHMlEBcA;

	public int QUOZKgdOmcfwYNoBbsuSWLdNnbzc;

	private UpdateLoopDataSet<NnStAFvJFztDXFWhitjoRpttZUOM> fXgmUnDkKlKqrRhTlPuYLKmdnNnT;

	public bool vCiehkrCwbqbyWOXKXpuvHcDfbFE
	{
		get
		{
			int count = fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Count;
			for (int i = 0; i < count; i++)
			{
				if (fXgmUnDkKlKqrRhTlPuYLKmdnNnT[i].bvvxtQZWzadcrxCMXVxPBbmodlSCA)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool qOLLFCdasZlBISSMjqpXBOjybSRdA
	{
		get
		{
			return fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.wAWJcfmjrslRaPdnMgbwPqRYIBLw;
		}
		set
		{
			fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.wAWJcfmjrslRaPdnMgbwPqRYIBLw = flag;
		}
	}

	static SJMiMZJZkwuyvjnzAVoAtfGYlqFC()
	{
		rPROqxAJnIdpNkoAFTXrjQgDpQpE = new ObjectPool<SJMiMZJZkwuyvjnzAVoAtfGYlqFC>(20, SRSaglzjriSaTwOOqUPGfdYkdFoh._003C_003E9.IpyQnmuUeICLzpHJlbNAVbWEwKpM, SRSaglzjriSaTwOOqUPGfdYkdFoh._003C_003E9.cSBAnNBAhrmmYhxVzPbNsnNKwdpU);
		jJrmatTGIoWmdwnsuVQAUlJRogSr = new SJMiMZJZkwuyvjnzAVoAtfGYlqFC[20];
	}

	public static void SdtZiaiGiOBRkWhsDyUVqFZZHOuT()
	{
		ovFDCHCnxntjTDgnDGsWbUHMlEBcA = 0;
		Array.Clear(jJrmatTGIoWmdwnsuVQAUlJRogSr, 0, jJrmatTGIoWmdwnsuVQAUlJRogSr.Length);
		rPROqxAJnIdpNkoAFTXrjQgDpQpE.Clear();
	}

	public static SJMiMZJZkwuyvjnzAVoAtfGYlqFC ftJHJOVhirgnAfpyAYwegOIzPfjTA(int P_0)
	{
		for (int i = 0; i < ovFDCHCnxntjTDgnDGsWbUHMlEBcA; i++)
		{
			if (jJrmatTGIoWmdwnsuVQAUlJRogSr[i] != null && jJrmatTGIoWmdwnsuVQAUlJRogSr[i].QUOZKgdOmcfwYNoBbsuSWLdNnbzc == P_0)
			{
				return jJrmatTGIoWmdwnsuVQAUlJRogSr[i];
			}
		}
		return null;
	}

	public static SJMiMZJZkwuyvjnzAVoAtfGYlqFC DFXjNULgOQrQHhNvRWZzvbhTKOA(int P_0, qBGoeVnKVifhULDhTYEtKsmyCkrB P_1)
	{
		SJMiMZJZkwuyvjnzAVoAtfGYlqFC sJMiMZJZkwuyvjnzAVoAtfGYlqFC = ftJHJOVhirgnAfpyAYwegOIzPfjTA(P_0);
		if (sJMiMZJZkwuyvjnzAVoAtfGYlqFC != null)
		{
			return sJMiMZJZkwuyvjnzAVoAtfGYlqFC;
		}
		sJMiMZJZkwuyvjnzAVoAtfGYlqFC = rPROqxAJnIdpNkoAFTXrjQgDpQpE.Get();
		sJMiMZJZkwuyvjnzAVoAtfGYlqFC.NurispHuKaXTHZTSmowappqArKAj(P_0);
		sJMiMZJZkwuyvjnzAVoAtfGYlqFC.dMtMKlLMkcQNuzTwPocKcZNNXrxb(ref P_1);
		sJMiMZJZkwuyvjnzAVoAtfGYlqFC.fXgmUnDkKlKqrRhTlPuYLKmdnNnT.SetUpdateLoop(ReInput.currentUpdateLoop);
		iSTgmSKkDrIGndIvODFWzmUzOiuA(sJMiMZJZkwuyvjnzAVoAtfGYlqFC);
		return sJMiMZJZkwuyvjnzAVoAtfGYlqFC;
	}

	public static void JggHihlzjjGVGuslzmppVdouDItL(UpdateLoopType P_0)
	{
		for (int i = 0; i < ovFDCHCnxntjTDgnDGsWbUHMlEBcA; i++)
		{
			if (jJrmatTGIoWmdwnsuVQAUlJRogSr[i] != null)
			{
				jJrmatTGIoWmdwnsuVQAUlJRogSr[i].EhIZuPYSfnkXJOLOhnJMpfgZUfve(P_0);
			}
		}
	}

	public static void JGDtVXAHzmzdeUnWsenokwdpxrorA(UpdateLoopType P_0, uint P_1)
	{
		for (int num = ovFDCHCnxntjTDgnDGsWbUHMlEBcA - 1; num >= 0; num--)
		{
			if (jJrmatTGIoWmdwnsuVQAUlJRogSr[num] == null)
			{
				if (num == ovFDCHCnxntjTDgnDGsWbUHMlEBcA - 1)
				{
					ovFDCHCnxntjTDgnDGsWbUHMlEBcA--;
				}
			}
			else
			{
				jJrmatTGIoWmdwnsuVQAUlJRogSr[num].szJwHuRkelRWThCnOdpQchydEIVy(P_1);
				if (!jJrmatTGIoWmdwnsuVQAUlJRogSr[num].vCiehkrCwbqbyWOXKXpuvHcDfbFE)
				{
					nCqdcTeKeszUEyPBkDmYGPYADRjQA(num);
				}
			}
		}
	}

	private static void iSTgmSKkDrIGndIvODFWzmUzOiuA(SJMiMZJZkwuyvjnzAVoAtfGYlqFC P_0)
	{
		int num = NzGHiWhxfcNNUqHyORPnJfGtIHfEA();
		if (num < 0)
		{
			if (ovFDCHCnxntjTDgnDGsWbUHMlEBcA == jJrmatTGIoWmdwnsuVQAUlJRogSr.Length)
			{
				SJMiMZJZkwuyvjnzAVoAtfGYlqFC[] array = jJrmatTGIoWmdwnsuVQAUlJRogSr;
				jJrmatTGIoWmdwnsuVQAUlJRogSr = new SJMiMZJZkwuyvjnzAVoAtfGYlqFC[jJrmatTGIoWmdwnsuVQAUlJRogSr.Length + 10];
				Array.Copy(array, jJrmatTGIoWmdwnsuVQAUlJRogSr, array.Length);
			}
			num = ovFDCHCnxntjTDgnDGsWbUHMlEBcA;
			ovFDCHCnxntjTDgnDGsWbUHMlEBcA++;
		}
		jJrmatTGIoWmdwnsuVQAUlJRogSr[num] = P_0;
	}

	private static void nCqdcTeKeszUEyPBkDmYGPYADRjQA(int P_0)
	{
		if (P_0 >= 0 && P_0 < ovFDCHCnxntjTDgnDGsWbUHMlEBcA)
		{
			SJMiMZJZkwuyvjnzAVoAtfGYlqFC sJMiMZJZkwuyvjnzAVoAtfGYlqFC = jJrmatTGIoWmdwnsuVQAUlJRogSr[P_0];
			if (sJMiMZJZkwuyvjnzAVoAtfGYlqFC != null)
			{
				rPROqxAJnIdpNkoAFTXrjQgDpQpE.Return(sJMiMZJZkwuyvjnzAVoAtfGYlqFC);
				jJrmatTGIoWmdwnsuVQAUlJRogSr[P_0] = null;
			}
			if (P_0 == ovFDCHCnxntjTDgnDGsWbUHMlEBcA - 1)
			{
				ovFDCHCnxntjTDgnDGsWbUHMlEBcA--;
			}
		}
	}

	private static int NzGHiWhxfcNNUqHyORPnJfGtIHfEA()
	{
		for (int i = 0; i < ovFDCHCnxntjTDgnDGsWbUHMlEBcA; i++)
		{
			if (jJrmatTGIoWmdwnsuVQAUlJRogSr[i] == null)
			{
				return i;
			}
		}
		if (ovFDCHCnxntjTDgnDGsWbUHMlEBcA >= jJrmatTGIoWmdwnsuVQAUlJRogSr.Length)
		{
			return -1;
		}
		int result = ovFDCHCnxntjTDgnDGsWbUHMlEBcA;
		ovFDCHCnxntjTDgnDGsWbUHMlEBcA++;
		return result;
	}

	public ButtonStateFlags jbgfcTRHsxwRThLPghNULhDTfYHB(bool P_0)
	{
		return fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.RGdwlckFyfWAyonxShzfJfkbCrv(P_0);
	}

	public SJMiMZJZkwuyvjnzAVoAtfGYlqFC()
	{
		fXgmUnDkKlKqrRhTlPuYLKmdnNnT = new UpdateLoopDataSet<NnStAFvJFztDXFWhitjoRpttZUOM>(ReInput.UserData.ConfigVars.updateLoop, SRSaglzjriSaTwOOqUPGfdYkdFoh._003C_003E9.jtUshkOFCsmblKBlSPVYaPZdFbGy);
		PMvHcwoyzyOnRbxnkRkhUpZkoCTC();
	}

	public void EhIZuPYSfnkXJOLOhnJMpfgZUfve(UpdateLoopType P_0)
	{
		fXgmUnDkKlKqrRhTlPuYLKmdnNnT.SetUpdateLoop(P_0);
		fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.UXSUXgfzSYHzPCxwHKdiuPYETeEH();
	}

	public void szJwHuRkelRWThCnOdpQchydEIVy(uint P_0)
	{
		fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.AABTGPwEJkVthVaVrMaHzwVzPGTL(P_0);
	}

	public void ITMCWPkgGsOFkdMFOSFvXjIogvFg(UpdateLoopType P_0, bool P_1)
	{
		fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.uFZMClqOwjLcOwqXLHtSaUaVFRro(P_1);
	}

	public void LXTCaFDkmlzkVLNnHNcpjJnNkhlDb(UpdateLoopType P_0, ButtonStateFlags P_1, bool P_2)
	{
		fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Current.xRfOFiokYEhrDboTFCLCuaRymUwL(P_1, P_2);
	}

	private void dMtMKlLMkcQNuzTwPocKcZNNXrxb(ref qBGoeVnKVifhULDhTYEtKsmyCkrB P_0)
	{
		int count = fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Count;
		for (int i = 0; i < count; i++)
		{
			fXgmUnDkKlKqrRhTlPuYLKmdnNnT[i].SlFlDLLBcMCnCSlnHplBXCIaGJyo(ref P_0);
		}
	}

	private void NurispHuKaXTHZTSmowappqArKAj(int P_0)
	{
		QUOZKgdOmcfwYNoBbsuSWLdNnbzc = P_0;
	}

	private void PMvHcwoyzyOnRbxnkRkhUpZkoCTC()
	{
		QUOZKgdOmcfwYNoBbsuSWLdNnbzc = -1;
		int count = fXgmUnDkKlKqrRhTlPuYLKmdnNnT.Count;
		for (int i = 0; i < count; i++)
		{
			fXgmUnDkKlKqrRhTlPuYLKmdnNnT[i].WrpKPdiWPfalLbMnyfUBCiRsEqTb();
		}
	}
}
