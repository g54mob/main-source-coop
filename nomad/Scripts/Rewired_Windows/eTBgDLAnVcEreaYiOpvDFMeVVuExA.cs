using Rewired.Utils.Classes.Data;

internal class eTBgDLAnVcEreaYiOpvDFMeVVuExA : LDJGvqLnFydDhJMnXduxzIERUQI
{
	public int VZjaMgWvNYHgadCmfWslmhJQPcDM;

	public double GFbHhsaDkhQsDzbiNFszlZyEQujk;

	public readonly int VxcqOuexFolZjZPhZwdQdmjroCGX;

	public readonly int OKxpFeIFyGfZOpGCwEnbGbKZXjSr;

	public readonly bool HcpOOjEGRPSSVcWILEMYjvPAqRoe;

	public readonly int jCOQIsyNVWYlypieeKpAyExqJETd;

	public readonly int wyuFkfulqCgacFPMVBGLfCGkCRslA;

	public readonly int gULvkGaDkKtXZVtqcfPrAJHbxIHt;

	public eTBgDLAnVcEreaYiOpvDFMeVVuExA(byte P_0, HIDInfo P_1, bool P_2, int P_3)
		: base(P_0, P_1)
	{
		VxcqOuexFolZjZPhZwdQdmjroCGX = ((P_1.bitSize > 0) ? ((P_1.bitSize + 8 - 1) / 8) : 0);
		OKxpFeIFyGfZOpGCwEnbGbKZXjSr = P_1.dataIndex;
		HcpOOjEGRPSSVcWILEMYjvPAqRoe = P_2;
		jCOQIsyNVWYlypieeKpAyExqJETd = P_1.logicalMin;
		wyuFkfulqCgacFPMVBGLfCGkCRslA = P_1.logicalMax;
		gULvkGaDkKtXZVtqcfPrAJHbxIHt = P_3;
	}

	public virtual void mmdXawNbKytWOMAfRNIKxSUNBNNm(NativeBuffer P_0, double P_1)
	{
		if (P_0 == null || P_0[0] != jSoHFXcXXwbGoxIhzdRXdkHeQAsb)
		{
			return;
		}
		GFbHhsaDkhQsDzbiNFszlZyEQujk = P_1;
		int num = 0;
		if (VxcqOuexFolZjZPhZwdQdmjroCGX > 1)
		{
			for (int i = 0; i < VxcqOuexFolZjZPhZwdQdmjroCGX; i++)
			{
				num |= P_0[OKxpFeIFyGfZOpGCwEnbGbKZXjSr + i] << 8 * i;
			}
		}
		else
		{
			num = P_0[OKxpFeIFyGfZOpGCwEnbGbKZXjSr];
		}
		VZjaMgWvNYHgadCmfWslmhJQPcDM = num;
	}
}
