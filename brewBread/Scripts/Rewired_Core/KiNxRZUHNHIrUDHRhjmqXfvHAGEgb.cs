using UnityEngine;

internal static class KiNxRZUHNHIrUDHRhjmqXfvHAGEgb
{
	private static int iRwkCJaZNIgqoghUdAJQvSdfiVfr;

	private static int aVOlNuRVvCOJdZFGeiVVgfpKNgOsA;

	private static double[] bOFdjcvwJMJJYbZVbzljqlBgtUWi;

	private static int WmQZlytRCEGLdxrywpuKXVbHcKtG;

	private static double bTIbkAIHSnaHeSYrpabflohQpdkhA;

	private static int LxklyITMDEoQSCTiuDzKdctaYhqU;

	public static double MEYOlWSBJCsEVbZbhJGWtjFEkIqv => bTIbkAIHSnaHeSYrpabflohQpdkhA;

	public static int mDxBmwIPGYLbnHsoJrmWCcgJDystB
	{
		get
		{
			return iRwkCJaZNIgqoghUdAJQvSdfiVfr;
		}
		set
		{
			if (num <= 0)
			{
				num = 1;
			}
			if (num != iRwkCJaZNIgqoghUdAJQvSdfiVfr)
			{
				iRwkCJaZNIgqoghUdAJQvSdfiVfr = num;
				jpwugzufXqktYbXkMYboQpqCbQgL();
			}
		}
	}

	static KiNxRZUHNHIrUDHRhjmqXfvHAGEgb()
	{
		iRwkCJaZNIgqoghUdAJQvSdfiVfr = 30;
		jpwugzufXqktYbXkMYboQpqCbQgL();
	}

	public static void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
	{
		int frameCount = Time.frameCount;
		if (LxklyITMDEoQSCTiuDzKdctaYhqU < frameCount)
		{
			bOFdjcvwJMJJYbZVbzljqlBgtUWi[aVOlNuRVvCOJdZFGeiVVgfpKNgOsA] = Time.deltaTime;
			if (WmQZlytRCEGLdxrywpuKXVbHcKtG < iRwkCJaZNIgqoghUdAJQvSdfiVfr)
			{
				WmQZlytRCEGLdxrywpuKXVbHcKtG++;
			}
			double num = 0.0;
			for (int i = 0; i < WmQZlytRCEGLdxrywpuKXVbHcKtG; i++)
			{
				num += bOFdjcvwJMJJYbZVbzljqlBgtUWi[i];
			}
			bTIbkAIHSnaHeSYrpabflohQpdkhA = num / (double)WmQZlytRCEGLdxrywpuKXVbHcKtG;
			aVOlNuRVvCOJdZFGeiVVgfpKNgOsA++;
			if (aVOlNuRVvCOJdZFGeiVVgfpKNgOsA >= iRwkCJaZNIgqoghUdAJQvSdfiVfr)
			{
				aVOlNuRVvCOJdZFGeiVVgfpKNgOsA = 0;
			}
			LxklyITMDEoQSCTiuDzKdctaYhqU = frameCount;
		}
	}

	public static void jpwugzufXqktYbXkMYboQpqCbQgL()
	{
		if (bOFdjcvwJMJJYbZVbzljqlBgtUWi == null || bOFdjcvwJMJJYbZVbzljqlBgtUWi.Length != iRwkCJaZNIgqoghUdAJQvSdfiVfr)
		{
			bOFdjcvwJMJJYbZVbzljqlBgtUWi = new double[iRwkCJaZNIgqoghUdAJQvSdfiVfr];
		}
		WmQZlytRCEGLdxrywpuKXVbHcKtG = 0;
		aVOlNuRVvCOJdZFGeiVVgfpKNgOsA = 0;
		LxklyITMDEoQSCTiuDzKdctaYhqU = 0;
	}
}
