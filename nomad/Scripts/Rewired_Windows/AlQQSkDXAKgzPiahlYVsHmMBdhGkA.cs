using System;
using Rewired.Utils.Classes.Data;

internal class AlQQSkDXAKgzPiahlYVsHmMBdhGkA : LDJGvqLnFydDhJMnXduxzIERUQI
{
	public enum WWlBQJlQhoCFmgwOpQpzKjCPuhNO
	{
		Default = 0,
		Custom = 1
	}

	public int qDbXJhTPhwfBaBhvucjHCjTseXSJ;

	public double JqEbDxcTBPIhJGjXjiuiobzptpWgc;

	public readonly int OrVCRitliPvAaazYRFcRJlHsLbt;

	public readonly int rMuYHvHwPCHBNfhTyOaLmmKFxdDO;

	public readonly WWlBQJlQhoCFmgwOpQpzKjCPuhNO BmrscOgqHSMIPcHBfgzBjlVPercM;

	private Func<int, int> LCRlAoDbuBedWoJkMkBrGggJPhDi;

	public AlQQSkDXAKgzPiahlYVsHmMBdhGkA(byte P_0, HIDInfo P_1, WWlBQJlQhoCFmgwOpQpzKjCPuhNO P_2)
		: base(P_0, P_1)
	{
		BmrscOgqHSMIPcHBfgzBjlVPercM = P_2;
		OrVCRitliPvAaazYRFcRJlHsLbt = ((P_1.bitSize > 0) ? ((P_1.bitSize + 8 - 1) / 8) : 0);
		rMuYHvHwPCHBNfhTyOaLmmKFxdDO = P_1.dataIndex;
	}

	public AlQQSkDXAKgzPiahlYVsHmMBdhGkA(byte P_0, HIDInfo P_1, Func<int, int> P_2)
		: this(P_0, P_1, WWlBQJlQhoCFmgwOpQpzKjCPuhNO.Custom)
	{
		LCRlAoDbuBedWoJkMkBrGggJPhDi = P_2;
	}

	public virtual void jjMFBifwOaZSKHwAGjuopkihwESTA(NativeBuffer P_0, double P_1)
	{
		if (P_0 == null || P_0[0] != jSoHFXcXXwbGoxIhzdRXdkHeQAsb)
		{
			return;
		}
		JqEbDxcTBPIhJGjXjiuiobzptpWgc = P_1;
		if (OrVCRitliPvAaazYRFcRJlHsLbt == 1)
		{
			qDbXJhTPhwfBaBhvucjHCjTseXSJ = P_0[rMuYHvHwPCHBNfhTyOaLmmKFxdDO];
		}
		else
		{
			qDbXJhTPhwfBaBhvucjHCjTseXSJ = 0;
			for (int i = 0; i < OrVCRitliPvAaazYRFcRJlHsLbt; i++)
			{
				qDbXJhTPhwfBaBhvucjHCjTseXSJ |= P_0[rMuYHvHwPCHBNfhTyOaLmmKFxdDO + i] << 8 * i;
			}
		}
		if (BmrscOgqHSMIPcHBfgzBjlVPercM == WWlBQJlQhoCFmgwOpQpzKjCPuhNO.Custom && LCRlAoDbuBedWoJkMkBrGggJPhDi != null)
		{
			qDbXJhTPhwfBaBhvucjHCjTseXSJ = LCRlAoDbuBedWoJkMkBrGggJPhDi(qDbXJhTPhwfBaBhvucjHCjTseXSJ);
		}
	}
}
