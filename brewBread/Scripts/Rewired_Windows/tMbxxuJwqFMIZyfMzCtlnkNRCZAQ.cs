using System;

internal static class tMbxxuJwqFMIZyfMzCtlnkNRCZAQ
{
	public enum gfkzxDzZzHaCLzqJGLcsnOrXdPeN
	{
		None = 0,
		CombinedTriggers = 1,
		SplitTriggers = 2
	}

	private static Guid[] jjBweJUcUOZLooGCNgDzHHjIcpwHb = new Guid[6]
	{
		new Guid("02D1045E-0000-0000-0000-504944564944"),
		new Guid("02DD045E-0000-0000-0000-504944564944"),
		new Guid("02E3045E-0000-0000-0000-504944564944"),
		new Guid("DEEF045E-0000-0000-0000-504944564944"),
		new Guid("02e0045e-0000-0000-0000-504944564944"),
		new Guid("02ff045e-0000-0000-0000-504944564944")
	};

	private static string[] mAMXhYmPbYYaPWTZYMvHQAwIuabD = new string[4] { "Controller (XBOX One For Windows)", "XBOX One For Windows (Controller)", "XBOX One Controller", "Xbox Bluetooth Gamepad" };

	private const string hOUfhGnyiiotdVOvlAWpphTQwakA = ".*xbox[ \\-]one.*";

	public static string uKRTSpybUDcsBMsDKkIYjkEzwolv(lsKtywNItRkdXkHtGNiwstXBoooQ P_0, Guid P_1, string P_2, string P_3)
	{
		if (P_0 == null)
		{
			return string.Empty;
		}
		return qQacHFjgbSgtMFFqDbcWXmYNAeQu(P_0.mMJLzBQmvFtnoNziMdPlkywXLEVf, P_1, P_2, P_3) switch
		{
			gfkzxDzZzHaCLzqJGLcsnOrXdPeN.CombinedTriggers => "[CombinedTriggers]", 
			gfkzxDzZzHaCLzqJGLcsnOrXdPeN.SplitTriggers => "[SplitTriggers]", 
			_ => string.Empty, 
		};
	}

	public static gfkzxDzZzHaCLzqJGLcsnOrXdPeN qQacHFjgbSgtMFFqDbcWXmYNAeQu(wSGjsAqFrhUqKLiDYgalBHHdGXvC[] P_0, Guid P_1, string P_2, string P_3)
	{
		if (!bctGcoziEXHqMiayEeZzVHnVWZkmA(P_1, P_2, P_3))
		{
			return gfkzxDzZzHaCLzqJGLcsnOrXdPeN.None;
		}
		for (int i = 0; i < P_0.Length; i++)
		{
			if (P_0[i].uuNGjxmdPvmxDegJbENqZbvxonIw == 1 && !P_0[i].iURENcRSxKDuucpJEleJgTvASoNyb && P_0[i].UodwIATkTxdVgIiRpCAhDEHugUBAb.wdLEfLZxrjtcLdIntcuABrEoICsnA == 53)
			{
				return gfkzxDzZzHaCLzqJGLcsnOrXdPeN.SplitTriggers;
			}
		}
		return gfkzxDzZzHaCLzqJGLcsnOrXdPeN.CombinedTriggers;
	}

	public static bool bctGcoziEXHqMiayEeZzVHnVWZkmA(Guid P_0, string P_1, string P_2)
	{
		if (Array.IndexOf(jjBweJUcUOZLooGCNgDzHHjIcpwHb, P_0) >= 0)
		{
			return true;
		}
		if (bctGcoziEXHqMiayEeZzVHnVWZkmA(P_1))
		{
			return true;
		}
		if (bctGcoziEXHqMiayEeZzVHnVWZkmA(P_2))
		{
			return true;
		}
		return false;
	}

	private static bool bctGcoziEXHqMiayEeZzVHnVWZkmA(string P_0)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return false;
		}
		for (int i = 0; i < mAMXhYmPbYYaPWTZYMvHQAwIuabD.Length; i++)
		{
			if (mAMXhYmPbYYaPWTZYMvHQAwIuabD[i].Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
