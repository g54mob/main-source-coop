using System;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class dVTqWOZMXYmqKBirNLNYnAJtvPUC : IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate IntPtr MSobiRKoTnfGzmbTNoGRrqUwiHSl(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct fhGixpiDWyjycuiNANXgLJQSjsve
	{
		public uint WkOzVBhhCWOyIigDrxSkoDfqkuRL;

		public IntPtr yqaCtOWzZGXknngQreadQLqKKOin;

		public int aGZiaDFUXyeCGyRlZmjrcgjVKRvI;

		public int sToaPphkpDCNsJviwwAUNDOZmgHCA;

		public IntPtr PHFARKGTelMlqazMHtsnUCCtobGLB;

		public IntPtr CzWuMfejwyMCTePkEcBkmuwoDkZG;

		public IntPtr epGvWHwsxJJagTOePJPTykhxpelK;

		public IntPtr SJoeIWLSoHQrwAYhwHZMhBtOZVvM;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string AhPUAENaHjjqUektgePrOlQSghFLA;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string LPlQnrvrAsDpyJcSwTbrQrtkWLBi;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct WgShjtxFwnEfOZRCRHvFBdwbTHxR
	{
		public IntPtr uiQMUoAGwrTuwBgSrDEkbOVZjfWwA;

		public IntPtr xBEEOICKVGWscDAVafSxprqLLJRxA;

		public IntPtr YYwyQwzawVAYgfUEoiBChpiBqcRgc;

		public IntPtr wxAmEmUYFioMVZqpUNIsPBIdZwMG;

		public int tGeAUCFcbjttCKDELCFpIgdmShkk;

		public int MvOLKmGxSYazDZCRDaKLfiKUmsFcA;

		public int nKAZeWLCGCaWqffRGEmzNUoLlhkcb;

		public int EBUyJmnIJqxcZPtRUdijRpXeziQh;

		public int GmwQqEnSKElDJqJCMedddFoYYysZ;

		public IntPtr VmSWEUOfksOMsKFMgmntvIHakRnH;

		public IntPtr OVvxTkYNCnMqEQwsnjjTwIVqqVvK;

		public uint VLMVeIHUQVfYDkfrKxScJewccuCCA;
	}

	private readonly ushort QtvltABasSkVWlLMTUIcIoZFzstP;

	private readonly string VICcxeGCdmliNWsJFvCswncgSzim;

	private bool nBcOhDVgGHgUDgPZtmEIfpHEsutN;

	private IntPtr GFuZrOtMHZqSkfMeofkAWFukhSrR;

	private int jeOGvJUIfojeUaZIEXCJiQubHpKcb;

	private uint vOtyTmiEdMZldQLZavdWWvsBDewK;

	private MSobiRKoTnfGzmbTNoGRrqUwiHSl kXuYuTFlrAYtfuxVmiwjqCszIblk;

	private MSobiRKoTnfGzmbTNoGRrqUwiHSl bySXUbJCcapxjqsemiJmisdjUbPC;

	public IntPtr qqeNEhvZzMQkqjFUFQSmwOmLiuvb => GFuZrOtMHZqSkfMeofkAWFukhSrR;

	[DllImport("user32.dll", EntryPoint = "RegisterClassW", SetLastError = true)]
	private static extern ushort UtZNCunFEBITIaIulcSYHUhjYCwAA([In] ref fhGixpiDWyjycuiNANXgLJQSjsve P_0);

	[DllImport("user32.dll", EntryPoint = "UnregisterClassW", SetLastError = true)]
	private static extern bool qKkLilbGetuEAseHalkszmJZwgyI([MarshalAs(UnmanagedType.LPWStr)] string P_0, IntPtr P_1);

	[DllImport("user32.dll", EntryPoint = "CreateWindowExW", SetLastError = true)]
	private static extern IntPtr CyyAqXUOOuzPNIHTNgTrmqcfbryEA(uint P_0, [MarshalAs(UnmanagedType.LPWStr)] string P_1, [MarshalAs(UnmanagedType.LPWStr)] string P_2, uint P_3, int P_4, int P_5, int P_6, int P_7, IntPtr P_8, IntPtr P_9, IntPtr P_10, IntPtr P_11);

	[DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true)]
	private static extern IntPtr AvEGZSiElZOgRKYKGOMnsbEOQouRA(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("user32.dll", EntryPoint = "DestroyWindow", SetLastError = true)]
	private static extern bool GKiwhWCfbiWILkBUEGxmljlsVlid(IntPtr P_0);

	public void Dispose()
	{
		wFcGqGsUXtvSsNLOOtceFWBkuPkJ(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void LjgsPmmsGGCRRWqyOAKxpYHtdvDN()
	{
		try
		{
			wFcGqGsUXtvSsNLOOtceFWBkuPkJ(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void wFcGqGsUXtvSsNLOOtceFWBkuPkJ(bool P_0)
	{
		if (!nBcOhDVgGHgUDgPZtmEIfpHEsutN)
		{
			if (P_0)
			{
				ObjectInstanceTracker.Default.Unregister(vOtyTmiEdMZldQLZavdWWvsBDewK);
			}
			if (GFuZrOtMHZqSkfMeofkAWFukhSrR != IntPtr.Zero)
			{
				GKiwhWCfbiWILkBUEGxmljlsVlid(GFuZrOtMHZqSkfMeofkAWFukhSrR);
				GFuZrOtMHZqSkfMeofkAWFukhSrR = IntPtr.Zero;
			}
			if (QtvltABasSkVWlLMTUIcIoZFzstP != 0 && !string.IsNullOrEmpty(VICcxeGCdmliNWsJFvCswncgSzim))
			{
				qKkLilbGetuEAseHalkszmJZwgyI(VICcxeGCdmliNWsJFvCswncgSzim, IntPtr.Zero);
			}
			nBcOhDVgGHgUDgPZtmEIfpHEsutN = true;
		}
	}

	public dVTqWOZMXYmqKBirNLNYnAJtvPUC(string P_0, bool P_1, MSobiRKoTnfGzmbTNoGRrqUwiHSl P_2)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			throw new ArgumentNullException("className");
		}
		if (P_2 == null)
		{
			throw new ArgumentNullException("staticCustomWndProcDelegate");
		}
		vOtyTmiEdMZldQLZavdWWvsBDewK = ObjectInstanceTracker.Default.Register(this);
		VICcxeGCdmliNWsJFvCswncgSzim = P_0;
		kXuYuTFlrAYtfuxVmiwjqCszIblk = BVyscUlsqOIjNCPCXENDXSnfCMvKA;
		bySXUbJCcapxjqsemiJmisdjUbPC = P_2;
		jeOGvJUIfojeUaZIEXCJiQubHpKcb = 0;
		fhGixpiDWyjycuiNANXgLJQSjsve fhGixpiDWyjycuiNANXgLJQSjsve2 = new fhGixpiDWyjycuiNANXgLJQSjsve
		{
			yqaCtOWzZGXknngQreadQLqKKOin = Marshal.GetFunctionPointerForDelegate(kXuYuTFlrAYtfuxVmiwjqCszIblk)
		};
		while (QtvltABasSkVWlLMTUIcIoZFzstP == 0 && jeOGvJUIfojeUaZIEXCJiQubHpKcb < 20)
		{
			fhGixpiDWyjycuiNANXgLJQSjsve2.LPlQnrvrAsDpyJcSwTbrQrtkWLBi = P_0;
			QtvltABasSkVWlLMTUIcIoZFzstP = UtZNCunFEBITIaIulcSYHUhjYCwAA(ref fhGixpiDWyjycuiNANXgLJQSjsve2);
			if (QtvltABasSkVWlLMTUIcIoZFzstP != 0)
			{
				break;
			}
			jeOGvJUIfojeUaZIEXCJiQubHpKcb++;
			P_0 = VICcxeGCdmliNWsJFvCswncgSzim + jeOGvJUIfojeUaZIEXCJiQubHpKcb;
		}
		if (QtvltABasSkVWlLMTUIcIoZFzstP == 0)
		{
			throw new Exception("Could not register window class!");
		}
		if (VICcxeGCdmliNWsJFvCswncgSzim != P_0)
		{
			VICcxeGCdmliNWsJFvCswncgSzim = P_0;
		}
		if (P_1)
		{
			GFuZrOtMHZqSkfMeofkAWFukhSrR = GfSAxsIwMvEaxbzpXmxmncvuQavI(P_0, new IntPtr((int)vOtyTmiEdMZldQLZavdWWvsBDewK));
		}
		else
		{
			GFuZrOtMHZqSkfMeofkAWFukhSrR = XVvhYwtjeBKAsujBAIJMYQzLQIxn(P_0, new IntPtr((int)vOtyTmiEdMZldQLZavdWWvsBDewK));
		}
	}

	private IntPtr XVvhYwtjeBKAsujBAIJMYQzLQIxn(string P_0, IntPtr P_1)
	{
		return CyyAqXUOOuzPNIHTNgTrmqcfbryEA(0u, P_0, string.Empty, 0u, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, P_1);
	}

	private IntPtr GfSAxsIwMvEaxbzpXmxmncvuQavI(string P_0, IntPtr P_1)
	{
		return CyyAqXUOOuzPNIHTNgTrmqcfbryEA(0u, P_0, string.Empty, 0u, 0, 0, 0, 0, gbkwoNGXbnemmwRJGohrxZfefZAp.yLvFwdJmrMYYyLYNSlEcAztLuTuE, IntPtr.Zero, IntPtr.Zero, P_1);
	}

	[MonoPInvokeCallback(typeof(MSobiRKoTnfGzmbTNoGRrqUwiHSl))]
	private unsafe static IntPtr BVyscUlsqOIjNCPCXENDXSnfCMvKA(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3)
	{
		if (P_0 == IntPtr.Zero)
		{
			return AvEGZSiElZOgRKYKGOMnsbEOQouRA(P_0, P_1, P_2, P_3);
		}
		bool flag = false;
		uint instanceId = 0u;
		if (P_1 == 1)
		{
			WgShjtxFwnEfOZRCRHvFBdwbTHxR* ptr = (WgShjtxFwnEfOZRCRHvFBdwbTHxR*)(void*)P_3;
			if (ptr->uiQMUoAGwrTuwBgSrDEkbOVZjfWwA != IntPtr.Zero)
			{
				KQKvYsAXvDlLWOZXkMKdMDaTTekW.ycRDdmfaaIQpstrpIFFQSOTVxbGv(P_0, -21, ptr->uiQMUoAGwrTuwBgSrDEkbOVZjfWwA);
			}
		}
		else
		{
			instanceId = (uint)KQKvYsAXvDlLWOZXkMKdMDaTTekW.vxQmmxwilIJheBcwiXxHghIgwKFs(P_0, -21).ToInt32();
			flag = true;
		}
		if (flag && ObjectInstanceTracker.Default.TryGetInstance<dVTqWOZMXYmqKBirNLNYnAJtvPUC>(instanceId, out var instance))
		{
			instance.bySXUbJCcapxjqsemiJmisdjUbPC(P_0, P_1, P_2, P_3);
		}
		return AvEGZSiElZOgRKYKGOMnsbEOQouRA(P_0, P_1, P_2, P_3);
	}

	public void MbMVPTZuzKvQpZbnyBlsxPvSMUBt(MSobiRKoTnfGzmbTNoGRrqUwiHSl P_0)
	{
		bySXUbJCcapxjqsemiJmisdjUbPC = P_0;
	}
}
