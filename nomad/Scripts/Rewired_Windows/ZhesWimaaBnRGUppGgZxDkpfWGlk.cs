using System;
using Rewired;

internal static class ZhesWimaaBnRGUppGgZxDkpfWGlk
{
	public enum BIWpfgzlfFhchCyefSgfuVQEUlJOA
	{
		None = 0,
		CombinedTriggers = 1,
		SplitTriggers = 2
	}

	private struct dAajQFjvtIJBPiHxvEehIiWIoHbU
	{
		public enum vvVeZbxtpMNoGBzBKvHiSYZiBuGT
		{
			MatchAnyOption = 0,
			MatchAllOptions = 1,
			IgnoreOptions = 2
		}

		public PidVid ijmjVHmkYunGRQDhTUmUyXLnhxUP;

		public tiQIyXHnZPhOHCSaxgMFwzWWbpSy omBPgTeLehCKYhmZbcvkISQxQBiKA;

		public vvVeZbxtpMNoGBzBKvHiSYZiBuGT YMLdfDhcHslpTgIroknkBEMbspUEA;

		public dAajQFjvtIJBPiHxvEehIiWIoHbU(PidVid P_0, tiQIyXHnZPhOHCSaxgMFwzWWbpSy P_1, vvVeZbxtpMNoGBzBKvHiSYZiBuGT P_2)
		{
			ijmjVHmkYunGRQDhTUmUyXLnhxUP = P_0;
			omBPgTeLehCKYhmZbcvkISQxQBiKA = P_1;
			YMLdfDhcHslpTgIroknkBEMbspUEA = P_2;
		}

		public bool EveAaSdiePnBAQuZaLUTtuIwqCSL(ushort P_0, ushort P_1, tiQIyXHnZPhOHCSaxgMFwzWWbpSy P_2)
		{
			if (ijmjVHmkYunGRQDhTUmUyXLnhxUP.vendorId != P_0)
			{
				return false;
			}
			if (ijmjVHmkYunGRQDhTUmUyXLnhxUP.productId != P_1)
			{
				return false;
			}
			return YMLdfDhcHslpTgIroknkBEMbspUEA switch
			{
				vvVeZbxtpMNoGBzBKvHiSYZiBuGT.MatchAnyOption => (omBPgTeLehCKYhmZbcvkISQxQBiKA & P_2) != 0, 
				vvVeZbxtpMNoGBzBKvHiSYZiBuGT.MatchAllOptions => omBPgTeLehCKYhmZbcvkISQxQBiKA == P_2, 
				vvVeZbxtpMNoGBzBKvHiSYZiBuGT.IgnoreOptions => true, 
				_ => throw new NotImplementedException(), 
			};
		}
	}

	public enum tiQIyXHnZPhOHCSaxgMFwzWWbpSy
	{
		None = 0,
		Bluetooth = 1,
		USB = 2
	}

	private static Guid[] DckoUAtZWSunuwPXDsHWZUAAdrnL = new Guid[6]
	{
		new Guid("02D1045E-0000-0000-0000-504944564944"),
		new Guid("02DD045E-0000-0000-0000-504944564944"),
		new Guid("02E3045E-0000-0000-0000-504944564944"),
		new Guid("DEEF045E-0000-0000-0000-504944564944"),
		new Guid("02e0045e-0000-0000-0000-504944564944"),
		new Guid("02ff045e-0000-0000-0000-504944564944")
	};

	private static string[] qmLauUvgXEHaWYIldHKARtBQSBTB = new string[4] { "Controller (XBOX One For Windows)", "XBOX One For Windows (Controller)", "XBOX One Controller", "Xbox Bluetooth Gamepad" };

	private static readonly dAajQFjvtIJBPiHxvEehIiWIoHbU[] dxVqPUbcBFauSkbDYLFCZiYssEUc = new dAajQFjvtIJBPiHxvEehIiWIoHbU[1]
	{
		new dAajQFjvtIJBPiHxvEehIiWIoHbU(new PidVid(8201, 1406), tiQIyXHnZPhOHCSaxgMFwzWWbpSy.USB, dAajQFjvtIJBPiHxvEehIiWIoHbU.vvVeZbxtpMNoGBzBKvHiSYZiBuGT.MatchAnyOption)
	};

	public static string gLMtIoCxfxIRTjGFnCCoLgYSqluG(swGGwMPkQHnMamNnlUoVRJlWgbRb P_0, Guid P_1, string P_2, string P_3)
	{
		if (P_0 == null)
		{
			return string.Empty;
		}
		return bYsSQLuUCWSZmculgbbeVhGuVBxi(P_0.WdfYApUTDepZAwUNjoLChHDYYkvh, P_1, P_2, P_3) switch
		{
			BIWpfgzlfFhchCyefSgfuVQEUlJOA.CombinedTriggers => "[CombinedTriggers]", 
			BIWpfgzlfFhchCyefSgfuVQEUlJOA.SplitTriggers => "[SplitTriggers]", 
			_ => string.Empty, 
		};
	}

	public static BIWpfgzlfFhchCyefSgfuVQEUlJOA bYsSQLuUCWSZmculgbbeVhGuVBxi(vpMtTamsKlyUvPLYvJWxeZTeUxkL[] P_0, Guid P_1, string P_2, string P_3)
	{
		if (!ARMiJDrSJwImAdbNOnxEsGQIcovGA(P_1, P_2, P_3))
		{
			return BIWpfgzlfFhchCyefSgfuVQEUlJOA.None;
		}
		for (int i = 0; i < P_0.Length; i++)
		{
			if (P_0[i].SGgcqwHtDWqCNNMSdypIryKHFjOLA == 1 && !P_0[i].ZEmytAGLeAYwJqjlUacciifDZNfA && P_0[i].XVelcgMVFVgUqjSsWJVWfbENDXFB.ZnzNUAsEPqRsjgjinCXEEMLoHBMIA == 53)
			{
				return BIWpfgzlfFhchCyefSgfuVQEUlJOA.SplitTriggers;
			}
		}
		return BIWpfgzlfFhchCyefSgfuVQEUlJOA.CombinedTriggers;
	}

	public static bool ARMiJDrSJwImAdbNOnxEsGQIcovGA(Guid P_0, string P_1, string P_2)
	{
		if (Array.IndexOf(DckoUAtZWSunuwPXDsHWZUAAdrnL, P_0) >= 0)
		{
			return true;
		}
		if (uhqxGEeBPOmsbMmXmtugkivkzMFM(P_1))
		{
			return true;
		}
		if (uhqxGEeBPOmsbMmXmtugkivkzMFM(P_2))
		{
			return true;
		}
		return false;
	}

	private static bool uhqxGEeBPOmsbMmXmtugkivkzMFM(string P_0)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return false;
		}
		for (int i = 0; i < qmLauUvgXEHaWYIldHKARtBQSBTB.Length; i++)
		{
			if (qmLauUvgXEHaWYIldHKARtBQSBTB[i].Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public static bool rpCDpbayXsXDNWeSlctYbytBjjdPb(InputSource P_0, ushort P_1, ushort P_2, tiQIyXHnZPhOHCSaxgMFwzWWbpSy P_3)
	{
		if (P_0 == InputSource.DirectInput || P_0 == InputSource.RawInput)
		{
			for (int i = 0; i < dxVqPUbcBFauSkbDYLFCZiYssEUc.Length; i++)
			{
				if (dxVqPUbcBFauSkbDYLFCZiYssEUc[i].EveAaSdiePnBAQuZaLUTtuIwqCSL(P_1, P_2, P_3))
				{
					return true;
				}
			}
		}
		return false;
	}
}
