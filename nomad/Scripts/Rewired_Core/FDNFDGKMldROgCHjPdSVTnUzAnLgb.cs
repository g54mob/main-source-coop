using System;
using Rewired.Data.Mapping;
using Rewired.Utils.Classes.Data;

internal abstract class FDNFDGKMldROgCHjPdSVTnUzAnLgb : FopEdGIdXdIETmMGGpfJptNngDfeb
{
	public class ZOYZiKTXJOgDMYfQecMShLaAjrubA
	{
		public readonly AxisDirection? hVoDmAuzzdsThlipnOyZAzFukJTU;

		public ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection? P_0)
		{
			hVoDmAuzzdsThlipnOyZAzFukJTU = P_0;
		}
	}

	public class fOURUkakBckGnlgOkmwbemAJMKXu
	{
		private readonly AList<ZOYZiKTXJOgDMYfQecMShLaAjrubA> MdPlpkLpDlwYFaNXmQIAVHqIifQu;

		public readonly int nljDQfJxofEWjKNOQVvUJFNJxAjqA;

		public fOURUkakBckGnlgOkmwbemAJMKXu(AList<ZOYZiKTXJOgDMYfQecMShLaAjrubA> P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < P_0._count; i++)
			{
				if (P_0._items[i] == null)
				{
					throw new ArgumentNullException();
				}
			}
			MdPlpkLpDlwYFaNXmQIAVHqIifQu = P_0;
			nljDQfJxofEWjKNOQVvUJFNJxAjqA = MdPlpkLpDlwYFaNXmQIAVHqIifQu._count;
		}

		public ZOYZiKTXJOgDMYfQecMShLaAjrubA kjxfzmNdTUsBjoWkavsROrRXBLoe(int P_0)
		{
			return MdPlpkLpDlwYFaNXmQIAVHqIifQu._items[P_0];
		}

		public int mzqArobTQIFTrvtmnTEwpJtuvJTTA(AxisDirection P_0)
		{
			for (int i = 0; i < MdPlpkLpDlwYFaNXmQIAVHqIifQu._count; i++)
			{
				if (MdPlpkLpDlwYFaNXmQIAVHqIifQu[i].hVoDmAuzzdsThlipnOyZAzFukJTU.HasValue && MdPlpkLpDlwYFaNXmQIAVHqIifQu[i].hVoDmAuzzdsThlipnOyZAzFukJTU.Value == P_0)
				{
					return i;
				}
			}
			return -1;
		}
	}

	public enum cwXjjPkxPWMBcEFwnCUCcKJomfDI
	{
		None = 0,
		Names = 1,
		Keys = 2,
		All = -1
	}

	public enum IWFfSiUJvBqDpiIZIfMeuQqJMGul
	{
		None = 0,
		DescriptiveName = 1,
		PositiveDescriptiveName = 2,
		NegativeDescriptiveName = 4,
		PositiveKey = 8,
		NegativeKey = 16,
		SpecialDescrptiveName0 = 16384,
		SpecialDescrptiveName1 = 32768,
		SpecialDescrptiveName2 = 65536,
		SpecialDescrptiveName3 = 131072,
		SpecialDescrptiveName4 = 262144,
		SpecialDescrptiveName5 = 524288,
		SpecialDescrptiveName6 = 1048576,
		SpecialDescrptiveName7 = 2097152,
		SpecialDescrptiveName8 = 4194304,
		SpecialKey0 = 8388608,
		SpecialKey1 = 16777216,
		SpecialKey2 = 33554432,
		SpecialKey3 = 67108864,
		SpecialKey4 = 134217728,
		SpecialKey5 = 268435456,
		SpecialKey6 = 536870912,
		SpecialKey7 = 1073741824,
		SpecialKey8 = -2147483648,
		All = -1
	}

	public enum sztzDKprOgaEtSRoFjITTczsHDuW
	{
		Axis = 0,
		Button = 1,
		CompoundElement = 100,
		Unknown = 2147483647
	}

	public enum LsWebCorzTdhEUjUrAlgVzPmJJHR
	{
		None = 0,
		Axis2D = 1,
		Hat = 2,
		ThumbStick = 3,
		DPad = 4,
		Stick = 5,
		Stick6D = 6,
		Unknown = 2147483647
	}

	private static readonly ADictionary<int, fOURUkakBckGnlgOkmwbemAJMKXu> KcQsqbHErXgucntESulVwFsWoXyl = new ADictionary<int, fOURUkakBckGnlgOkmwbemAJMKXu>
	{
		{
			4,
			new fOURUkakBckGnlgOkmwbemAJMKXu(new AList<ZOYZiKTXJOgDMYfQecMShLaAjrubA>
			{
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Horizontal),
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Vertical)
			})
		},
		{
			1,
			new fOURUkakBckGnlgOkmwbemAJMKXu(new AList<ZOYZiKTXJOgDMYfQecMShLaAjrubA>
			{
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Horizontal),
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Vertical)
			})
		},
		{
			5,
			new fOURUkakBckGnlgOkmwbemAJMKXu(new AList<ZOYZiKTXJOgDMYfQecMShLaAjrubA>
			{
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Horizontal),
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Vertical)
			})
		},
		{
			3,
			new fOURUkakBckGnlgOkmwbemAJMKXu(new AList<ZOYZiKTXJOgDMYfQecMShLaAjrubA>
			{
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Horizontal),
				new ZOYZiKTXJOgDMYfQecMShLaAjrubA(AxisDirection.Vertical)
			})
		}
	};

	private sztzDKprOgaEtSRoFjITTczsHDuW ouEwQdtIzXnpidHPXFcDDZTCeGlbA;

	private LsWebCorzTdhEUjUrAlgVzPmJJHR YLjMNxMDhLBdsfxbbWpThhcEywFG;

	public sztzDKprOgaEtSRoFjITTczsHDuW tnQKiUchqifinDnJuSQdboJqaQgK
	{
		get
		{
			return ouEwQdtIzXnpidHPXFcDDZTCeGlbA;
		}
		set
		{
			if (sztzDKprOgaEtSRoFjITTczsHDuW2 != ouEwQdtIzXnpidHPXFcDDZTCeGlbA)
			{
				ouEwQdtIzXnpidHPXFcDDZTCeGlbA = sztzDKprOgaEtSRoFjITTczsHDuW2;
				if (base.LnugEECVSivmVOJnlMxqTndnTmyO)
				{
					bIVZUTIzQVeRSNEzqyWioRbktgUX();
				}
			}
		}
	}

	public LsWebCorzTdhEUjUrAlgVzPmJJHR bEKIbEjDpMfnJogudzLlVaoylrODb
	{
		get
		{
			return YLjMNxMDhLBdsfxbbWpThhcEywFG;
		}
		set
		{
			if (lsWebCorzTdhEUjUrAlgVzPmJJHR != YLjMNxMDhLBdsfxbbWpThhcEywFG)
			{
				YLjMNxMDhLBdsfxbbWpThhcEywFG = lsWebCorzTdhEUjUrAlgVzPmJJHR;
				if (base.LnugEECVSivmVOJnlMxqTndnTmyO)
				{
					bIVZUTIzQVeRSNEzqyWioRbktgUX();
				}
			}
		}
	}

	public static bool tGJPnPxMDZQkntURokRfyFQymnPw(LsWebCorzTdhEUjUrAlgVzPmJJHR P_0, out fOURUkakBckGnlgOkmwbemAJMKXu P_1)
	{
		return KcQsqbHErXgucntESulVwFsWoXyl.TryGetValue((int)P_0, out P_1);
	}

	public static int FaWRBKagAChKzYIPTHxodNThJXtKA(sztzDKprOgaEtSRoFjITTczsHDuW P_0, LsWebCorzTdhEUjUrAlgVzPmJJHR P_1)
	{
		if (P_0 != sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement)
		{
			return 0;
		}
		if (!KcQsqbHErXgucntESulVwFsWoXyl.TryGetValue((int)P_1, out var value))
		{
			return 0;
		}
		return value.nljDQfJxofEWjKNOQVvUJFNJxAjqA;
	}

	protected FDNFDGKMldROgCHjPdSVTnUzAnLgb(sztzDKprOgaEtSRoFjITTczsHDuW P_0, LsWebCorzTdhEUjUrAlgVzPmJJHR P_1)
	{
		ouEwQdtIzXnpidHPXFcDDZTCeGlbA = P_0;
		YLjMNxMDhLBdsfxbbWpThhcEywFG = P_1;
	}

	protected FDNFDGKMldROgCHjPdSVTnUzAnLgb(bguKJVtsagJfXPpJQeurpzlOLIYd P_0, sztzDKprOgaEtSRoFjITTczsHDuW P_1, LsWebCorzTdhEUjUrAlgVzPmJJHR P_2)
		: base(P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("dataSource");
		}
		ouEwQdtIzXnpidHPXFcDDZTCeGlbA = P_1;
		YLjMNxMDhLBdsfxbbWpThhcEywFG = P_2;
	}

	protected virtual void UnrybsryaIWFpZtUNyuzUDGOqKMH()
	{
		base.ZdBgfEeqqkPJHSqpfMhEfMocFnxqB();
		nIhCTLejCUDzcAzoRcmLjSsBLnitA();
	}

	public virtual void IKlsylKZUPoPZutFmVypQCUyJCdT()
	{
		base.mSjKwPJiyMmQSynHcMWYSOiGAmDB();
		nIhCTLejCUDzcAzoRcmLjSsBLnitA(cwXjjPkxPWMBcEFwnCUCcKJomfDI.Names);
	}

	public virtual void sUWPMtNBLWLNaHkrSGMrmklXilrKA()
	{
		base.hPAqnroLHIcbmjXldArpEMTOSiTdb();
		nIhCTLejCUDzcAzoRcmLjSsBLnitA(cwXjjPkxPWMBcEFwnCUCcKJomfDI.Keys);
	}

	public virtual void WhyomjqAiSQidbzNtitaTqMOrWgT()
	{
		base.YSgvMmquHVoFhixWnSsVWmcflge();
		nIhCTLejCUDzcAzoRcmLjSsBLnitA(cwXjjPkxPWMBcEFwnCUCcKJomfDI.Names);
	}

	public virtual bool TIbllctYhBPpFlqcVRhFKAQKVURc(FopEdGIdXdIETmMGGpfJptNngDfeb P_0, bool P_1)
	{
		FDNFDGKMldROgCHjPdSVTnUzAnLgb fDNFDGKMldROgCHjPdSVTnUzAnLgb = P_0 as FDNFDGKMldROgCHjPdSVTnUzAnLgb;
		if (fDNFDGKMldROgCHjPdSVTnUzAnLgb != null)
		{
			return false;
		}
		if (!base.PstXrjtELzTCScyWAcSsHTXMaNuK(P_0, P_1))
		{
			return false;
		}
		return ouEwQdtIzXnpidHPXFcDDZTCeGlbA == fDNFDGKMldROgCHjPdSVTnUzAnLgb.tnQKiUchqifinDnJuSQdboJqaQgK;
	}

	protected virtual void bnDwmhTjXYwxCTTxOQceUSJDV()
	{
		base.uCzgBTyTNNdCVFbBSFinxXiSmemm();
		EfmhgRjLUYYNAKgnORbsBEMiVRrB(IWFfSiUJvBqDpiIZIfMeuQqJMGul.All);
	}

	protected virtual void nIhCTLejCUDzcAzoRcmLjSsBLnitA(cwXjjPkxPWMBcEFwnCUCcKJomfDI P_0 = cwXjjPkxPWMBcEFwnCUCcKJomfDI.None)
	{
		if (P_0 != cwXjjPkxPWMBcEFwnCUCcKJomfDI.None)
		{
			FgsRvQnWWmjCqImTQehRwHPjGIsy(P_0);
		}
		bguKJVtsagJfXPpJQeurpzlOLIYd bguKJVtsagJfXPpJQeurpzlOLIYd2 = tFHgnqTfapMzUfeUcgrtSgOorhTm();
		if (bguKJVtsagJfXPpJQeurpzlOLIYd2 != null && (bguKJVtsagJfXPpJQeurpzlOLIYd2.autoGeneratedValueFlags & 1) == 0 && string.IsNullOrEmpty(bguKJVtsagJfXPpJQeurpzlOLIYd2.nonLocalizedDescriptiveName) && !string.IsNullOrEmpty(bguKJVtsagJfXPpJQeurpzlOLIYd2.scriptingName))
		{
			bguKJVtsagJfXPpJQeurpzlOLIYd2.nonLocalizedDescriptiveName = bguKJVtsagJfXPpJQeurpzlOLIYd2.scriptingName;
			bguKJVtsagJfXPpJQeurpzlOLIYd2.autoGeneratedValueFlags |= 1;
			WNOKfMLBcQgmwLBfRHLEkGrCnlHM(1);
		}
	}

	protected virtual void xYbcWjXLLhKYUojdhfrKszdygzaM(int P_0)
	{
		base.xGNDJHgrbsPfNhcEFUzykpVDiHjoD(P_0);
		EfmhgRjLUYYNAKgnORbsBEMiVRrB((IWFfSiUJvBqDpiIZIfMeuQqJMGul)P_0);
	}

	protected virtual void EfmhgRjLUYYNAKgnORbsBEMiVRrB(IWFfSiUJvBqDpiIZIfMeuQqJMGul P_0)
	{
		bguKJVtsagJfXPpJQeurpzlOLIYd bguKJVtsagJfXPpJQeurpzlOLIYd2 = tFHgnqTfapMzUfeUcgrtSgOorhTm();
		if (bguKJVtsagJfXPpJQeurpzlOLIYd2 != null && ((uint)bguKJVtsagJfXPpJQeurpzlOLIYd2.autoGeneratedValueFlags & (uint)P_0) != 0 && (P_0 & IWFfSiUJvBqDpiIZIfMeuQqJMGul.DescriptiveName) != IWFfSiUJvBqDpiIZIfMeuQqJMGul.None && (bguKJVtsagJfXPpJQeurpzlOLIYd2.autoGeneratedValueFlags & 1) != 0)
		{
			if (tFHgnqTfapMzUfeUcgrtSgOorhTm() != null)
			{
				tFHgnqTfapMzUfeUcgrtSgOorhTm().nonLocalizedDescriptiveName = null;
			}
			WNOKfMLBcQgmwLBfRHLEkGrCnlHM(1);
			bguKJVtsagJfXPpJQeurpzlOLIYd2.autoGeneratedValueFlags &= -2;
		}
	}

	private void FgsRvQnWWmjCqImTQehRwHPjGIsy(cwXjjPkxPWMBcEFwnCUCcKJomfDI P_0)
	{
		IWFfSiUJvBqDpiIZIfMeuQqJMGul iWFfSiUJvBqDpiIZIfMeuQqJMGul = MYTaudXHwNmJAoFwqqafrbwMFkCFA(P_0);
		if (iWFfSiUJvBqDpiIZIfMeuQqJMGul != IWFfSiUJvBqDpiIZIfMeuQqJMGul.None)
		{
			EfmhgRjLUYYNAKgnORbsBEMiVRrB(iWFfSiUJvBqDpiIZIfMeuQqJMGul);
		}
	}

	protected virtual IWFfSiUJvBqDpiIZIfMeuQqJMGul MYTaudXHwNmJAoFwqqafrbwMFkCFA(cwXjjPkxPWMBcEFwnCUCcKJomfDI P_0)
	{
		IWFfSiUJvBqDpiIZIfMeuQqJMGul iWFfSiUJvBqDpiIZIfMeuQqJMGul = IWFfSiUJvBqDpiIZIfMeuQqJMGul.None;
		if ((P_0 & cwXjjPkxPWMBcEFwnCUCcKJomfDI.Names) != cwXjjPkxPWMBcEFwnCUCcKJomfDI.None)
		{
			iWFfSiUJvBqDpiIZIfMeuQqJMGul |= IWFfSiUJvBqDpiIZIfMeuQqJMGul.DescriptiveName;
		}
		return iWFfSiUJvBqDpiIZIfMeuQqJMGul;
	}

	protected virtual void KDsUceJlYCDVBhMNBMROGovUueSnA()
	{
		base.osxHrggonzizrFGCtREPyqJvvdnM();
		gHHpnMuGywauOByTsYtKEwUhbDMtA(1, new OBzZblFCcoXjpegrNwgWHpLkWsfD
		{
			KOtNCnhieDYUVmamgrTOUjanaOIu = cxVNsagEchUjISqSPQDiPoPJjeKKA
		});
	}
}
