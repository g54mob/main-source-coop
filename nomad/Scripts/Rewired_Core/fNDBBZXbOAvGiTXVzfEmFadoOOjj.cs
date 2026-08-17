using System;
using System.Collections.Generic;
using System.Text;
using Rewired;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils;

internal static class fNDBBZXbOAvGiTXVzfEmFadoOOjj
{
	internal static string ZjyEVyERnmGwvaLVfGpAagVLJQHN(ControllerType P_0)
	{
		return LNBXTycUoZmmAIjTIwrhZoijHURb(wAIJUtJnRDeoKIthCveEEyAfjePsA.EvNqTyCxDnMyOBrFuMukHLvYIQjU(P_0));
	}

	internal static string LNBXTycUoZmmAIjTIwrhZoijHURb(cBFxQChnAZFRRQeDStCHagOAAZyI P_0)
	{
		switch (P_0)
		{
		case cBFxQChnAZFRRQeDStCHagOAAZyI.Joystick:
		case cBFxQChnAZFRRQeDStCHagOAAZyI.Keyboard:
		case cBFxQChnAZFRRQeDStCHagOAAZyI.Mouse:
			return "controller";
		case cBFxQChnAZFRRQeDStCHagOAAZyI.CustomController:
			return "controller/custom";
		case cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate:
			return "controller/template";
		default:
			throw new NotImplementedException();
		}
	}

	internal static bool XtzQdtNYRtRlONoIGaqPAeMEGfYmA(LocalizedString P_0, string P_1, string P_2, string P_3, uint P_4, DeviceLocalizationInfo P_5, cBFxQChnAZFRRQeDStCHagOAAZyI P_6, int P_7, AxisRange P_8, int P_9, out string P_10)
	{
		if (string.IsNullOrEmpty(P_1))
		{
			P_10 = P_3;
			return false;
		}
		bool result = false;
		uint dependenciesVersion = 0u;
		bool flag = !string.IsNullOrEmpty(P_2);
		StringBuilder sharedStringBuilder = LocalizationManager.GetSharedStringBuilder();
		if (P_5 != null && P_5.parentKeys != null)
		{
			for (int i = 0; i < P_5.parentKeys.Count; i++)
			{
				if (string.IsNullOrEmpty(P_5.parentKeys[i]))
				{
					continue;
				}
				sharedStringBuilder.Length = 0;
				if (flag)
				{
					sharedStringBuilder.Append(P_2);
				}
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_5.parentKeys[i]);
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1);
				if (!LocalizationManager.TryLocalizeString(P_0, sharedStringBuilder.ToString(), P_4, dependenciesVersion, out P_10))
				{
					continue;
				}
				goto IL_008e;
			}
		}
		if (P_6 != cBFxQChnAZFRRQeDStCHagOAAZyI.Keyboard)
		{
			sharedStringBuilder.Length = 0;
			sharedStringBuilder.Append("controller/element");
			LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1);
			if (LocalizationManager.TryLocalizeString(P_0, sharedStringBuilder.ToString(), P_4, dependenciesVersion, out P_10))
			{
				result = true;
				goto IL_0185;
			}
		}
		if (P_6 == cBFxQChnAZFRRQeDStCHagOAAZyI.Joystick && P_7 >= 0 && P_5 != null && P_5.controllerTemplateGuids != null)
		{
			for (int j = 0; j < P_5.controllerTemplateGuids.Count; j++)
			{
				if (!yJnOYpFmNErczgGQBSLKomAifmFHA(P_5.guid, P_5.controllerTemplateGuids[j], P_7, P_8, P_9, out var _, out var value) || string.IsNullOrEmpty(value))
				{
					continue;
				}
				sharedStringBuilder.Length = 0;
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, "controller/template");
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, value);
				if (!LocalizationManager.TryLocalizeString(P_0, sharedStringBuilder.ToString(), P_4, dependenciesVersion, out P_10))
				{
					continue;
				}
				goto IL_0167;
			}
		}
		P_10 = P_3;
		goto IL_0185;
		IL_0167:
		result = true;
		goto IL_0185;
		IL_008e:
		result = true;
		goto IL_0185;
		IL_0185:
		P_0.cachedValue = P_10;
		return result;
	}

	public static bool yJnOYpFmNErczgGQBSLKomAifmFHA(Guid P_0, Guid P_1, int P_2, AxisRange P_3, int P_4, out string P_5, out string P_6)
	{
		if (!(ReInput.IfizLymKkTakdnRWaNLVnCtshNql(P_1) is IHardwareControllerTemplateMap_Internal hardwareControllerTemplateMap_Internal))
		{
			P_6 = null;
			P_5 = null;
			return false;
		}
		using (TempListPool.TList<HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA> tList = TempListPool.GetTList<HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA>())
		{
			List<HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA> list = tList.list;
			ReInput.mapping.YpODnAqLZPqAbLeGSTAMeqcCIwgC(P_1, P_0, P_2, list);
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA jiuvLTxcDzWSddWpdRJbLPDWISiIA = list[i];
				IControllerTemplateElementIdentifier templateElementIdentifierById = hardwareControllerTemplateMap_Internal.GetTemplateElementIdentifierById(jiuvLTxcDzWSddWpdRJbLPDWISiIA.IhtpnYRlqMiPrMNxIAdtxyiCeyrh);
				if (templateElementIdentifierById == null)
				{
					continue;
				}
				if (P_4 >= 0)
				{
					P_5 = templateElementIdentifierById.GetSpecialElementKey(P_4);
				}
				else if (!jiuvLTxcDzWSddWpdRJbLPDWISiIA.dJsxBLjaWpYIjFpaxNcnMfgakBeg || (jiuvLTxcDzWSddWpdRJbLPDWISiIA.mfSwFnJqaubVGckClgYswnorqksAb == P_2 && jiuvLTxcDzWSddWpdRJbLPDWISiIA.bKiSaMoXBEBBrYElcYinrFOxWVty == P_2))
				{
					switch (P_3)
					{
					case AxisRange.Full:
						P_5 = templateElementIdentifierById.key;
						break;
					case AxisRange.Positive:
						P_5 = templateElementIdentifierById.positiveKey;
						break;
					case AxisRange.Negative:
						P_5 = templateElementIdentifierById.negativeKey;
						break;
					default:
						throw new NotImplementedException();
					}
				}
				else
				{
					P_5 = ((jiuvLTxcDzWSddWpdRJbLPDWISiIA.mfSwFnJqaubVGckClgYswnorqksAb == P_2) ? templateElementIdentifierById.positiveKey : templateElementIdentifierById.negativeKey);
				}
				if (!string.IsNullOrEmpty(P_5))
				{
					P_6 = ((!string.IsNullOrEmpty(hardwareControllerTemplateMap_Internal.typeKey)) ? LocalizationManager.AppendToKeyAsPath(hardwareControllerTemplateMap_Internal.typeKey, P_5) : null);
					return true;
				}
			}
		}
		P_6 = null;
		P_5 = null;
		return false;
	}

	internal static LocalizationManager.GetAndUpdateLocalizedStringResultFlags UAWmqofIfwJFRJqjERIiiqQCpcOG(LocalizedString P_0, string P_1, string P_2, string P_3, DeviceLocalizationInfo P_4, cBFxQChnAZFRRQeDStCHagOAAZyI P_5, int P_6, AxisRange P_7, int P_8, out string P_9)
	{
		if (!LocalizationManager.isEnabled)
		{
			P_9 = P_3;
			return LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Failed;
		}
		LocalizationManager.GetAndUpdateLocalizedStringResultFlags getAndUpdateLocalizedStringResultFlags = ((!LocalizationManager.TryGetCachedLocalizedString(P_0, P_3, LocalizationManager.version, 0u, out var localizationVersionChanged, out P_9)) ? LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Failed : LocalizationManager.GetAndUpdateLocalizedStringResultFlags.IsCachedValue);
		if (!P_0.hasCachedValue || localizationVersionChanged)
		{
			getAndUpdateLocalizedStringResultFlags |= LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Changed;
			if (XtzQdtNYRtRlONoIGaqPAeMEGfYmA(P_0, P_1, P_2, P_3, LocalizationManager.version, P_4, P_5, P_6, P_7, P_8, out P_9))
			{
				getAndUpdateLocalizedStringResultFlags |= LocalizationManager.GetAndUpdateLocalizedStringResultFlags.JustLocalized;
				getAndUpdateLocalizedStringResultFlags &= (LocalizationManager.GetAndUpdateLocalizedStringResultFlags)(-2);
			}
			else
			{
				getAndUpdateLocalizedStringResultFlags |= LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Failed;
			}
		}
		return getAndUpdateLocalizedStringResultFlags;
	}
}
