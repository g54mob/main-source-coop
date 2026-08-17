using Rewired.Utils;

internal class sVblRlLmgCbwrGVblzVfOQFdlFJd : UJWICcCoomjNSdpdRhBjfogHzmkXb
{
	public readonly int RLkZQPlGcgWsHSWfVQxBjZelBsWg;

	public readonly int qQKBvIZKNZyzJGaOEOEsEQbrTLbS;

	public readonly int OCIUkterLnXnOeypqcbOWAMNbdjl;

	public readonly int qFKwwFCyBbSLjJjqhEdZQvSIMnqd;

	public readonly int dAPebAicpysGqFqVvzpkVnEsmNGm;

	public readonly int sbzksAStCuJVIuLaBssvADfkiqnH;

	public readonly uint JoaCKTOSUomdjwuJcJDHGCSQvgLF;

	public readonly uint pllQGtPqohGIdldIRRVOfngQUOVK;

	public readonly int cRWvFQhaKEgbtgOswUvQOLDcjcfi;

	private readonly int bDmOvqapOgPfPhuldtxTpRxTRnlp;

	public uint AnebwuHTCIhUVphVZsCAuxNDYiqd;

	public int lldCVvRkHRfVnMGBtiZlGoIQhngM
	{
		get
		{
			if (AnebwuHTCIhUVphVZsCAuxNDYiqd < RLkZQPlGcgWsHSWfVQxBjZelBsWg || AnebwuHTCIhUVphVZsCAuxNDYiqd > qQKBvIZKNZyzJGaOEOEsEQbrTLbS)
			{
				return -1;
			}
			int num = (int)((AnebwuHTCIhUVphVZsCAuxNDYiqd - RLkZQPlGcgWsHSWfVQxBjZelBsWg) / bDmOvqapOgPfPhuldtxTpRxTRnlp * 4500);
			if (num >= 36000)
			{
				num = 0;
			}
			return num;
		}
	}

	public sVblRlLmgCbwrGVblzVfOQFdlFJd(byte P_0, ushort P_1, ushort P_2, int P_3, int P_4, int P_5, int P_6, int P_7, int P_8, uint P_9, uint P_10, int P_11)
		: base(P_0, P_1, P_2, P_3, P_4)
	{
		RLkZQPlGcgWsHSWfVQxBjZelBsWg = P_5;
		qQKBvIZKNZyzJGaOEOEsEQbrTLbS = P_6;
		JoaCKTOSUomdjwuJcJDHGCSQvgLF = P_9;
		pllQGtPqohGIdldIRRVOfngQUOVK = P_10;
		cRWvFQhaKEgbtgOswUvQOLDcjcfi = P_11;
		OCIUkterLnXnOeypqcbOWAMNbdjl = P_5 - 1;
		if (OCIUkterLnXnOeypqcbOWAMNbdjl < 0)
		{
			OCIUkterLnXnOeypqcbOWAMNbdjl = P_6 + 1;
		}
		sbzksAStCuJVIuLaBssvADfkiqnH = -1;
		int num = P_6 - P_5 + 1;
		bDmOvqapOgPfPhuldtxTpRxTRnlp = MathTools.Clamp(num / 8, 1, int.MaxValue);
		ZrbFhGEbWRbTzVxxQimUntnkwisKA();
	}

	public override void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		AnebwuHTCIhUVphVZsCAuxNDYiqd = (uint)OCIUkterLnXnOeypqcbOWAMNbdjl;
	}
}
