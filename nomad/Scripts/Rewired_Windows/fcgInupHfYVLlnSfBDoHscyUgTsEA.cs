using System;
using Rewired.Utils.Classes.Data;

internal class fcgInupHfYVLlnSfBDoHscyUgTsEA : LDJGvqLnFydDhJMnXduxzIERUQI
{
	public readonly float[] SytbxvDfrRdLDWckugtMRDSBscWP;

	public double UdrhIVttsGithdESGBfFjANwuKQhA;

	public readonly int XEUekQvCarjTbCYxMKFCWVhhQrjsA;

	private readonly byte[] hIucMnIjkplxZJONvbPaBxbgDPxQ;

	private readonly int fFBgLKcRsznaWIZimaESYEsMNALEA;

	private readonly int fNURhAqyKjKUNxIdcsvseiRTsXyf;

	private readonly Action<byte[], float[]> NXggyuZLRmbOHaXhDXjDIDJxKwNcb;

	public fcgInupHfYVLlnSfBDoHscyUgTsEA(byte P_0, HIDInfo P_1, int P_2, Action<byte[], float[]> P_3)
		: base(P_0, P_1)
	{
		XEUekQvCarjTbCYxMKFCWVhhQrjsA = P_2;
		NXggyuZLRmbOHaXhDXjDIDJxKwNcb = P_3;
		fFBgLKcRsznaWIZimaESYEsMNALEA = ((P_1.bitSize > 0) ? ((P_1.bitSize + 8 - 1) / 8) : 0);
		fNURhAqyKjKUNxIdcsvseiRTsXyf = P_1.dataIndex;
		hIucMnIjkplxZJONvbPaBxbgDPxQ = new byte[fFBgLKcRsznaWIZimaESYEsMNALEA];
		SytbxvDfrRdLDWckugtMRDSBscWP = new float[P_2];
	}

	public virtual void RPfCVslENnUFLvuLEgmBHlFanUJc(NativeBuffer P_0, double P_1)
	{
		if (P_0 != null && P_0[0] == jSoHFXcXXwbGoxIhzdRXdkHeQAsb)
		{
			UdrhIVttsGithdESGBfFjANwuKQhA = P_1;
			for (int i = 0; i < fFBgLKcRsznaWIZimaESYEsMNALEA; i++)
			{
				hIucMnIjkplxZJONvbPaBxbgDPxQ[i] = P_0[fNURhAqyKjKUNxIdcsvseiRTsXyf + i];
			}
			if (NXggyuZLRmbOHaXhDXjDIDJxKwNcb != null)
			{
				NXggyuZLRmbOHaXhDXjDIDJxKwNcb(hIucMnIjkplxZJONvbPaBxbgDPxQ, SytbxvDfrRdLDWckugtMRDSBscWP);
			}
		}
	}
}
