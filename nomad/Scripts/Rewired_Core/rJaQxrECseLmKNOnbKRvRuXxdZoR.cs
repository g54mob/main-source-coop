using System;
using Rewired;
using Rewired.Internal.Localization;

internal sealed class rJaQxrECseLmKNOnbKRvRuXxdZoR : FDNFDGKMldROgCHjPdSVTnUzAnLgb, cBQeMhYqRgOCwlnJCsFnDXPZyIWh
{
	private readonly cBFxQChnAZFRRQeDStCHagOAAZyI JcygUeAWezcaUcWnwlJzOyiFfKPB;

	private readonly int EzxfQhsPcXePZMGzZMIXRFPQjnzt;

	private DeviceLocalizationInfo UyaOYosvAlJFwdNKzwRCzgrJghjeA;

	string FopEdGIdXdIETmMGGpfJptNngDfeb.MpfwJMTclVnnxEuHhBPCmlxJadkBA
	{
		get
		{
			bguKJVtsagJfXPpJQeurpzlOLIYd bguKJVtsagJfXPpJQeurpzlOLIYd2 = tFHgnqTfapMzUfeUcgrtSgOorhTm();
			if (bguKJVtsagJfXPpJQeurpzlOLIYd2 == null)
			{
				return string.Empty;
			}
			if (!LocalizationManager.isEnabled)
			{
				return bguKJVtsagJfXPpJQeurpzlOLIYd2.nonLocalizedDescriptiveName;
			}
			LocalizationManager.TryGetCachedLocalizedString(cxVNsagEchUjISqSPQDiPoPJjeKKA, bguKJVtsagJfXPpJQeurpzlOLIYd2.nonLocalizedDescriptiveName, LocalizationManager.version, 0u, out var localizationVersionChanged, out var result);
			if (!cxVNsagEchUjISqSPQDiPoPJjeKKA.hasCachedValue || localizationVersionChanged)
			{
				fNDBBZXbOAvGiTXVzfEmFadoOOjj.XtzQdtNYRtRlONoIGaqPAeMEGfYmA(cxVNsagEchUjISqSPQDiPoPJjeKKA, bguKJVtsagJfXPpJQeurpzlOLIYd2.key, fNDBBZXbOAvGiTXVzfEmFadoOOjj.LNBXTycUoZmmAIjTIwrhZoijHURb(JcygUeAWezcaUcWnwlJzOyiFfKPB), bguKJVtsagJfXPpJQeurpzlOLIYd2.nonLocalizedDescriptiveName, LocalizationManager.version, UyaOYosvAlJFwdNKzwRCzgrJghjeA, JcygUeAWezcaUcWnwlJzOyiFfKPB, EzxfQhsPcXePZMGzZMIXRFPQjnzt, AxisRange.Full, -1, out result);
			}
			return result;
		}
	}

	DeviceLocalizationInfo cBQeMhYqRgOCwlnJCsFnDXPZyIWh.NoNdGdFDpUCLxyUmgslGWEGfuOYG => UyaOYosvAlJFwdNKzwRCzgrJghjeA;

	private rJaQxrECseLmKNOnbKRvRuXxdZoR(cBFxQChnAZFRRQeDStCHagOAAZyI P_0, sztzDKprOgaEtSRoFjITTczsHDuW P_1, LsWebCorzTdhEUjUrAlgVzPmJJHR P_2, int P_3, DeviceLocalizationInfo P_4)
		: base(P_1, P_2)
	{
		if (P_4 == null)
		{
			throw new ArgumentNullException();
		}
		JcygUeAWezcaUcWnwlJzOyiFfKPB = P_0;
		EzxfQhsPcXePZMGzZMIXRFPQjnzt = P_3;
		UyaOYosvAlJFwdNKzwRCzgrJghjeA = P_4;
	}

	private rJaQxrECseLmKNOnbKRvRuXxdZoR(lOhdpMIGSdyahJLjLKbbeUkHQJxnB P_0, cBFxQChnAZFRRQeDStCHagOAAZyI P_1, sztzDKprOgaEtSRoFjITTczsHDuW P_2, LsWebCorzTdhEUjUrAlgVzPmJJHR P_3, int P_4, DeviceLocalizationInfo P_5)
		: base(P_0, P_2, P_3)
	{
		if (P_5 == null)
		{
			throw new ArgumentNullException();
		}
		JcygUeAWezcaUcWnwlJzOyiFfKPB = P_1;
		EzxfQhsPcXePZMGzZMIXRFPQjnzt = P_4;
		UyaOYosvAlJFwdNKzwRCzgrJghjeA = P_5;
	}

	public static rJaQxrECseLmKNOnbKRvRuXxdZoR oqrPXVFddmGZZCIUEBtKUvjlPOvCA(cBFxQChnAZFRRQeDStCHagOAAZyI P_0, sztzDKprOgaEtSRoFjITTczsHDuW P_1, LsWebCorzTdhEUjUrAlgVzPmJJHR P_2, int P_3, DeviceLocalizationInfo P_4)
	{
		return new rJaQxrECseLmKNOnbKRvRuXxdZoR(P_0, P_1, P_2, P_3, P_4);
	}

	public static rJaQxrECseLmKNOnbKRvRuXxdZoR ozJQOxKnUjJVmDMAdnhFgCNyVyFO(lOhdpMIGSdyahJLjLKbbeUkHQJxnB P_0, cBFxQChnAZFRRQeDStCHagOAAZyI P_1, sztzDKprOgaEtSRoFjITTczsHDuW P_2, LsWebCorzTdhEUjUrAlgVzPmJJHR P_3, int P_4, DeviceLocalizationInfo P_5)
	{
		rJaQxrECseLmKNOnbKRvRuXxdZoR obj = new rJaQxrECseLmKNOnbKRvRuXxdZoR(P_0, P_1, P_2, P_3, P_4, P_5);
		obj.bIVZUTIzQVeRSNEzqyWioRbktgUX();
		return obj;
	}

	public bool pdKtZPyaDDKaDUYpWSfLwOAjYvel(FopEdGIdXdIETmMGGpfJptNngDfeb P_0, bool P_1)
	{
		if (!(P_0 is rJaQxrECseLmKNOnbKRvRuXxdZoR rJaQxrECseLmKNOnbKRvRuXxdZoR2))
		{
			return false;
		}
		if (!TIbllctYhBPpFlqcVRhFKAQKVURc(P_0, P_1))
		{
			return false;
		}
		if (JcygUeAWezcaUcWnwlJzOyiFfKPB == rJaQxrECseLmKNOnbKRvRuXxdZoR2.JcygUeAWezcaUcWnwlJzOyiFfKPB && EzxfQhsPcXePZMGzZMIXRFPQjnzt == rJaQxrECseLmKNOnbKRvRuXxdZoR2.EzxfQhsPcXePZMGzZMIXRFPQjnzt)
		{
			return DeviceLocalizationInfo.DataMatches(UyaOYosvAlJFwdNKzwRCzgrJghjeA, rJaQxrECseLmKNOnbKRvRuXxdZoR2.UyaOYosvAlJFwdNKzwRCzgrJghjeA);
		}
		return false;
	}
}
