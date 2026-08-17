using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rewired.Internal.Localization;
using Rewired.Utils.Classes.Data;

internal abstract class FopEdGIdXdIETmMGGpfJptNngDfeb : cAwfhgIDGfMqIqwFGxVCNiWfViqT
{
	protected struct OBzZblFCcoXjpegrNwgWHpLkWsfD : IEquatable<OBzZblFCcoXjpegrNwgWHpLkWsfD>
	{
		public LocalizedString KOtNCnhieDYUVmamgrTOUjanaOIu;

		public int zYaGNhhcsCIUvDXBxclPthDHCMVvA;

		public OBzZblFCcoXjpegrNwgWHpLkWsfD(LocalizedString P_0, int P_1)
		{
			KOtNCnhieDYUVmamgrTOUjanaOIu = P_0;
			zYaGNhhcsCIUvDXBxclPthDHCMVvA = P_1;
		}

		public bool HNxGLekZHiPlfqNBWjpHHqIagPLd(object P_0)
		{
			if (!(P_0 is OBzZblFCcoXjpegrNwgWHpLkWsfD oBzZblFCcoXjpegrNwgWHpLkWsfD))
			{
				return false;
			}
			if (oBzZblFCcoXjpegrNwgWHpLkWsfD.KOtNCnhieDYUVmamgrTOUjanaOIu == KOtNCnhieDYUVmamgrTOUjanaOIu)
			{
				return oBzZblFCcoXjpegrNwgWHpLkWsfD.zYaGNhhcsCIUvDXBxclPthDHCMVvA == zYaGNhhcsCIUvDXBxclPthDHCMVvA;
			}
			return false;
		}

		public int KFEcaaWbmUiGXGkrwYMmwPgphuNy()
		{
			return (17 * 29 + KOtNCnhieDYUVmamgrTOUjanaOIu.GetHashCode()) * 29 + zYaGNhhcsCIUvDXBxclPthDHCMVvA.GetHashCode();
		}

		public bool Equals(OBzZblFCcoXjpegrNwgWHpLkWsfD other)
		{
			if (KOtNCnhieDYUVmamgrTOUjanaOIu == other.KOtNCnhieDYUVmamgrTOUjanaOIu)
			{
				return zYaGNhhcsCIUvDXBxclPthDHCMVvA == other.zYaGNhhcsCIUvDXBxclPthDHCMVvA;
			}
			return false;
		}

		bool IEquatable<OBzZblFCcoXjpegrNwgWHpLkWsfD>.Equals(OBzZblFCcoXjpegrNwgWHpLkWsfD other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		[SpecialName]
		public static bool UsziCfYSdUGLhHjbNMJgSgHZBtLZA(OBzZblFCcoXjpegrNwgWHpLkWsfD P_0, OBzZblFCcoXjpegrNwgWHpLkWsfD P_1)
		{
			return P_0.Equals(P_1);
		}

		[SpecialName]
		public static bool qjcjcdWmUAfEXeSuhesyEEDDISpSA(OBzZblFCcoXjpegrNwgWHpLkWsfD P_0, OBzZblFCcoXjpegrNwgWHpLkWsfD P_1)
		{
			return !P_0.Equals(P_1);
		}
	}

	private bguKJVtsagJfXPpJQeurpzlOLIYd IGYnQTyrLaOlJSkcwVhRDOUmXbur;

	protected readonly LocalizedString cxVNsagEchUjISqSPQDiPoPJjeKKA;

	private Id jLVmRFZefgaSuSroXIuoDZbWUXufA;

	private readonly Dictionary<int, List<OBzZblFCcoXjpegrNwgWHpLkWsfD>> rKpLIcePJnEQcEqBcjkFEAaEAoIzA;

	private bool rYohYODoYEitENdaKrwOVgjoIrVR;

	protected bool LnugEECVSivmVOJnlMxqTndnTmyO => rYohYODoYEitENdaKrwOVgjoIrVR;

	public abstract string MpfwJMTclVnnxEuHhBPCmlxJadkBA { get; }

	protected FopEdGIdXdIETmMGGpfJptNngDfeb()
	{
		cxVNsagEchUjISqSPQDiPoPJjeKKA = new LocalizedString();
		rKpLIcePJnEQcEqBcjkFEAaEAoIzA = new Dictionary<int, List<OBzZblFCcoXjpegrNwgWHpLkWsfD>>();
	}

	protected FopEdGIdXdIETmMGGpfJptNngDfeb(bguKJVtsagJfXPpJQeurpzlOLIYd P_0)
		: this()
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("dataSource");
		}
		IGYnQTyrLaOlJSkcwVhRDOUmXbur = P_0;
	}

	public void bIVZUTIzQVeRSNEzqyWioRbktgUX()
	{
		ZdBgfEeqqkPJHSqpfMhEfMocFnxqB();
		if (LocalizationManager.isEnabled && LocalizationManager.autoPrefetch)
		{
			eCoUKPYzeytCrbuDxEwhAldiAbYf();
		}
	}

	protected virtual void ZdBgfEeqqkPJHSqpfMhEfMocFnxqB()
	{
		BtfNvejKWVTjEMOvbuWnNhEjRQIj();
		osxHrggonzizrFGCtREPyqJvvdnM();
		LocalizationManager.Add(this, ref jLVmRFZefgaSuSroXIuoDZbWUXufA);
		rYohYODoYEitENdaKrwOVgjoIrVR = true;
	}

	public virtual void BtfNvejKWVTjEMOvbuWnNhEjRQIj()
	{
		uCzgBTyTNNdCVFbBSFinxXiSmemm();
		LocalizationManager.Remove(ref jLVmRFZefgaSuSroXIuoDZbWUXufA);
		rYohYODoYEitENdaKrwOVgjoIrVR = false;
	}

	public virtual void FSygSkZkOZRlfprpDasbAbVdqyGH(bguKJVtsagJfXPpJQeurpzlOLIYd P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("value");
		}
		if (P_0 != IGYnQTyrLaOlJSkcwVhRDOUmXbur)
		{
			if (IGYnQTyrLaOlJSkcwVhRDOUmXbur != null)
			{
				uCzgBTyTNNdCVFbBSFinxXiSmemm();
			}
			IGYnQTyrLaOlJSkcwVhRDOUmXbur = P_0;
			bIVZUTIzQVeRSNEzqyWioRbktgUX();
		}
	}

	public virtual void mSjKwPJiyMmQSynHcMWYSOiGAmDB()
	{
		cxVNsagEchUjISqSPQDiPoPJjeKKA.Clear();
	}

	public virtual void hPAqnroLHIcbmjXldArpEMTOSiTdb()
	{
		cxVNsagEchUjISqSPQDiPoPJjeKKA.Clear();
	}

	public virtual void YSgvMmquHVoFhixWnSsVWmcflge()
	{
		cxVNsagEchUjISqSPQDiPoPJjeKKA.Clear();
	}

	public virtual bool PstXrjtELzTCScyWAcSsHTXMaNuK(FopEdGIdXdIETmMGGpfJptNngDfeb P_0, bool P_1)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (!object.Equals(GetType(), P_0.GetType()))
		{
			return false;
		}
		if (IGYnQTyrLaOlJSkcwVhRDOUmXbur == null != (P_0.IGYnQTyrLaOlJSkcwVhRDOUmXbur == null))
		{
			return false;
		}
		if (IGYnQTyrLaOlJSkcwVhRDOUmXbur != null)
		{
			if (!string.Equals(IGYnQTyrLaOlJSkcwVhRDOUmXbur.keyCategory, P_0.IGYnQTyrLaOlJSkcwVhRDOUmXbur.keyCategory, StringComparison.Ordinal) || !string.Equals(IGYnQTyrLaOlJSkcwVhRDOUmXbur.scriptingName, P_0.IGYnQTyrLaOlJSkcwVhRDOUmXbur.scriptingName, StringComparison.Ordinal) || !string.Equals(IGYnQTyrLaOlJSkcwVhRDOUmXbur.key, P_0.IGYnQTyrLaOlJSkcwVhRDOUmXbur.key, StringComparison.Ordinal))
			{
				return false;
			}
			if (P_1 && !string.Equals(IGYnQTyrLaOlJSkcwVhRDOUmXbur.nonLocalizedDescriptiveName, P_0.IGYnQTyrLaOlJSkcwVhRDOUmXbur.nonLocalizedDescriptiveName, StringComparison.Ordinal))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual void uCzgBTyTNNdCVFbBSFinxXiSmemm()
	{
		cxVNsagEchUjISqSPQDiPoPJjeKKA.Clear();
		rKpLIcePJnEQcEqBcjkFEAaEAoIzA.Clear();
	}

	protected bguKJVtsagJfXPpJQeurpzlOLIYd tFHgnqTfapMzUfeUcgrtSgOorhTm()
	{
		return IGYnQTyrLaOlJSkcwVhRDOUmXbur;
	}

	protected virtual void eCoUKPYzeytCrbuDxEwhAldiAbYf()
	{
		_ = MpfwJMTclVnnxEuHhBPCmlxJadkBA;
	}

	void cAwfhgIDGfMqIqwFGxVCNiWfViqT.Localize()
	{
		eCoUKPYzeytCrbuDxEwhAldiAbYf();
	}

	protected virtual void xGNDJHgrbsPfNhcEFUzykpVDiHjoD(int P_0)
	{
	}

	protected virtual void osxHrggonzizrFGCtREPyqJvvdnM()
	{
	}

	protected virtual void gHHpnMuGywauOByTsYtKEwUhbDMtA(int P_0, OBzZblFCcoXjpegrNwgWHpLkWsfD P_1)
	{
		for (int i = 0; i < 32; i++)
		{
			int num = 1 << i;
			if ((P_0 & num) != 0)
			{
				if (!rKpLIcePJnEQcEqBcjkFEAaEAoIzA.TryGetValue(num, out var value))
				{
					value = new List<OBzZblFCcoXjpegrNwgWHpLkWsfD>();
					rKpLIcePJnEQcEqBcjkFEAaEAoIzA[num] = value;
				}
				if (!value.Contains(P_1))
				{
					value.Add(P_1);
				}
			}
		}
	}

	protected virtual void ePZAGjDlOynbvwdYXeWLbhBvbUvj(int P_0, OBzZblFCcoXjpegrNwgWHpLkWsfD P_1)
	{
		for (int i = 0; i < 32; i++)
		{
			int num = 1 << i;
			if ((P_0 & num) == 0 || !rKpLIcePJnEQcEqBcjkFEAaEAoIzA.TryGetValue(num, out var value))
			{
				continue;
			}
			for (int num2 = value.Count - 1; num2 >= 0; num2--)
			{
				if (OBzZblFCcoXjpegrNwgWHpLkWsfD.UsziCfYSdUGLhHjbNMJgSgHZBtLZA(value[num2], P_1))
				{
					value.RemoveAt(num2);
				}
			}
		}
	}

	protected virtual void WNOKfMLBcQgmwLBfRHLEkGrCnlHM(int P_0)
	{
		for (int i = 0; i < 32; i++)
		{
			int num = 1 << i;
			if ((P_0 & num) == 0 || !rKpLIcePJnEQcEqBcjkFEAaEAoIzA.TryGetValue(num, out var value))
			{
				continue;
			}
			int count = value.Count;
			for (int j = 0; j < count; j++)
			{
				if (value[j].zYaGNhhcsCIUvDXBxclPthDHCMVvA != 0)
				{
					xGNDJHgrbsPfNhcEFUzykpVDiHjoD(value[j].zYaGNhhcsCIUvDXBxclPthDHCMVvA);
				}
				if (value[j].KOtNCnhieDYUVmamgrTOUjanaOIu != null)
				{
					value[j].KOtNCnhieDYUVmamgrTOUjanaOIu.Clear();
				}
			}
		}
	}
}
