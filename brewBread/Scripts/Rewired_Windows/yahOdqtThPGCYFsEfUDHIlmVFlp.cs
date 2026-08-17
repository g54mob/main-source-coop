using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 4)]
internal struct yahOdqtThPGCYFsEfUDHIlmVFlp
{
	private int AErMhpdwbOvJuaKrdgIbHdGycvmeA;

	private const int AwsndVPfnjBzkTEePXfRzGhoNKSU = 65534;

	private const int tUJWwrQVRxbsWTNPFhHZdiwChcFmA = 16776960;

	public IsDELYDpjvwtAtxUZEGxhuFLuhdoA ZIvyiOSzgMRCzZiwgApXsRrDjjHn => (IsDELYDpjvwtAtxUZEGxhuFLuhdoA)(AErMhpdwbOvJuaKrdgIbHdGycvmeA & -16776961);

	public int mIjCdPlVjzGkiHyfeRJaFIbiZgMwA => (AErMhpdwbOvJuaKrdgIbHdGycvmeA >> 8) & 0xFFFF;

	public yahOdqtThPGCYFsEfUDHIlmVFlp(IsDELYDpjvwtAtxUZEGxhuFLuhdoA P_0, int P_1)
	{
		this = default(yahOdqtThPGCYFsEfUDHIlmVFlp);
		AErMhpdwbOvJuaKrdgIbHdGycvmeA = (int)(P_0 & ~IsDELYDpjvwtAtxUZEGxhuFLuhdoA.AnyInstance) | ((!(P_1 < 0 || P_1 > 65534)) ? ((P_1 & 0xFFFF) << 8) : 0);
	}

	[SpecialName]
	public static int WEpFdhrCknulBLmFExBPmfoKIqww(yahOdqtThPGCYFsEfUDHIlmVFlp P_0)
	{
		return P_0.AErMhpdwbOvJuaKrdgIbHdGycvmeA;
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(yahOdqtThPGCYFsEfUDHIlmVFlp P_0)
	{
		return P_0.AErMhpdwbOvJuaKrdgIbHdGycvmeA == AErMhpdwbOvJuaKrdgIbHdGycvmeA;
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if ((object)P_0.GetType() != typeof(yahOdqtThPGCYFsEfUDHIlmVFlp))
		{
			return false;
		}
		return RiVeyXzIIJEVyClDkSYOlnCBsUpL((yahOdqtThPGCYFsEfUDHIlmVFlp)P_0);
	}

	public int vpCtZDiWtrmaqwDCmMjniXrnNrOD()
	{
		return AErMhpdwbOvJuaKrdgIbHdGycvmeA;
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return string.Format(CultureInfo.InvariantCulture, "Flags: {0} InstanceNumber: {1} RawId: 0x{2:X8}", new object[3] { ZIvyiOSzgMRCzZiwgApXsRrDjjHn, mIjCdPlVjzGkiHyfeRJaFIbiZgMwA, AErMhpdwbOvJuaKrdgIbHdGycvmeA });
	}
}
