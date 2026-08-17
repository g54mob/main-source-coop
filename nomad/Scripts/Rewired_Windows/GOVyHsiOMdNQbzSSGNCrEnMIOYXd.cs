using System;
using System.Runtime.InteropServices;
using Rewired;
using Rewired.Utils.Attributes;

internal class GOVyHsiOMdNQbzSSGNCrEnMIOYXd : IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate IntPtr hxGLloukbpztmvrQIQWIyehsVDai(int nCode, IntPtr wParam, IntPtr lParam);

	private struct axjBaYNVezegpweghruckgHrEgEkA
	{
		public IntPtr fCtuowupEdVZBigYSaVDTPLIVBIK;

		public IntPtr NZsIlCyvtakyNZquuitQcriktaFM;

		public uint lzxhkCJAxauleiaRsnAjaRLkqiUf;

		public IntPtr FQMECSpxClJMtImZlaMUaCfIUHrM;
	}

	private static GOVyHsiOMdNQbzSSGNCrEnMIOYXd EFfpMWlUlVdfPnUIsPyGQVYvPWpV;

	private IntPtr USuDAyeekQPNYVaFHywOLaDbLwCAb = IntPtr.Zero;

	private hxGLloukbpztmvrQIQWIyehsVDai zmdwTPTAdXaUqoXwAAHmCpSaKdugb;

	private Action<MfzatynuFTZcaumUqgpvALYfiEpbb, PfnQbhAAztkGebiJJBwStuolfJCF, uint, IntPtr> WZMZndgXLxJsrXPKWfbgWuUuUcGr;

	private byte[] CuByJtBLITzKJcBZSDLIrUiTVNcE;

	private readonly bool KLngryxgWXWKuCVSJQLGsAzSCoYh;

	private axjBaYNVezegpweghruckgHrEgEkA NMogUGEDVaJoPdTdxDnDDhUIHGZj;

	private bool UiAkPuPijImTgwaKWgYQEcWbDaso;

	public GOVyHsiOMdNQbzSSGNCrEnMIOYXd()
	{
		if (EFfpMWlUlVdfPnUIsPyGQVYvPWpV != null)
		{
			throw new Exception("Singleton instance already exists!");
		}
		EFfpMWlUlVdfPnUIsPyGQVYvPWpV = this;
		KLngryxgWXWKuCVSJQLGsAzSCoYh = IntPtr.Size == 8;
		CuByJtBLITzKJcBZSDLIrUiTVNcE = new byte[IntPtr.Size * 3 + 4];
	}

	public void ZaeAONefLBhpRGYekpJmRbMfpOQab(Action<MfzatynuFTZcaumUqgpvALYfiEpbb, PfnQbhAAztkGebiJJBwStuolfJCF, uint, IntPtr> P_0, bool P_1)
	{
		WZMZndgXLxJsrXPKWfbgWuUuUcGr = P_0;
		zmdwTPTAdXaUqoXwAAHmCpSaKdugb = aLuBGZWlcQwFVDyXAiqIQcMeVBtm;
		uint num = 0u;
		if (P_1)
		{
			num = (uint)AppDomain.GetCurrentThreadId();
		}
		USuDAyeekQPNYVaFHywOLaDbLwCAb = aLzwyfyQvOnzHYfwjvxcnYZcJNIy(4, zmdwTPTAdXaUqoXwAAHmCpSaKdugb, IntPtr.Zero, num);
		if (USuDAyeekQPNYVaFHywOLaDbLwCAb == IntPtr.Zero)
		{
			Logger.LogError("SetWindowsHookEx Failed");
		}
	}

	public void vibYlpTDnEFuSKriaghkYPuPYzCl()
	{
		if (!(USuDAyeekQPNYVaFHywOLaDbLwCAb == IntPtr.Zero))
		{
			if (!nPOcbTdtBoipMjdNJQdhymFcgmhdc(USuDAyeekQPNYVaFHywOLaDbLwCAb))
			{
				Logger.LogError("UnhookWindowsHookEx Failed");
			}
			else
			{
				USuDAyeekQPNYVaFHywOLaDbLwCAb = IntPtr.Zero;
			}
		}
	}

	[MonoPInvokeCallback(typeof(hxGLloukbpztmvrQIQWIyehsVDai))]
	private static IntPtr aLuBGZWlcQwFVDyXAiqIQcMeVBtm(int P_0, IntPtr P_1, IntPtr P_2)
	{
		Marshal.Copy(P_2, EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE, 0, EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE.Length);
		int num = 0;
		EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.fCtuowupEdVZBigYSaVDTPLIVBIK = MfzatynuFTZcaumUqgpvALYfiEpbb.gUPJhcGKfXWpuxczBwNYekvTSsyb(MfzatynuFTZcaumUqgpvALYfiEpbb.nsVSFpYykHpmEhuOWHBXrLenwrTS(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE, num));
		num += MfzatynuFTZcaumUqgpvALYfiEpbb.QQQAKQvZqrHpvcOaaZCyIvgHKHGM;
		EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.NZsIlCyvtakyNZquuitQcriktaFM = PfnQbhAAztkGebiJJBwStuolfJCF.vAfzFBfderhYbpTpLcGHnVNjHpdP(PfnQbhAAztkGebiJJBwStuolfJCF.KvqDchPNtDgBpkKtRcTSZfrrhCdE(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE, num));
		num += PfnQbhAAztkGebiJJBwStuolfJCF.zIGwtSxpVwmfhNQkdgSwKTtVCvbu;
		EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.lzxhkCJAxauleiaRsnAjaRLkqiUf = BitConverter.ToUInt32(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE, num);
		num += 4;
		if (EFfpMWlUlVdfPnUIsPyGQVYvPWpV.KLngryxgWXWKuCVSJQLGsAzSCoYh)
		{
			EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.FQMECSpxClJMtImZlaMUaCfIUHrM = new IntPtr(BitConverter.ToInt32(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE, num + 4));
		}
		else
		{
			EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.FQMECSpxClJMtImZlaMUaCfIUHrM = new IntPtr(BitConverter.ToInt32(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.CuByJtBLITzKJcBZSDLIrUiTVNcE, num));
		}
		if (P_0 >= 0)
		{
			EFfpMWlUlVdfPnUIsPyGQVYvPWpV.WZMZndgXLxJsrXPKWfbgWuUuUcGr(MfzatynuFTZcaumUqgpvALYfiEpbb.yXqnlWRVYEFdDzolmhhrejoSQllY(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.fCtuowupEdVZBigYSaVDTPLIVBIK), PfnQbhAAztkGebiJJBwStuolfJCF.WpWHfjulCUuweammBUmNUoEZbQoH(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.NZsIlCyvtakyNZquuitQcriktaFM), EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.lzxhkCJAxauleiaRsnAjaRLkqiUf, EFfpMWlUlVdfPnUIsPyGQVYvPWpV.NMogUGEDVaJoPdTdxDnDDhUIHGZj.FQMECSpxClJMtImZlaMUaCfIUHrM);
		}
		return WOFWGQxEiTNyvdqlECRADHTIUSYk(EFfpMWlUlVdfPnUIsPyGQVYvPWpV.USuDAyeekQPNYVaFHywOLaDbLwCAb, P_0, P_1, P_2);
	}

	public void Dispose()
	{
		QDtblPEgwRcTCgKwSvnUPNakmwuvA(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void hOuOeFrGnQBBDgvmHXNyoDyGfMwTA()
	{
		try
		{
			QDtblPEgwRcTCgKwSvnUPNakmwuvA(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void QDtblPEgwRcTCgKwSvnUPNakmwuvA(bool P_0)
	{
		if (!UiAkPuPijImTgwaKWgYQEcWbDaso)
		{
			vibYlpTDnEFuSKriaghkYPuPYzCl();
			if (EFfpMWlUlVdfPnUIsPyGQVYvPWpV == this)
			{
				EFfpMWlUlVdfPnUIsPyGQVYvPWpV = null;
			}
			UiAkPuPijImTgwaKWgYQEcWbDaso = true;
		}
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "SetWindowsHookEx")]
	private static extern IntPtr aLzwyfyQvOnzHYfwjvxcnYZcJNIy(int P_0, hxGLloukbpztmvrQIQWIyehsVDai P_1, IntPtr P_2, uint P_3);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "UnhookWindowsHookEx")]
	private static extern bool nPOcbTdtBoipMjdNJQdhymFcgmhdc(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "CallNextHookEx")]
	private static extern IntPtr WOFWGQxEiTNyvdqlECRADHTIUSYk(IntPtr P_0, int P_1, IntPtr P_2, IntPtr P_3);
}
