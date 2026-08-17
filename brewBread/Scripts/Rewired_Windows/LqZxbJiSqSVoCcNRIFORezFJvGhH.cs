using System;
using System.Runtime.InteropServices;

internal class LqZxbJiSqSVoCcNRIFORezFJvGhH : IDisposable
{
	internal enum tJuHNqAMIvBRyDFYsKPrckfDYgxCB
	{
		Current = 0,
		All = 1
	}

	private delegate IntPtr wJQQTIaCtPKEcauylOJSWGsLNjiU(int nCode, IntPtr wParam, IntPtr lParam);

	private const int aJMDQXegEYOSgMyFiXYmEhqmfbHV = 4;

	private IntPtr yztUpSKtNhWbezdXxrdcJxjSCrnn = IntPtr.Zero;

	private wJQQTIaCtPKEcauylOJSWGsLNjiU hknXfqffnowkzUFNwXCnSsBuRzvP;

	private Action<IntPtr, IntPtr, uint, uint> FyfuAiKFdYiicnFpcZGQvkwNRBGH;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public void YIkNozGFoHJibqeYvJMWxfAcCtxK(Action<IntPtr, IntPtr, uint, uint> P_0, tJuHNqAMIvBRyDFYsKPrckfDYgxCB P_1)
	{
		FyfuAiKFdYiicnFpcZGQvkwNRBGH = P_0;
		hknXfqffnowkzUFNwXCnSsBuRzvP = DuUEbbeKXoRlNLVHXudukmeViWCVA;
		uint num = 0u;
		if (P_1 == tJuHNqAMIvBRyDFYsKPrckfDYgxCB.Current)
		{
			num = (uint)AppDomain.GetCurrentThreadId();
		}
		yztUpSKtNhWbezdXxrdcJxjSCrnn = bojGwmInlzxKjbVkPrDRRAjOpuSm(4, hknXfqffnowkzUFNwXCnSsBuRzvP, IntPtr.Zero, num);
		_ = yztUpSKtNhWbezdXxrdcJxjSCrnn == IntPtr.Zero;
	}

	public void ZLbTBPOXATkWPrJKPKzMXNCodYqT()
	{
		if (!(yztUpSKtNhWbezdXxrdcJxjSCrnn == IntPtr.Zero) && wSCPPmwMMpmSPwTOtUCTPorAbRYA(yztUpSKtNhWbezdXxrdcJxjSCrnn))
		{
			yztUpSKtNhWbezdXxrdcJxjSCrnn = IntPtr.Zero;
		}
	}

	private IntPtr DuUEbbeKXoRlNLVHXudukmeViWCVA(int P_0, IntPtr P_1, IntPtr P_2)
	{
		if (P_0 >= 0)
		{
			int num = 0;
			IntPtr arg = Marshal.ReadIntPtr(P_2, num);
			num += IntPtr.Size;
			IntPtr arg2 = Marshal.ReadIntPtr(P_2, num);
			num += IntPtr.Size;
			uint arg3 = (uint)Marshal.ReadInt32(P_2, num);
			num += 4;
			if (IntPtr.Size == 8)
			{
				num += 4;
			}
			uint arg4 = (uint)Marshal.ReadInt32(P_2, num);
			FyfuAiKFdYiicnFpcZGQvkwNRBGH(arg, arg2, arg3, arg4);
		}
		return PizDcvqGeVNUcvwZRmiincYYlAeL(yztUpSKtNhWbezdXxrdcJxjSCrnn, P_0, P_1, P_2);
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
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "SetWindowsHookEx")]
	private static extern IntPtr bojGwmInlzxKjbVkPrDRRAjOpuSm(int P_0, wJQQTIaCtPKEcauylOJSWGsLNjiU P_1, IntPtr P_2, uint P_3);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "UnhookWindowsHookEx")]
	private static extern bool wSCPPmwMMpmSPwTOtUCTPorAbRYA(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "CallNextHookEx")]
	private static extern IntPtr PizDcvqGeVNUcvwZRmiincYYlAeL(IntPtr P_0, int P_1, IntPtr P_2, IntPtr P_3);
}
