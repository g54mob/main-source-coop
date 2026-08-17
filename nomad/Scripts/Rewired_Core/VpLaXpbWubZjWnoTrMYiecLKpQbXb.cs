using System.Text;
using Rewired;
using Rewired.Internal.Glyphs;
using Rewired.Internal.Localization;

internal static class VpLaXpbWubZjWnoTrMYiecLKpQbXb
{
	internal static bool AWnqcotPesgLaHWXkMOJhtIoTYR(KeyedGlyph P_0, string P_1, string P_2, uint P_3, DeviceLocalizationInfo P_4, cBFxQChnAZFRRQeDStCHagOAAZyI P_5, int P_6, AxisRange P_7, int P_8, out object P_9)
	{
		if (string.IsNullOrEmpty(P_1))
		{
			P_9 = null;
			return false;
		}
		bool result = false;
		uint dependenciesVersion = 0u;
		bool flag = !string.IsNullOrEmpty(P_2);
		StringBuilder sharedStringBuilder = GlyphManager.GetSharedStringBuilder();
		if (P_4 != null && P_4.parentKeys != null)
		{
			for (int i = 0; i < P_4.parentKeys.Count; i++)
			{
				if (string.IsNullOrEmpty(P_4.parentKeys[i]))
				{
					continue;
				}
				sharedStringBuilder.Length = 0;
				if (flag)
				{
					sharedStringBuilder.Append(P_2);
				}
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_4.parentKeys[i]);
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1);
				if (!GlyphManager.TryGetGlyph(P_0, sharedStringBuilder.ToString(), P_3, dependenciesVersion, out P_9))
				{
					continue;
				}
				goto IL_008d;
			}
		}
		if (P_5 != cBFxQChnAZFRRQeDStCHagOAAZyI.Keyboard)
		{
			sharedStringBuilder.Length = 0;
			sharedStringBuilder.Append("controller/element");
			LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1);
			if (GlyphManager.TryGetGlyph(P_0, sharedStringBuilder.ToString(), P_3, dependenciesVersion, out P_9))
			{
				result = true;
				goto IL_0182;
			}
		}
		if (P_5 == cBFxQChnAZFRRQeDStCHagOAAZyI.Joystick && P_6 >= 0 && P_4 != null && P_4.controllerTemplateGuids != null)
		{
			for (int j = 0; j < P_4.controllerTemplateGuids.Count; j++)
			{
				if (!fNDBBZXbOAvGiTXVzfEmFadoOOjj.yJnOYpFmNErczgGQBSLKomAifmFHA(P_4.guid, P_4.controllerTemplateGuids[j], P_6, P_7, P_8, out var _, out var value) || string.IsNullOrEmpty(value))
				{
					continue;
				}
				sharedStringBuilder.Length = 0;
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, "controller/template");
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, value);
				if (!GlyphManager.TryGetGlyph(P_0, sharedStringBuilder.ToString(), P_3, dependenciesVersion, out P_9))
				{
					continue;
				}
				goto IL_0164;
			}
		}
		P_9 = null;
		goto IL_0182;
		IL_0164:
		result = true;
		goto IL_0182;
		IL_008d:
		result = true;
		goto IL_0182;
		IL_0182:
		P_0.cachedValue = P_9;
		return result;
	}

	internal static GlyphManager.GetAndUpdateGlyphResultFlags gjMLaFWJEEdJqjcgpgIUDoroPEIDb(KeyedGlyph P_0, string P_1, string P_2, DeviceLocalizationInfo P_3, cBFxQChnAZFRRQeDStCHagOAAZyI P_4, int P_5, AxisRange P_6, int P_7, out object P_8)
	{
		if (!GlyphManager.isEnabled)
		{
			P_8 = null;
			return GlyphManager.GetAndUpdateGlyphResultFlags.Failed;
		}
		GlyphManager.GetAndUpdateGlyphResultFlags getAndUpdateGlyphResultFlags = ((!GlyphManager.TryGetCachedGlyph(P_0, GlyphManager.version, 0u, out var glyphProviderVersionChanged, out P_8)) ? GlyphManager.GetAndUpdateGlyphResultFlags.Failed : GlyphManager.GetAndUpdateGlyphResultFlags.IsCachedValue);
		if (!P_0.hasCachedValue || glyphProviderVersionChanged)
		{
			getAndUpdateGlyphResultFlags |= GlyphManager.GetAndUpdateGlyphResultFlags.Changed;
			if (AWnqcotPesgLaHWXkMOJhtIoTYR(P_0, P_1, P_2, GlyphManager.version, P_3, P_4, P_5, P_6, P_7, out P_8))
			{
				getAndUpdateGlyphResultFlags |= GlyphManager.GetAndUpdateGlyphResultFlags.JustGot;
				getAndUpdateGlyphResultFlags &= (GlyphManager.GetAndUpdateGlyphResultFlags)(-2);
			}
			else
			{
				getAndUpdateGlyphResultFlags |= GlyphManager.GetAndUpdateGlyphResultFlags.Failed;
			}
		}
		return getAndUpdateGlyphResultFlags;
	}
}
