using System;
using System.Diagnostics;
using System.Threading;
using Rewired;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal class dXlctfYTaIJgQQIMXYbceYWYtRZE : IDisposable
{
	private enum YNIJnOblxligSolAcDiwOKeAGKHO
	{
		Idle = 0,
		Waiting = 1,
		ErrorPending = 2,
		FinishedError = 3,
		SuccessPending = 4,
		FinishedSuccess = 5
	}

	public enum tVvrxFHOVpLzqIiIrdPzxthuGXVj
	{
		Idle = 0,
		Success = 1,
		Error = 2,
		Waiting = 3,
		CriticalError = 4
	}

	public const int IfJHcbrQCrFFHEUzIIRgaGaXGwzBA = 8;

	private const int GPnDoSoWThHSnlShRcqHwyDeANxp = 10;

	private readonly string IGhwwVimoYBFtjUQDDzpSIVMvbhH;

	private IntPtr sPWJXsqtYIuJLbmDLIEpCuOYdAsEA = lOaGwNFKosAYVzBMPIOzPQUuPhbib.dzMCOZmqXtcKnJjHTkAsIpYupllw;

	private readonly NativeBuffer hqjaEUASFjOXvvoGsKkZpjKAdZUFb;

	private readonly int NdvIisdxpAjyRqtknkxGqVkWbUbAA;

	private readonly lOaGwNFKosAYVzBMPIOzPQUuPhbib.mfKTslDWrfLVgWVDZgojfEoecgmM LtEuzVgMbVgyZSUeGZEbAaBduyzD;

	private readonly object PMzStvVnkxPbSalskbFUNyuMPUUr;

	private readonly object nsXIDZSodhiaPkGVPEUXbAnnjcLu;

	private readonly uint gSXxwrOZBevfedkvWwvRhYkVILhr;

	private NativeOverlapped MmpIodpXGyZPENpSItNNNgXCESyd;

	private YNIJnOblxligSolAcDiwOKeAGKHO ZdanPFxnHlmGrZTSPPoXkTLBTVDk;

	private int kPsIbbLtFfplrRtrnFkAPwbkbluG;

	private bool UBPvWSFEtTaHeqgtVpSKAVKUuCDg;

	private int HVgumHMKyhtoFDfOrxAkuQMTeGXT;

	private int jFvjIeWgRmUqLymwcXYCmoPsDDkP;

	public readonly int BEWqjSVhZoUPUSDBYzhGUpfLLXsk;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	private bool LnsGeYXuVXqxrAqgebBxnSEmFRMR => UuuveWSkAJlBMYJyLPwCBGbjebeK.LnsGeYXuVXqxrAqgebBxnSEmFRMR(IGhwwVimoYBFtjUQDDzpSIVMvbhH);

	public dXlctfYTaIJgQQIMXYbceYWYtRZE(string P_0, int P_1, int P_2)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			throw new ArgumentNullException("devicePath");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("reportLength must be > 0");
		}
		gSXxwrOZBevfedkvWwvRhYkVILhr = ObjectInstanceTracker.Default.Register(this);
		IGhwwVimoYBFtjUQDDzpSIVMvbhH = P_0;
		if (!LrbcmbAyeGqgxlrDnuonahMGrDyg())
		{
			throw new Exception("Could not open HID device.");
		}
		NdvIisdxpAjyRqtknkxGqVkWbUbAA = P_1;
		BEWqjSVhZoUPUSDBYzhGUpfLLXsk = P_1 + 8;
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb = new NativeBuffer(BEWqjSVhZoUPUSDBYzhGUpfLLXsk);
		MmpIodpXGyZPENpSItNNNgXCESyd = default(NativeOverlapped);
		ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.Idle;
		kPsIbbLtFfplrRtrnFkAPwbkbluG = ((P_2 < 0) ? 65535 : P_2);
		PMzStvVnkxPbSalskbFUNyuMPUUr = new object();
		nsXIDZSodhiaPkGVPEUXbAnnjcLu = new object();
		LtEuzVgMbVgyZSUeGZEbAaBduyzD = BIfPRPNnjivaLBYnEotgriHIXQWe;
		QXdiXCMNmtICDXJiLzJcnlEDgLLHA(MmpIodpXGyZPENpSItNNNgXCESyd);
	}

	public tVvrxFHOVpLzqIiIrdPzxthuGXVj pkpJIXUPRvEEdtemqnOqHyayGnzb(byte[] P_0)
	{
		lock (nsXIDZSodhiaPkGVPEUXbAnnjcLu)
		{
			if (NchdYNbKzqsssgcQJdenZuGqXgLo)
			{
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.CriticalError;
			}
			if (!YeMFpJBDeFLGYTaWPyPxxMDJvbaN())
			{
				return (jFvjIeWgRmUqLymwcXYCmoPsDDkP >= 10) ? tVvrxFHOVpLzqIiIrdPzxthuGXVj.CriticalError : tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
			}
			if (P_0 == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (P_0.Length < BEWqjSVhZoUPUSDBYzhGUpfLLXsk)
			{
				throw new Exception("buffer must be at least " + BEWqjSVhZoUPUSDBYzhGUpfLLXsk + " bytes");
			}
			switch (ZdanPFxnHlmGrZTSPPoXkTLBTVDk)
			{
			case YNIJnOblxligSolAcDiwOKeAGKHO.Idle:
				UCrqaJVGRFEzGyTTahvGDkzwBjXM();
				break;
			case YNIJnOblxligSolAcDiwOKeAGKHO.Waiting:
				cFodQSIhHMvErrcNRXYvOGnCNPwDA();
				break;
			case YNIJnOblxligSolAcDiwOKeAGKHO.ErrorPending:
				SixEiWripxzeckOhJGPoiVQKMwgN();
				break;
			case YNIJnOblxligSolAcDiwOKeAGKHO.SuccessPending:
				ALawAtkMCugabgzAelfeDXMpfnpFA();
				break;
			}
			switch (ZdanPFxnHlmGrZTSPPoXkTLBTVDk)
			{
			case YNIJnOblxligSolAcDiwOKeAGKHO.Idle:
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Idle;
			case YNIJnOblxligSolAcDiwOKeAGKHO.Waiting:
			case YNIJnOblxligSolAcDiwOKeAGKHO.ErrorPending:
			case YNIJnOblxligSolAcDiwOKeAGKHO.SuccessPending:
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Waiting;
			case YNIJnOblxligSolAcDiwOKeAGKHO.FinishedSuccess:
				hqjaEUASFjOXvvoGsKkZpjKAdZUFb.TryReadBytes(P_0, BEWqjSVhZoUPUSDBYzhGUpfLLXsk);
				ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.Idle;
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Success;
			case YNIJnOblxligSolAcDiwOKeAGKHO.FinishedError:
				ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.Idle;
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
			default:
				throw new NotImplementedException();
			}
		}
	}

	private bool UCrqaJVGRFEzGyTTahvGDkzwBjXM()
	{
		if (ZdanPFxnHlmGrZTSPPoXkTLBTVDk != YNIJnOblxligSolAcDiwOKeAGKHO.Idle)
		{
			int zdanPFxnHlmGrZTSPPoXkTLBTVDk = (int)ZdanPFxnHlmGrZTSPPoXkTLBTVDk;
			throw new Exception("Cannot StartRead from this state. State = " + zdanPFxnHlmGrZTSPPoXkTLBTVDk);
		}
		try
		{
			woKyyTBPSBeTJzbsBHrQfbqfolrf();
			lock (PMzStvVnkxPbSalskbFUNyuMPUUr)
			{
				bool num = lOaGwNFKosAYVzBMPIOzPQUuPhbib.GqcAgQbDLUOuyCURCmMMTNrKkfyIc(sPWJXsqtYIuJLbmDLIEpCuOYdAsEA, hqjaEUASFjOXvvoGsKkZpjKAdZUFb, (uint)NdvIisdxpAjyRqtknkxGqVkWbUbAA, ref MmpIodpXGyZPENpSItNNNgXCESyd, LtEuzVgMbVgyZSUeGZEbAaBduyzD);
				if (num)
				{
					ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.Waiting;
					UBPvWSFEtTaHeqgtVpSKAVKUuCDg = true;
				}
				else
				{
					veDgSHIODnfvRycKIdJkUgiJzqaLA();
				}
				return num;
			}
		}
		catch (Exception)
		{
			veDgSHIODnfvRycKIdJkUgiJzqaLA();
			return false;
		}
	}

	private void cFodQSIhHMvErrcNRXYvOGnCNPwDA()
	{
		if (ZdanPFxnHlmGrZTSPPoXkTLBTVDk != YNIJnOblxligSolAcDiwOKeAGKHO.Waiting)
		{
			int zdanPFxnHlmGrZTSPPoXkTLBTVDk = (int)ZdanPFxnHlmGrZTSPPoXkTLBTVDk;
			throw new Exception("Cannot CheckReadStatus from this state. State = " + zdanPFxnHlmGrZTSPPoXkTLBTVDk);
		}
		switch (PCPEhvGcVJnWExanxhKLfIyQiEkL())
		{
		case tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error:
			veDgSHIODnfvRycKIdJkUgiJzqaLA();
			break;
		case tVvrxFHOVpLzqIiIrdPzxthuGXVj.Success:
			SbTUMPJTIURfYayZOoNGumrJrrIs();
			break;
		case tVvrxFHOVpLzqIiIrdPzxthuGXVj.Waiting:
			break;
		}
	}

	private tVvrxFHOVpLzqIiIrdPzxthuGXVj PCPEhvGcVJnWExanxhKLfIyQiEkL()
	{
		if (ZdanPFxnHlmGrZTSPPoXkTLBTVDk != YNIJnOblxligSolAcDiwOKeAGKHO.Waiting)
		{
			return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
		}
		try
		{
			switch (lOaGwNFKosAYVzBMPIOzPQUuPhbib.aRjeyjcUarJIeCcYpTjFIowmrRCgA(kPsIbbLtFfplrRtrnFkAPwbkbluG, true))
			{
			case 0u:
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Waiting;
			case 192u:
			{
				if (!lOaGwNFKosAYVzBMPIOzPQUuPhbib.HcrSnZYnzYwrzVrhLMPdUMPwkqOk(sPWJXsqtYIuJLbmDLIEpCuOYdAsEA, ref MmpIodpXGyZPENpSItNNNgXCESyd, out var num, false))
				{
					return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
				}
				return (num > 0) ? tVvrxFHOVpLzqIiIrdPzxthuGXVj.Success : tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
			}
			case uint.MaxValue:
			case 128u:
			case 258u:
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Waiting;
			default:
				return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
			}
		}
		catch
		{
			return tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error;
		}
	}

	private void veDgSHIODnfvRycKIdJkUgiJzqaLA()
	{
		ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.ErrorPending;
		SixEiWripxzeckOhJGPoiVQKMwgN();
	}

	private void SixEiWripxzeckOhJGPoiVQKMwgN()
	{
		if (ZdanPFxnHlmGrZTSPPoXkTLBTVDk != YNIJnOblxligSolAcDiwOKeAGKHO.ErrorPending)
		{
			int zdanPFxnHlmGrZTSPPoXkTLBTVDk = (int)ZdanPFxnHlmGrZTSPPoXkTLBTVDk;
			throw new Exception("Cannot CheckErrorFinished from this state. State = " + zdanPFxnHlmGrZTSPPoXkTLBTVDk);
		}
		ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.FinishedError;
	}

	private void SbTUMPJTIURfYayZOoNGumrJrrIs()
	{
		ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.SuccessPending;
		ALawAtkMCugabgzAelfeDXMpfnpFA();
	}

	private void ALawAtkMCugabgzAelfeDXMpfnpFA()
	{
		if (ZdanPFxnHlmGrZTSPPoXkTLBTVDk != YNIJnOblxligSolAcDiwOKeAGKHO.SuccessPending)
		{
			int zdanPFxnHlmGrZTSPPoXkTLBTVDk = (int)ZdanPFxnHlmGrZTSPPoXkTLBTVDk;
			throw new Exception("Cannot CheckSuccessFinished from this state. State = " + zdanPFxnHlmGrZTSPPoXkTLBTVDk);
		}
		ZdanPFxnHlmGrZTSPPoXkTLBTVDk = YNIJnOblxligSolAcDiwOKeAGKHO.FinishedSuccess;
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb.Write(ReInput.realTime, NdvIisdxpAjyRqtknkxGqVkWbUbAA);
	}

	private void woKyyTBPSBeTJzbsBHrQfbqfolrf()
	{
		QXdiXCMNmtICDXJiLzJcnlEDgLLHA(MmpIodpXGyZPENpSItNNNgXCESyd);
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb.Clear();
		HVgumHMKyhtoFDfOrxAkuQMTeGXT = 0;
		UBPvWSFEtTaHeqgtVpSKAVKUuCDg = false;
	}

	private void QXdiXCMNmtICDXJiLzJcnlEDgLLHA(NativeOverlapped P_0)
	{
		P_0.EventHandle = new IntPtr((int)gSXxwrOZBevfedkvWwvRhYkVILhr);
		P_0.InternalHigh = IntPtr.Zero;
		P_0.InternalLow = IntPtr.Zero;
		P_0.OffsetHigh = 0;
		P_0.OffsetLow = 0;
	}

	private bool YeMFpJBDeFLGYTaWPyPxxMDJvbaN()
	{
		if (jFvjIeWgRmUqLymwcXYCmoPsDDkP >= 10)
		{
			return false;
		}
		if (!LrbcmbAyeGqgxlrDnuonahMGrDyg())
		{
			jFvjIeWgRmUqLymwcXYCmoPsDDkP++;
			return false;
		}
		if (jFvjIeWgRmUqLymwcXYCmoPsDDkP > 0)
		{
			jFvjIeWgRmUqLymwcXYCmoPsDDkP = 0;
		}
		return true;
	}

	private bool LrbcmbAyeGqgxlrDnuonahMGrDyg()
	{
		if (sPWJXsqtYIuJLbmDLIEpCuOYdAsEA != lOaGwNFKosAYVzBMPIOzPQUuPhbib.dzMCOZmqXtcKnJjHTkAsIpYupllw)
		{
			return true;
		}
		if (!LnsGeYXuVXqxrAqgebBxnSEmFRMR)
		{
			return false;
		}
		IntPtr intPtr = ZiApFxdPMXFPWzNilJhIjQcgUpCM.qwikFJGqNmMnDJgUqkvDGKSTBGrbA(IGhwwVimoYBFtjUQDDzpSIVMvbhH, FetReFZeivXTbFKvMetlWJjLivLY.Overlapped, 3221225472u, OfNiIhrbjvtEprDQxlotXkMgtwTD.ShareRead | OfNiIhrbjvtEprDQxlotXkMgtwTD.ShareWrite);
		if (intPtr == lOaGwNFKosAYVzBMPIOzPQUuPhbib.dzMCOZmqXtcKnJjHTkAsIpYupllw)
		{
			return false;
		}
		sPWJXsqtYIuJLbmDLIEpCuOYdAsEA = intPtr;
		return true;
	}

	private void rgKnubiUfXwToPpYzDMZmaZtRRBv()
	{
		if (!(sPWJXsqtYIuJLbmDLIEpCuOYdAsEA == lOaGwNFKosAYVzBMPIOzPQUuPhbib.dzMCOZmqXtcKnJjHTkAsIpYupllw))
		{
			ZiApFxdPMXFPWzNilJhIjQcgUpCM.jGNlyucAgibeWrBEJpvyOjeKfhGKA(sPWJXsqtYIuJLbmDLIEpCuOYdAsEA);
			sPWJXsqtYIuJLbmDLIEpCuOYdAsEA = lOaGwNFKosAYVzBMPIOzPQUuPhbib.dzMCOZmqXtcKnJjHTkAsIpYupllw;
		}
	}

	[MonoPInvokeCallback(typeof(lOaGwNFKosAYVzBMPIOzPQUuPhbib.mfKTslDWrfLVgWVDZgojfEoecgmM))]
	private unsafe static void BIfPRPNnjivaLBYnEotgriHIXQWe(int P_0, int P_1, IntPtr P_2)
	{
		NativeOverlapped* ptr = (NativeOverlapped*)(void*)P_2;
		uint instanceId = (uint)ptr->EventHandle.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<dXlctfYTaIJgQQIMXYbceYWYtRZE>(instanceId, out var instance))
		{
			return;
		}
		lock (instance.PMzStvVnkxPbSalskbFUNyuMPUUr)
		{
			instance.HVgumHMKyhtoFDfOrxAkuQMTeGXT = P_0;
			instance.UBPvWSFEtTaHeqgtVpSKAVKUuCDg = false;
		}
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
		if (NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			return;
		}
		using (new Locker(nsXIDZSodhiaPkGVPEUXbAnnjcLu))
		{
			if (P_0)
			{
				ObjectInstanceTracker.Default.Unregister(gSXxwrOZBevfedkvWwvRhYkVILhr);
			}
			rgKnubiUfXwToPpYzDMZmaZtRRBv();
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	[Conditional("DEBUGTHIS")]
	private void fiqCRXfXOLjBLtgcpEEibmxrqqfz(string P_0)
	{
	}
}
