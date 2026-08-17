using UnityEngine;

internal static class kpzsiDkaavQYTfjiBePeYOibMRNK
{
	private static int hrnekGatjuHfxiXAXyGERbZYGbsZA;

	private static int ClcaeAluATqpHYVoIojyytjWrRAS;

	private static double[] vfCBtsWiTAskdPiFDBWPciRIPGho;

	private static int mtyBLrTItkAaUXFPbAoPXqkfcWzeA;

	private static double rHBjXiZEffIdhNlBMCFarMWsXFYK;

	private static int bkPjkyWgkxoHRKJgYwzDFvHgFOiq;

	public static double imlhKKPXqhGYrvhfXhLdkmnvmiPB => rHBjXiZEffIdhNlBMCFarMWsXFYK;

	public static int rWCIZILDIZGSnjkpJJlDMjmiTXCH
	{
		get
		{
			return hrnekGatjuHfxiXAXyGERbZYGbsZA;
		}
		set
		{
			if (num <= 0)
			{
				num = 1;
			}
			if (num != hrnekGatjuHfxiXAXyGERbZYGbsZA)
			{
				hrnekGatjuHfxiXAXyGERbZYGbsZA = num;
				nmQcvfiQljqGEhbwKOoyuxMHHAmRA();
			}
		}
	}

	static kpzsiDkaavQYTfjiBePeYOibMRNK()
	{
		hrnekGatjuHfxiXAXyGERbZYGbsZA = 30;
		nmQcvfiQljqGEhbwKOoyuxMHHAmRA();
	}

	public static void CMmKAEEdkDTQmjazAcLsfYEKBNccA()
	{
		int frameCount = Time.frameCount;
		if (bkPjkyWgkxoHRKJgYwzDFvHgFOiq < frameCount)
		{
			vfCBtsWiTAskdPiFDBWPciRIPGho[ClcaeAluATqpHYVoIojyytjWrRAS] = Time.deltaTime;
			if (mtyBLrTItkAaUXFPbAoPXqkfcWzeA < hrnekGatjuHfxiXAXyGERbZYGbsZA)
			{
				mtyBLrTItkAaUXFPbAoPXqkfcWzeA++;
			}
			double num = 0.0;
			for (int i = 0; i < mtyBLrTItkAaUXFPbAoPXqkfcWzeA; i++)
			{
				num += vfCBtsWiTAskdPiFDBWPciRIPGho[i];
			}
			rHBjXiZEffIdhNlBMCFarMWsXFYK = num / (double)mtyBLrTItkAaUXFPbAoPXqkfcWzeA;
			ClcaeAluATqpHYVoIojyytjWrRAS++;
			if (ClcaeAluATqpHYVoIojyytjWrRAS >= hrnekGatjuHfxiXAXyGERbZYGbsZA)
			{
				ClcaeAluATqpHYVoIojyytjWrRAS = 0;
			}
			bkPjkyWgkxoHRKJgYwzDFvHgFOiq = frameCount;
		}
	}

	public static void nmQcvfiQljqGEhbwKOoyuxMHHAmRA()
	{
		if (vfCBtsWiTAskdPiFDBWPciRIPGho == null || vfCBtsWiTAskdPiFDBWPciRIPGho.Length != hrnekGatjuHfxiXAXyGERbZYGbsZA)
		{
			vfCBtsWiTAskdPiFDBWPciRIPGho = new double[hrnekGatjuHfxiXAXyGERbZYGbsZA];
		}
		mtyBLrTItkAaUXFPbAoPXqkfcWzeA = 0;
		ClcaeAluATqpHYVoIojyytjWrRAS = 0;
		bkPjkyWgkxoHRKJgYwzDFvHgFOiq = 0;
	}
}
