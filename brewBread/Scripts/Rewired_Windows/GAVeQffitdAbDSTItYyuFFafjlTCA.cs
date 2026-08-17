using System;
using System.Runtime.InteropServices;
using Rewired;
using Rewired.Utils.Attributes;

internal class GAVeQffitdAbDSTItYyuFFafjlTCA : IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate IntPtr xEDbgLbwlRXBgkNzbdtegckMhifLA(int nCode, IntPtr wParam, IntPtr lParam);

	private struct LngEnfgFYCOYWPfRhTgYjtaXMKNlA
	{
		public IntPtr egIBtUeyNfDDMIkXIeDLygYOBzwK;

		public IntPtr dlDwVVEpZaDMhqKcAZjssMNgfOtN;

		public uint tpJAJObRzzfXTANTIasbEoNJEAwPb;

		public IntPtr HZxPlvGkyKbWHOmuJEMoIEeKfktAb;
	}

	private const int aJMDQXegEYOSgMyFiXYmEhqmfbHV = 4;

	private static GAVeQffitdAbDSTItYyuFFafjlTCA CUMjiYgOsfRNPymJKKmVCqeHmAEM;

	private IntPtr yztUpSKtNhWbezdXxrdcJxjSCrnn = IntPtr.Zero;

	private xEDbgLbwlRXBgkNzbdtegckMhifLA hknXfqffnowkzUFNwXCnSsBuRzvP;

	private Action<LMLzRMaHqCQoIFicCcuNJBqWigbe, uomYrxyUvdwJTSvjuZHYxDEFgzWT, uint, IntPtr> FyfuAiKFdYiicnFpcZGQvkwNRBGH;

	private byte[] WMfyuTrAUZFONYQuASTVuHCbjGQaA;

	private readonly bool pcaCUIfFnGbEZJMlRSwWCANluoWDA;

	private LngEnfgFYCOYWPfRhTgYjtaXMKNlA AfnOYuypDIsKuvFNeicqhxjiBhsgA;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public GAVeQffitdAbDSTItYyuFFafjlTCA()
	{
		if (CUMjiYgOsfRNPymJKKmVCqeHmAEM != null)
		{
			throw new Exception("Singleton instance already exists!");
		}
		CUMjiYgOsfRNPymJKKmVCqeHmAEM = this;
		pcaCUIfFnGbEZJMlRSwWCANluoWDA = IntPtr.Size == 8;
		WMfyuTrAUZFONYQuASTVuHCbjGQaA = new byte[IntPtr.Size * 3 + 4];
	}

	public void YIkNozGFoHJibqeYvJMWxfAcCtxK(Action<LMLzRMaHqCQoIFicCcuNJBqWigbe, uomYrxyUvdwJTSvjuZHYxDEFgzWT, uint, IntPtr> P_0, bool P_1)
	{
		FyfuAiKFdYiicnFpcZGQvkwNRBGH = P_0;
		hknXfqffnowkzUFNwXCnSsBuRzvP = DuUEbbeKXoRlNLVHXudukmeViWCVA;
		uint num = 0u;
		if (P_1)
		{
			num = (uint)AppDomain.GetCurrentThreadId();
		}
		yztUpSKtNhWbezdXxrdcJxjSCrnn = bojGwmInlzxKjbVkPrDRRAjOpuSm(4, hknXfqffnowkzUFNwXCnSsBuRzvP, IntPtr.Zero, num);
		if (yztUpSKtNhWbezdXxrdcJxjSCrnn == IntPtr.Zero)
		{
			Logger.LogError("SetWindowsHookEx Failed");
		}
	}

	public void ZLbTBPOXATkWPrJKPKzMXNCodYqT()
	{
		if (!(yztUpSKtNhWbezdXxrdcJxjSCrnn == IntPtr.Zero))
		{
			if (!wSCPPmwMMpmSPwTOtUCTPorAbRYA(yztUpSKtNhWbezdXxrdcJxjSCrnn))
			{
				Logger.LogError("UnhookWindowsHookEx Failed");
			}
			else
			{
				yztUpSKtNhWbezdXxrdcJxjSCrnn = IntPtr.Zero;
			}
		}
	}

	[MonoPInvokeCallback(typeof(xEDbgLbwlRXBgkNzbdtegckMhifLA))]
	private static IntPtr DuUEbbeKXoRlNLVHXudukmeViWCVA(int P_0, IntPtr P_1, IntPtr P_2)
	{
		Marshal.Copy(P_2, CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA, 0, CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA.Length);
		int num = 0;
		CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.egIBtUeyNfDDMIkXIeDLygYOBzwK = LMLzRMaHqCQoIFicCcuNJBqWigbe.zsNtDbJRpaGeYqGorVKjcaHZirNu(LMLzRMaHqCQoIFicCcuNJBqWigbe.xkNfVhJUDncENtYwwMvLymmXVmAC(CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA, num));
		num += LMLzRMaHqCQoIFicCcuNJBqWigbe.ZHEQuImTVvNEkKAXcGUIvuGvaloA;
		CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.dlDwVVEpZaDMhqKcAZjssMNgfOtN = uomYrxyUvdwJTSvjuZHYxDEFgzWT.zsNtDbJRpaGeYqGorVKjcaHZirNu(uomYrxyUvdwJTSvjuZHYxDEFgzWT.xkNfVhJUDncENtYwwMvLymmXVmAC(CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA, num));
		num += uomYrxyUvdwJTSvjuZHYxDEFgzWT.ZHEQuImTVvNEkKAXcGUIvuGvaloA;
		CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.tpJAJObRzzfXTANTIasbEoNJEAwPb = BitConverter.ToUInt32(CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA, num);
		num += 4;
		if (CUMjiYgOsfRNPymJKKmVCqeHmAEM.pcaCUIfFnGbEZJMlRSwWCANluoWDA)
		{
			CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.HZxPlvGkyKbWHOmuJEMoIEeKfktAb = new IntPtr(BitConverter.ToInt32(CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA, num + 4));
		}
		else
		{
			CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.HZxPlvGkyKbWHOmuJEMoIEeKfktAb = new IntPtr(BitConverter.ToInt32(CUMjiYgOsfRNPymJKKmVCqeHmAEM.WMfyuTrAUZFONYQuASTVuHCbjGQaA, num));
		}
		if (P_0 >= 0)
		{
			CUMjiYgOsfRNPymJKKmVCqeHmAEM.FyfuAiKFdYiicnFpcZGQvkwNRBGH(LMLzRMaHqCQoIFicCcuNJBqWigbe.zsNtDbJRpaGeYqGorVKjcaHZirNu(CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.egIBtUeyNfDDMIkXIeDLygYOBzwK), uomYrxyUvdwJTSvjuZHYxDEFgzWT.zsNtDbJRpaGeYqGorVKjcaHZirNu(CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.dlDwVVEpZaDMhqKcAZjssMNgfOtN), CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.tpJAJObRzzfXTANTIasbEoNJEAwPb, CUMjiYgOsfRNPymJKKmVCqeHmAEM.AfnOYuypDIsKuvFNeicqhxjiBhsgA.HZxPlvGkyKbWHOmuJEMoIEeKfktAb);
		}
		return PizDcvqGeVNUcvwZRmiincYYlAeL(CUMjiYgOsfRNPymJKKmVCqeHmAEM.yztUpSKtNhWbezdXxrdcJxjSCrnn, P_0, P_1, P_2);
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
		if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			ZLbTBPOXATkWPrJKPKzMXNCodYqT();
			if (CUMjiYgOsfRNPymJKKmVCqeHmAEM == this)
			{
				CUMjiYgOsfRNPymJKKmVCqeHmAEM = null;
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "SetWindowsHookEx")]
	private static extern IntPtr bojGwmInlzxKjbVkPrDRRAjOpuSm(int P_0, xEDbgLbwlRXBgkNzbdtegckMhifLA P_1, IntPtr P_2, uint P_3);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "UnhookWindowsHookEx")]
	private static extern bool wSCPPmwMMpmSPwTOtUCTPorAbRYA(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "CallNextHookEx")]
	private static extern IntPtr PizDcvqGeVNUcvwZRmiincYYlAeL(IntPtr P_0, int P_1, IntPtr P_2, IntPtr P_3);
}
