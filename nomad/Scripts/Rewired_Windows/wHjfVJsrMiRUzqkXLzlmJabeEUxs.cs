using System.Runtime.InteropServices;

internal class wHjfVJsrMiRUzqkXLzlmJabeEUxs
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int ZbQZJltGNExEPdofBzfnRHXkcpQV(int arg0, void* arg1);

	private static bool QUbFdxESiRJRCakiWuytTQseIZRV;

	private static akrbRtVxdChjDUDDQVgNeGhDwOgV gpvDByXTNodhRgcAnSzwAMRLxJUy;

	private static string XlPMdxtrPOWStlJlcdRIATjvdpTHA;

	private static ZbQZJltGNExEPdofBzfnRHXkcpQV xdiVgkNsWYmcInDaGESEDIqKHcPO;

	private static pKkCzsEckBaUoBcvMDSUkuZdVCzgA eVMQjBDscwenzUPGcQHDtXvkypBI;

	public static bool qGuinsZyvsIwtWKCiMUujAADchchA => QUbFdxESiRJRCakiWuytTQseIZRV;

	public static akrbRtVxdChjDUDDQVgNeGhDwOgV SwAUHbwiWlRhjFcSIvOWVHKYyKUJ => gpvDByXTNodhRgcAnSzwAMRLxJUy;

	public static ZbQZJltGNExEPdofBzfnRHXkcpQV caSmkZlhbthYMNYMgfgArSjdTIYM => xdiVgkNsWYmcInDaGESEDIqKHcPO;

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int SvAAHsDOHDRtkGUYAiwzngTMmIphb(int P_0, void* P_1);

	private unsafe static int oDVVRpAZboKaNhaxJrZptRlHGNHS(int P_0, void* P_1)
	{
		return SvAAHsDOHDRtkGUYAiwzngTMmIphb(P_0, P_1);
	}

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int ZxkDEQleoBRbMGuTndoppcpfmYQE(int P_0, void* P_1);

	private unsafe static int ovCcLhYuKGSjPsrLDtYtXfvYtSQA(int P_0, void* P_1)
	{
		return ZxkDEQleoBRbMGuTndoppcpfmYQE(P_0, P_1);
	}

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int ZQPeTIJPnmbGZFYXHtqWBArvdkFwb(int P_0, void* P_1);

	private unsafe static int AZrbDgbmcCrMwqvIfkNJVeuXugwmA(int P_0, void* P_1)
	{
		return ZQPeTIJPnmbGZFYXHtqWBArvdkFwb(P_0, P_1);
	}

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int uzyQdCMtycFXtunRtXrBxHmJbKHc(int P_0, void* P_1);

	private unsafe static int VhISQFgMAWqTmkDBHStaZRuEnLwC(int P_0, void* P_1)
	{
		return uzyQdCMtycFXtunRtXrBxHmJbKHc(P_0, P_1);
	}

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int cETRocoVxLpKSxSOQFBrHLHOWRSO(int P_0, void* P_1);

	private unsafe static int UidaqZilIqWQdvCohmNpZQuqBRGVA(int P_0, void* P_1)
	{
		return cETRocoVxLpKSxSOQFBrHLHOWRSO(P_0, P_1);
	}

	public static bool tmSPMeeaNrAIkoZQOoVxEotHEtqiA(out akrbRtVxdChjDUDDQVgNeGhDwOgV P_0, out string P_1, out int P_2)
	{
		P_2 = 0;
		P_1 = "None";
		P_0 = akrbRtVxdChjDUDDQVgNeGhDwOgV.None;
		QUbFdxESiRJRCakiWuytTQseIZRV = false;
		xdiVgkNsWYmcInDaGESEDIqKHcPO = null;
		if (dALsVrQEHoDiUBIENqiXTssdgAtI())
		{
			gpvDByXTNodhRgcAnSzwAMRLxJUy = akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_4;
			XlPMdxtrPOWStlJlcdRIATjvdpTHA = "Xinput1_4.dll";
		}
		else if (jNrHpeuEqwWPmoHAJawjDZdHejLg())
		{
			gpvDByXTNodhRgcAnSzwAMRLxJUy = akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_3;
			XlPMdxtrPOWStlJlcdRIATjvdpTHA = "Xinput1_3.dll";
		}
		else if (LhstFaImbXHfASnBzVNdkfuzMjNC())
		{
			gpvDByXTNodhRgcAnSzwAMRLxJUy = akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_2;
			XlPMdxtrPOWStlJlcdRIATjvdpTHA = "Xinput1_2.dll";
		}
		else if (IJAOBFOTueAkvHxEnfoqNkxJqQdI())
		{
			gpvDByXTNodhRgcAnSzwAMRLxJUy = akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_1;
			XlPMdxtrPOWStlJlcdRIATjvdpTHA = "Xinput1_1.dll";
		}
		else
		{
			if (!lwbagVkAKhoTzbEKzHyBaCfFvoAWA())
			{
				P_2 = 1;
				return false;
			}
			gpvDByXTNodhRgcAnSzwAMRLxJUy = akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_9_1_0;
			XlPMdxtrPOWStlJlcdRIATjvdpTHA = "Xinput9_1_0.dll";
		}
		P_1 = XlPMdxtrPOWStlJlcdRIATjvdpTHA;
		P_0 = gpvDByXTNodhRgcAnSzwAMRLxJUy;
		if (QUbFdxESiRJRCakiWuytTQseIZRV && !ccaJCvDffZDMBjLWQLcaDdkHygpSb())
		{
			QUbFdxESiRJRCakiWuytTQseIZRV = false;
		}
		if (!HiCgZPaBCckomOwkybjAkHqbabRjb())
		{
			acMqKyPUxPmXOkMIsTwChGHaXiWn();
			return false;
		}
		return true;
	}

	private unsafe static bool dALsVrQEHoDiUBIENqiXTssdgAtI()
	{
		try
		{
			fixed (pKkCzsEckBaUoBcvMDSUkuZdVCzgA* ptr = &eVMQjBDscwenzUPGcQHDtXvkypBI)
			{
				void* ptr2 = ptr;
				UidaqZilIqWQdvCohmNpZQuqBRGVA(255, ptr2);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private unsafe static bool jNrHpeuEqwWPmoHAJawjDZdHejLg()
	{
		try
		{
			fixed (pKkCzsEckBaUoBcvMDSUkuZdVCzgA* ptr = &eVMQjBDscwenzUPGcQHDtXvkypBI)
			{
				void* ptr2 = ptr;
				VhISQFgMAWqTmkDBHStaZRuEnLwC(255, ptr2);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private unsafe static bool LhstFaImbXHfASnBzVNdkfuzMjNC()
	{
		try
		{
			fixed (pKkCzsEckBaUoBcvMDSUkuZdVCzgA* ptr = &eVMQjBDscwenzUPGcQHDtXvkypBI)
			{
				void* ptr2 = ptr;
				AZrbDgbmcCrMwqvIfkNJVeuXugwmA(255, ptr2);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private unsafe static bool IJAOBFOTueAkvHxEnfoqNkxJqQdI()
	{
		try
		{
			fixed (pKkCzsEckBaUoBcvMDSUkuZdVCzgA* ptr = &eVMQjBDscwenzUPGcQHDtXvkypBI)
			{
				void* ptr2 = ptr;
				ovCcLhYuKGSjPsrLDtYtXfvYtSQA(255, ptr2);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private unsafe static bool lwbagVkAKhoTzbEKzHyBaCfFvoAWA()
	{
		try
		{
			fixed (pKkCzsEckBaUoBcvMDSUkuZdVCzgA* ptr = &eVMQjBDscwenzUPGcQHDtXvkypBI)
			{
				void* ptr2 = ptr;
				oDVVRpAZboKaNhaxJrZptRlHGNHS(255, ptr2);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static bool ccaJCvDffZDMBjLWQLcaDdkHygpSb()
	{
		_ = QUbFdxESiRJRCakiWuytTQseIZRV;
		return false;
	}

	private static bool HiCgZPaBCckomOwkybjAkHqbabRjb()
	{
		try
		{
			_ = new NdFZhsSzsZDhsLnhJcJIaVPkCbZU().vzdAOjvBqovFbmLHSfIMgBZObqxH;
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void acMqKyPUxPmXOkMIsTwChGHaXiWn()
	{
		if (QUbFdxESiRJRCakiWuytTQseIZRV)
		{
			xdiVgkNsWYmcInDaGESEDIqKHcPO = null;
		}
	}
}
