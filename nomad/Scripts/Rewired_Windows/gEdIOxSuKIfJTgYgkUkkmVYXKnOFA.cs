using System;
using Rewired;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal class gEdIOxSuKIfJTgYgkUkkmVYXKnOFA : IDisposable
{
	private enum QpaUrwVqFRIuFtckjGsSHsQBsjBRA
	{
		Idle = 0,
		Waiting = 1,
		ErrorPending = 2,
		FinishedError = 3,
		SuccessPending = 4,
		FinishedSuccess = 5
	}

	public enum RnQCTXuoxpSKPcwrSLLHUuqznWnf
	{
		Idle = 0,
		Success = 1,
		Error = 2,
		Waiting = 3,
		CriticalError = 4
	}

	private readonly string XXoCtuBlUZOxisYqzlVynXGGTncH;

	private IntPtr fsNCmACGXUvzXonKoLpwdoixzhtJ = ilaWjJWdVgCyCNqcioVzdGmptsqD.ZVvdrviZinLLtjrshwCCaNSCQxvY;

	private readonly NativeBuffer pOzVPrZWpYeIGXyMAwLcWVHirhMp;

	private readonly int YecyrzoGmlGVSxiBhJQsvPqsPOFw;

	private readonly ilaWjJWdVgCyCNqcioVzdGmptsqD.VCvIUMXwUYPAsHBQdPjudZWNCoGq PomGVHFAyghOYzuEvmWYZuErSvYQA;

	private readonly object kkDCxWFjyxiffNpKGArOESvBtOIac;

	private readonly uint PQJEYsxSrOzcVkXkMEFQVpCwnvnB;

	private global::yeHxKEAaVPLUZVtJLvgdriEgDazW<ilaWjJWdVgCyCNqcioVzdGmptsqD.kpQpxGFuplWasifMAguxiTqcOTCWA> CVbgNJDVfsklHjyoJVpyYfiwTHHw;

	private QpaUrwVqFRIuFtckjGsSHsQBsjBRA IrVyXvbhCLFXCkbfpKQPiiEvdPls;

	private int WCKFSpswUwrwlEDzwWCFQCmRdpaK;

	private bool fYJxoaddvUIitEdBurKLYpMyGQLo;

	private int LHiCTEwHMSPIbAOsIAoCdPnsEmBd;

	private int bPDfDUJCjWyqtbILVnCtkbPOjnHU;

	public readonly int ChdpfanOEKPfdmLvdIIFdWomXgYJ;

	private bool iIjbRvFVHVtRrVUpOpRPtCSaFwmXA;

	private bool ocnflucelPgIiVoHgoNMIsckGIqBA => VgeWXOWKtHAtHYvQawbKkivsGohCA.jQTOfMalSRQFVJgtRCVfXCZfnOzU(XXoCtuBlUZOxisYqzlVynXGGTncH);

	public gEdIOxSuKIfJTgYgkUkkmVYXKnOFA(string P_0, int P_1, int P_2)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			throw new ArgumentNullException("devicePath");
		}
		if (P_1 <= 0)
		{
			throw new ArgumentOutOfRangeException("reportLength must be > 0");
		}
		PQJEYsxSrOzcVkXkMEFQVpCwnvnB = ObjectInstanceTracker.Default.Register(this);
		XXoCtuBlUZOxisYqzlVynXGGTncH = P_0;
		if (!aMfoHXfNfdRxOpEofTuBtZfKyUN())
		{
			throw new Exception("Could not open HID device.");
		}
		YecyrzoGmlGVSxiBhJQsvPqsPOFw = P_1;
		ChdpfanOEKPfdmLvdIIFdWomXgYJ = P_1 + 8;
		pOzVPrZWpYeIGXyMAwLcWVHirhMp = new NativeBuffer(ChdpfanOEKPfdmLvdIIFdWomXgYJ);
		CVbgNJDVfsklHjyoJVpyYfiwTHHw = new global::yeHxKEAaVPLUZVtJLvgdriEgDazW<ilaWjJWdVgCyCNqcioVzdGmptsqD.kpQpxGFuplWasifMAguxiTqcOTCWA>();
		IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Idle;
		WCKFSpswUwrwlEDzwWCFQCmRdpaK = ((P_2 < 0) ? 65535 : P_2);
		kkDCxWFjyxiffNpKGArOESvBtOIac = new object();
		PomGVHFAyghOYzuEvmWYZuErSvYQA = hdprJiEnjmfPJhawMTErDYJlNPpU;
		RMbGoftKtiqoSjqDBjTRHXMABLLhA();
	}

	public RnQCTXuoxpSKPcwrSLLHUuqznWnf DkPpsvALKIWITZLnuzUMOyGUBLWT(byte[] P_0)
	{
		lock (kkDCxWFjyxiffNpKGArOESvBtOIac)
		{
			if (iIjbRvFVHVtRrVUpOpRPtCSaFwmXA)
			{
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.CriticalError;
			}
			if (!TrwwJuOJGCdYEVvYIhbWsASqWYtA())
			{
				return (bPDfDUJCjWyqtbILVnCtkbPOjnHU >= 10) ? RnQCTXuoxpSKPcwrSLLHUuqznWnf.CriticalError : RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
			}
			if (P_0 == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (P_0.Length < ChdpfanOEKPfdmLvdIIFdWomXgYJ)
			{
				int chdpfanOEKPfdmLvdIIFdWomXgYJ = ChdpfanOEKPfdmLvdIIFdWomXgYJ;
				throw new Exception("buffer must be at least " + chdpfanOEKPfdmLvdIIFdWomXgYJ + " bytes");
			}
			switch (IrVyXvbhCLFXCkbfpKQPiiEvdPls)
			{
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Idle:
				ripTiZqmeBPpnejqciYTAPWMANz();
				break;
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Waiting:
				DEqrDZdbmHACyBenyEnMAMYCdCItb();
				break;
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.ErrorPending:
				OKoeEGcqHjbvTSKvAAuRYMFEPErR();
				break;
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.SuccessPending:
				qoLlGKtDLJyzkjkRbtKDKeGAplBt();
				break;
			}
			switch (IrVyXvbhCLFXCkbfpKQPiiEvdPls)
			{
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Idle:
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Idle;
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Waiting:
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.ErrorPending:
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.SuccessPending:
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Waiting;
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.FinishedSuccess:
				pOzVPrZWpYeIGXyMAwLcWVHirhMp.TryReadBytes(P_0, ChdpfanOEKPfdmLvdIIFdWomXgYJ);
				IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Idle;
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Success;
			case QpaUrwVqFRIuFtckjGsSHsQBsjBRA.FinishedError:
				IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Idle;
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
			default:
				throw new NotImplementedException();
			}
		}
	}

	private bool ripTiZqmeBPpnejqciYTAPWMANz()
	{
		if (IrVyXvbhCLFXCkbfpKQPiiEvdPls != QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Idle)
		{
			int irVyXvbhCLFXCkbfpKQPiiEvdPls = (int)IrVyXvbhCLFXCkbfpKQPiiEvdPls;
			throw new Exception("Cannot StartRead from this state. State = " + irVyXvbhCLFXCkbfpKQPiiEvdPls);
		}
		try
		{
			UodMtbVhGNKncRxrnrEAfBtXwXRA();
			bool num = ilaWjJWdVgCyCNqcioVzdGmptsqD.HKRXFJifQHAPhxjTzIrXuWgfaXFq(fsNCmACGXUvzXonKoLpwdoixzhtJ, pOzVPrZWpYeIGXyMAwLcWVHirhMp, (uint)YecyrzoGmlGVSxiBhJQsvPqsPOFw, XDbjydBaRflbbcCWXMsNJdiGwFsu.ppDCbGJKoMCeiEeyUIfgpJPpsDxF(CVbgNJDVfsklHjyoJVpyYfiwTHHw.kRwcWSbXuaICyiQzWAGAeIWfvqPeb), PomGVHFAyghOYzuEvmWYZuErSvYQA);
			if (num)
			{
				IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Waiting;
				fYJxoaddvUIitEdBurKLYpMyGQLo = true;
			}
			else
			{
				hdmHVbJwwaCbkDpvbtAOkDSIQMQg();
			}
			return num;
		}
		catch (Exception)
		{
			hdmHVbJwwaCbkDpvbtAOkDSIQMQg();
			return false;
		}
	}

	private void DEqrDZdbmHACyBenyEnMAMYCdCItb()
	{
		if (IrVyXvbhCLFXCkbfpKQPiiEvdPls != QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Waiting)
		{
			int irVyXvbhCLFXCkbfpKQPiiEvdPls = (int)IrVyXvbhCLFXCkbfpKQPiiEvdPls;
			throw new Exception("Cannot CheckReadStatus from this state. State = " + irVyXvbhCLFXCkbfpKQPiiEvdPls);
		}
		switch (TsbpXbLOtRhJqLfuxBUcApPmmPGw())
		{
		case RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error:
			hdmHVbJwwaCbkDpvbtAOkDSIQMQg();
			break;
		case RnQCTXuoxpSKPcwrSLLHUuqznWnf.Success:
			AYIGIAPAEqKafpHfpKBezEGgDhBFA();
			break;
		case RnQCTXuoxpSKPcwrSLLHUuqznWnf.Waiting:
			break;
		}
	}

	private RnQCTXuoxpSKPcwrSLLHUuqznWnf TsbpXbLOtRhJqLfuxBUcApPmmPGw()
	{
		if (IrVyXvbhCLFXCkbfpKQPiiEvdPls != QpaUrwVqFRIuFtckjGsSHsQBsjBRA.Waiting)
		{
			return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
		}
		try
		{
			switch (ilaWjJWdVgCyCNqcioVzdGmptsqD.RLPaPDCwSMakReuTSBgVwmkZLPFHb(WCKFSpswUwrwlEDzwWCFQCmRdpaK, true))
			{
			case 0u:
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Waiting;
			case 192u:
			{
				if (!ilaWjJWdVgCyCNqcioVzdGmptsqD.mKmqliwSIAqgiLDmPJHRNsaabYVD(fsNCmACGXUvzXonKoLpwdoixzhtJ, XDbjydBaRflbbcCWXMsNJdiGwFsu.ppDCbGJKoMCeiEeyUIfgpJPpsDxF(CVbgNJDVfsklHjyoJVpyYfiwTHHw.kRwcWSbXuaICyiQzWAGAeIWfvqPeb), out var num, false))
				{
					return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
				}
				return (num > 0) ? RnQCTXuoxpSKPcwrSLLHUuqznWnf.Success : RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
			}
			case 4294967295u:
			case 128u:
			case 258u:
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Waiting;
			default:
				return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
			}
		}
		catch
		{
			return RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error;
		}
	}

	private void hdmHVbJwwaCbkDpvbtAOkDSIQMQg()
	{
		IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.ErrorPending;
		OKoeEGcqHjbvTSKvAAuRYMFEPErR();
	}

	private void OKoeEGcqHjbvTSKvAAuRYMFEPErR()
	{
		if (IrVyXvbhCLFXCkbfpKQPiiEvdPls != QpaUrwVqFRIuFtckjGsSHsQBsjBRA.ErrorPending)
		{
			int irVyXvbhCLFXCkbfpKQPiiEvdPls = (int)IrVyXvbhCLFXCkbfpKQPiiEvdPls;
			throw new Exception("Cannot CheckErrorFinished from this state. State = " + irVyXvbhCLFXCkbfpKQPiiEvdPls);
		}
		IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.FinishedError;
	}

	private void AYIGIAPAEqKafpHfpKBezEGgDhBFA()
	{
		IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.SuccessPending;
		qoLlGKtDLJyzkjkRbtKDKeGAplBt();
	}

	private void qoLlGKtDLJyzkjkRbtKDKeGAplBt()
	{
		if (IrVyXvbhCLFXCkbfpKQPiiEvdPls != QpaUrwVqFRIuFtckjGsSHsQBsjBRA.SuccessPending)
		{
			int irVyXvbhCLFXCkbfpKQPiiEvdPls = (int)IrVyXvbhCLFXCkbfpKQPiiEvdPls;
			throw new Exception("Cannot CheckSuccessFinished from this state. State = " + irVyXvbhCLFXCkbfpKQPiiEvdPls);
		}
		IrVyXvbhCLFXCkbfpKQPiiEvdPls = QpaUrwVqFRIuFtckjGsSHsQBsjBRA.FinishedSuccess;
		pOzVPrZWpYeIGXyMAwLcWVHirhMp.Write(ReInput.realTime, YecyrzoGmlGVSxiBhJQsvPqsPOFw);
	}

	private void UodMtbVhGNKncRxrnrEAfBtXwXRA()
	{
		RMbGoftKtiqoSjqDBjTRHXMABLLhA();
		pOzVPrZWpYeIGXyMAwLcWVHirhMp.Clear();
		LHiCTEwHMSPIbAOsIAoCdPnsEmBd = 0;
		fYJxoaddvUIitEdBurKLYpMyGQLo = false;
	}

	private void RMbGoftKtiqoSjqDBjTRHXMABLLhA()
	{
		ilaWjJWdVgCyCNqcioVzdGmptsqD.kpQpxGFuplWasifMAguxiTqcOTCWA kpQpxGFuplWasifMAguxiTqcOTCWA = default(ilaWjJWdVgCyCNqcioVzdGmptsqD.kpQpxGFuplWasifMAguxiTqcOTCWA);
		kpQpxGFuplWasifMAguxiTqcOTCWA.pYXXtIalWmtDmEteQhqqFLjnBxXG = new IntPtr((int)PQJEYsxSrOzcVkXkMEFQVpCwnvnB);
		kpQpxGFuplWasifMAguxiTqcOTCWA.ZWlqXWHskImfrrXKDAOpolfPNjeY = IntPtr.Zero;
		kpQpxGFuplWasifMAguxiTqcOTCWA.POxJVyIhzDGYoQskoXmmvTKgdMtJ = IntPtr.Zero;
		kpQpxGFuplWasifMAguxiTqcOTCWA.axfHLSayekUJjLMBIecTeGYgEZEl = 0;
		kpQpxGFuplWasifMAguxiTqcOTCWA.pfBjXRFQHHdeXungzYhNULgGFZSBA = 0;
		CVbgNJDVfsklHjyoJVpyYfiwTHHw.YdWtcSRuzXyCzeeQdIauXKhuQdui = kpQpxGFuplWasifMAguxiTqcOTCWA;
	}

	private bool TrwwJuOJGCdYEVvYIhbWsASqWYtA()
	{
		if (bPDfDUJCjWyqtbILVnCtkbPOjnHU >= 10)
		{
			return false;
		}
		if (!aMfoHXfNfdRxOpEofTuBtZfKyUN())
		{
			bPDfDUJCjWyqtbILVnCtkbPOjnHU++;
			return false;
		}
		if (bPDfDUJCjWyqtbILVnCtkbPOjnHU > 0)
		{
			bPDfDUJCjWyqtbILVnCtkbPOjnHU = 0;
		}
		return true;
	}

	private bool aMfoHXfNfdRxOpEofTuBtZfKyUN()
	{
		if (fsNCmACGXUvzXonKoLpwdoixzhtJ != ilaWjJWdVgCyCNqcioVzdGmptsqD.ZVvdrviZinLLtjrshwCCaNSCQxvY)
		{
			return true;
		}
		if (!ocnflucelPgIiVoHgoNMIsckGIqBA)
		{
			return false;
		}
		IntPtr intPtr = EpWcxfdzTRDwZlCGYDLEIMydTvDw.GjWBUUdKHQGSVaxxzFrCiYqzZdKCA(XXoCtuBlUZOxisYqzlVynXGGTncH, WJbYsVLvnfHqqPTFbVttlKnOpHEJ.Overlapped, 3221225472u, DiTuIbfSwjNzcdlrGWHlgucpTlYP.ShareRead | DiTuIbfSwjNzcdlrGWHlgucpTlYP.ShareWrite);
		if (intPtr == ilaWjJWdVgCyCNqcioVzdGmptsqD.ZVvdrviZinLLtjrshwCCaNSCQxvY)
		{
			return false;
		}
		fsNCmACGXUvzXonKoLpwdoixzhtJ = intPtr;
		return true;
	}

	private void gYQRULIWGcyqIwZyTHARyICekZRF()
	{
		if (!(fsNCmACGXUvzXonKoLpwdoixzhtJ == ilaWjJWdVgCyCNqcioVzdGmptsqD.ZVvdrviZinLLtjrshwCCaNSCQxvY))
		{
			EpWcxfdzTRDwZlCGYDLEIMydTvDw.FgHnNobrikPHiXnvFKmDeDfmHUEc(fsNCmACGXUvzXonKoLpwdoixzhtJ);
			fsNCmACGXUvzXonKoLpwdoixzhtJ = ilaWjJWdVgCyCNqcioVzdGmptsqD.ZVvdrviZinLLtjrshwCCaNSCQxvY;
		}
	}

	[MonoPInvokeCallback(typeof(ilaWjJWdVgCyCNqcioVzdGmptsqD.VCvIUMXwUYPAsHBQdPjudZWNCoGq))]
	private static void hdprJiEnjmfPJhawMTErDYJlNPpU(int P_0, int P_1, IntPtr P_2)
	{
	}

	public void Dispose()
	{
		eVnuHoVpNKtdyRowugkAkaolOzmFA(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void epLsCJUZbJSpLTMcKjeqgOStDlBqA()
	{
		try
		{
			eVnuHoVpNKtdyRowugkAkaolOzmFA(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void eVnuHoVpNKtdyRowugkAkaolOzmFA(bool P_0)
	{
		if (iIjbRvFVHVtRrVUpOpRPtCSaFwmXA)
		{
			return;
		}
		using (new Locker(kkDCxWFjyxiffNpKGArOESvBtOIac))
		{
			if (P_0)
			{
				CVbgNJDVfsklHjyoJVpyYfiwTHHw.Dispose();
				ObjectInstanceTracker.Default.Unregister(PQJEYsxSrOzcVkXkMEFQVpCwnvnB);
			}
			gYQRULIWGcyqIwZyTHARyICekZRF();
			iIjbRvFVHVtRrVUpOpRPtCSaFwmXA = true;
		}
	}
}
