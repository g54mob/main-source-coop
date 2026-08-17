using System;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal sealed class BUlcpPpBEhCFBgRQoJiKmsOlEHseA : IDisposable
{
	public delegate bool AnTBaFTKfcRtoNwxZozJKadpMfSv(aMZqdyjJERTAUbjSZWzzHWVxTEnF report, int timeoutMs);

	private class kSHMsQvFhpFHhieDUaUGzTybMOUFb : IDisposable
	{
		public struct XxZYEvApODnnZPrjCcHCiiChoiaH
		{
			public aMZqdyjJERTAUbjSZWzzHWVxTEnF cCaCDTXnNxSPIiFAEQIPKluuavvs;

			public int BcIvPxPUbzcPUBFrWruAwfoEVXMT;
		}

		private bool njrbcIUcjsDIIKjqvFhECgIEEZSx;

		private XxZYEvApODnnZPrjCcHCiiChoiaH tSIyExygxFOivkwJdjaPMDTpeHXo;

		private NativeBuffer YyqRRiiLUROkfAhJtcqGsfrklUyR;

		private int uDhjIYHPLMxBJJQqjACLTBlsVNAF;

		private bool CVhosdyGSCtptCOTQFWxqRqxdPGF;

		public bool botFzwAfijgEEShsNpjJlVbebOkGb => njrbcIUcjsDIIKjqvFhECgIEEZSx;

		public kSHMsQvFhpFHhieDUaUGzTybMOUFb()
		{
			YyqRRiiLUROkfAhJtcqGsfrklUyR = new NativeBuffer(0);
		}

		public void vVutNSWbzTYhaIWKCIwdSpDdlAzP(ref aMZqdyjJERTAUbjSZWzzHWVxTEnF P_0, int P_1)
		{
			njrbcIUcjsDIIKjqvFhECgIEEZSx = false;
			if (!P_0.PhDXVugvBTbgzVslaZheBDCyjcNL)
			{
				return;
			}
			tSIyExygxFOivkwJdjaPMDTpeHXo = default(XxZYEvApODnnZPrjCcHCiiChoiaH);
			tSIyExygxFOivkwJdjaPMDTpeHXo.cCaCDTXnNxSPIiFAEQIPKluuavvs = P_0;
			tSIyExygxFOivkwJdjaPMDTpeHXo.BcIvPxPUbzcPUBFrWruAwfoEVXMT = P_1;
			if (YyqRRiiLUROkfAhJtcqGsfrklUyR.Length >= P_0.qgutIQhPnXIiQtsjnVldiGCCNOUR || YyqRRiiLUROkfAhJtcqGsfrklUyR.Resize(P_0.qgutIQhPnXIiQtsjnVldiGCCNOUR, preserveData: false))
			{
				try
				{
					YyqRRiiLUROkfAhJtcqGsfrklUyR.Write(P_0.wgtCCiLXCvFMKPuxittDTUaHJTEk, P_0.qgutIQhPnXIiQtsjnVldiGCCNOUR, P_0.qgutIQhPnXIiQtsjnVldiGCCNOUR);
				}
				catch
				{
					return;
				}
				tSIyExygxFOivkwJdjaPMDTpeHXo.cCaCDTXnNxSPIiFAEQIPKluuavvs.wgtCCiLXCvFMKPuxittDTUaHJTEk = YyqRRiiLUROkfAhJtcqGsfrklUyR.Pointer;
				tSIyExygxFOivkwJdjaPMDTpeHXo.cCaCDTXnNxSPIiFAEQIPKluuavvs.qgutIQhPnXIiQtsjnVldiGCCNOUR = YyqRRiiLUROkfAhJtcqGsfrklUyR.Length;
				uDhjIYHPLMxBJJQqjACLTBlsVNAF = P_1;
				njrbcIUcjsDIIKjqvFhECgIEEZSx = true;
			}
		}

		public XxZYEvApODnnZPrjCcHCiiChoiaH eovPhkPahPyQIRFFnxOeXdnsDiC()
		{
			if (!njrbcIUcjsDIIKjqvFhECgIEEZSx)
			{
				return default(XxZYEvApODnnZPrjCcHCiiChoiaH);
			}
			njrbcIUcjsDIIKjqvFhECgIEEZSx = false;
			return tSIyExygxFOivkwJdjaPMDTpeHXo;
		}

		public void DeSsENLrCtTeAeuBBgtLnOonfzdAA()
		{
			tSIyExygxFOivkwJdjaPMDTpeHXo = default(XxZYEvApODnnZPrjCcHCiiChoiaH);
			njrbcIUcjsDIIKjqvFhECgIEEZSx = false;
		}

		public void Dispose()
		{
			YyhxGXdggkDyEkxTSmrBlrJqiJnu(true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		protected virtual void MmVVBjvlDOQbmLihZiANkbzIsGMT()
		{
			try
			{
				YyhxGXdggkDyEkxTSmrBlrJqiJnu(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void YyhxGXdggkDyEkxTSmrBlrJqiJnu(bool P_0)
		{
			if (!CVhosdyGSCtptCOTQFWxqRqxdPGF)
			{
				if (P_0 && YyqRRiiLUROkfAhJtcqGsfrklUyR != null)
				{
					YyqRRiiLUROkfAhJtcqGsfrklUyR.Dispose();
				}
				CVhosdyGSCtptCOTQFWxqRqxdPGF = true;
			}
		}
	}

	private ThreadHelper LpVDJvhoYkRZPDBBFzlepURztMcF;

	private kSHMsQvFhpFHhieDUaUGzTybMOUFb XhGeYIbeJYQcPmjdXhKsoLEajiaS;

	private kSHMsQvFhpFHhieDUaUGzTybMOUFb pmlfUVguGQVpXZZbjsFRDcuVThbw;

	private bool fbnJsBWHkcDotTDShfFefaaLOoWoA;

	private bool bPhEYlhOMoUEZGqIeShgdpiVhkdl;

	private readonly object KLiBVfgzhKOGuCMYhGTplOmTDKEM;

	private AnTBaFTKfcRtoNwxZozJKadpMfSv oeKHuHQcztFIPwniihEbFNBLANbKA;

	private bool mEnYfDssqTJleOSjUDyPbJHPNTvBb;

	public BUlcpPpBEhCFBgRQoJiKmsOlEHseA(AnTBaFTKfcRtoNwxZozJKadpMfSv P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("writeReportDelegate");
		}
		oeKHuHQcztFIPwniihEbFNBLANbKA = P_0;
		XhGeYIbeJYQcPmjdXhKsoLEajiaS = new kSHMsQvFhpFHhieDUaUGzTybMOUFb();
		pmlfUVguGQVpXZZbjsFRDcuVThbw = new kSHMsQvFhpFHhieDUaUGzTybMOUFb();
		KLiBVfgzhKOGuCMYhGTplOmTDKEM = new object();
	}

	public void gOEBDPkviQuIfVylJPFSWvJxSTvl(aMZqdyjJERTAUbjSZWzzHWVxTEnF P_0, int P_1)
	{
		lock (KLiBVfgzhKOGuCMYhGTplOmTDKEM)
		{
			if (mEnYfDssqTJleOSjUDyPbJHPNTvBb || !P_0.PhDXVugvBTbgzVslaZheBDCyjcNL || !VJZulLXEeZRqdYuEwiRPGafpwtMp())
			{
				return;
			}
			lock (XhGeYIbeJYQcPmjdXhKsoLEajiaS)
			{
				XhGeYIbeJYQcPmjdXhKsoLEajiaS.vVutNSWbzTYhaIWKCIwdSpDdlAzP(ref P_0, P_1);
			}
		}
	}

	public void peuYubkkOOBHbhlDbOKVaJCBncsIb()
	{
		if (XhGeYIbeJYQcPmjdXhKsoLEajiaS != null)
		{
			if (pmlfUVguGQVpXZZbjsFRDcuVThbw != null)
			{
				lock (XhGeYIbeJYQcPmjdXhKsoLEajiaS)
				{
					lock (pmlfUVguGQVpXZZbjsFRDcuVThbw)
					{
						XhGeYIbeJYQcPmjdXhKsoLEajiaS.DeSsENLrCtTeAeuBBgtLnOonfzdAA();
						pmlfUVguGQVpXZZbjsFRDcuVThbw.DeSsENLrCtTeAeuBBgtLnOonfzdAA();
						return;
					}
				}
			}
			lock (XhGeYIbeJYQcPmjdXhKsoLEajiaS)
			{
				XhGeYIbeJYQcPmjdXhKsoLEajiaS.DeSsENLrCtTeAeuBBgtLnOonfzdAA();
				return;
			}
		}
		if (pmlfUVguGQVpXZZbjsFRDcuVThbw != null)
		{
			lock (pmlfUVguGQVpXZZbjsFRDcuVThbw)
			{
				pmlfUVguGQVpXZZbjsFRDcuVThbw.DeSsENLrCtTeAeuBBgtLnOonfzdAA();
			}
		}
	}

	private bool VJZulLXEeZRqdYuEwiRPGafpwtMp()
	{
		if (fbnJsBWHkcDotTDShfFefaaLOoWoA)
		{
			return false;
		}
		if (!KePsEZImeNvDbMIMFcXiEPRlykGQ())
		{
			return false;
		}
		if (bPhEYlhOMoUEZGqIeShgdpiVhkdl)
		{
			return true;
		}
		bPhEYlhOMoUEZGqIeShgdpiVhkdl = true;
		return true;
	}

	private bool KePsEZImeNvDbMIMFcXiEPRlykGQ()
	{
		if (fbnJsBWHkcDotTDShfFefaaLOoWoA)
		{
			return false;
		}
		if (LpVDJvhoYkRZPDBBFzlepURztMcF == null)
		{
			try
			{
				LpVDJvhoYkRZPDBBFzlepURztMcF = ThreadHelper.CreateFixedTimeStep(100, 10000);
				LpVDJvhoYkRZPDBBFzlepURztMcF.ThreadUpdateEvent += eRUzOPdERGSRfvtWKQzfeioRWqvx;
				LpVDJvhoYkRZPDBBFzlepURztMcF.ThreadStartedEvent += ABUqCulRjQtoaoEhgasjMYLyGQyp;
				LpVDJvhoYkRZPDBBFzlepURztMcF.ThreadPreStopEvent += KqGaDifGPKOfFKdsXdNJnmXjujxE;
				LpVDJvhoYkRZPDBBFzlepURztMcF.Start(wait: false);
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Exception occurred while creating thread!\n" + ex, requiredThreadSafety: true);
				if (LpVDJvhoYkRZPDBBFzlepURztMcF != null)
				{
					LpVDJvhoYkRZPDBBFzlepURztMcF.Stop(wait: false);
				}
				fbnJsBWHkcDotTDShfFefaaLOoWoA = true;
				return false;
			}
		}
		if (!LpVDJvhoYkRZPDBBFzlepURztMcF.isRunning)
		{
			LpVDJvhoYkRZPDBBFzlepURztMcF.Start(wait: false);
		}
		else
		{
			LpVDJvhoYkRZPDBBFzlepURztMcF.ResetTimeout();
		}
		return true;
	}

	private void kiaQpISXQikLLUaBnSuchykEkcUI()
	{
		lock (XhGeYIbeJYQcPmjdXhKsoLEajiaS)
		{
			lock (pmlfUVguGQVpXZZbjsFRDcuVThbw)
			{
				MiscTools.Swap(ref XhGeYIbeJYQcPmjdXhKsoLEajiaS, ref pmlfUVguGQVpXZZbjsFRDcuVThbw);
			}
		}
	}

	private void ABUqCulRjQtoaoEhgasjMYLyGQyp()
	{
	}

	private void KqGaDifGPKOfFKdsXdNJnmXjujxE()
	{
	}

	private void eRUzOPdERGSRfvtWKQzfeioRWqvx()
	{
		kiaQpISXQikLLUaBnSuchykEkcUI();
		lock (pmlfUVguGQVpXZZbjsFRDcuVThbw)
		{
			if (!pmlfUVguGQVpXZZbjsFRDcuVThbw.botFzwAfijgEEShsNpjJlVbebOkGb)
			{
				return;
			}
			try
			{
				kSHMsQvFhpFHhieDUaUGzTybMOUFb.XxZYEvApODnnZPrjCcHCiiChoiaH xxZYEvApODnnZPrjCcHCiiChoiaH = pmlfUVguGQVpXZZbjsFRDcuVThbw.eovPhkPahPyQIRFFnxOeXdnsDiC();
				oeKHuHQcztFIPwniihEbFNBLANbKA(xxZYEvApODnnZPrjCcHCiiChoiaH.cCaCDTXnNxSPIiFAEQIPKluuavvs, xxZYEvApODnnZPrjCcHCiiChoiaH.BcIvPxPUbzcPUBFrWruAwfoEVXMT);
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception occurred while sending HID output report.\nMessage: " + ex.Message, requiredThreadSafety: true);
			}
		}
	}

	public void Dispose()
	{
		MpHsWZQRZCverTDTnZnJbyDuGvtI(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected void FPtTTsLHjxPesHgXaQqhiJVyfqaW()
	{
		try
		{
			MpHsWZQRZCverTDTnZnJbyDuGvtI(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void MpHsWZQRZCverTDTnZnJbyDuGvtI(bool P_0)
	{
		if (mEnYfDssqTJleOSjUDyPbJHPNTvBb)
		{
			return;
		}
		lock (KLiBVfgzhKOGuCMYhGTplOmTDKEM)
		{
			if (P_0)
			{
				peuYubkkOOBHbhlDbOKVaJCBncsIb();
				if (LpVDJvhoYkRZPDBBFzlepURztMcF != null)
				{
					LpVDJvhoYkRZPDBBFzlepURztMcF.Dispose();
				}
				if (XhGeYIbeJYQcPmjdXhKsoLEajiaS != null)
				{
					XhGeYIbeJYQcPmjdXhKsoLEajiaS.Dispose();
				}
				if (pmlfUVguGQVpXZZbjsFRDcuVThbw != null)
				{
					pmlfUVguGQVpXZZbjsFRDcuVThbw.Dispose();
				}
			}
			mEnYfDssqTJleOSjUDyPbJHPNTvBb = true;
		}
	}
}
