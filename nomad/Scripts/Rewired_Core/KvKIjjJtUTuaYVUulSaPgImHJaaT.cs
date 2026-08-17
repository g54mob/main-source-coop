using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils.Classes.Data;

internal sealed class KvKIjjJtUTuaYVUulSaPgImHJaaT : IHardwareControllerTemplateMap, IHardwareControllerTemplateMap_Internal
{
	private struct RHqqDjfxNbWPIVXdkwqSmFTxOWWL : IControllerTemplateMapSpecialElement_Internal
	{
		private IControllerTemplateMapSpecialElement_Internal YdoLvKNSGHuIAJNuZvJOYpVpYrWm;

		public RHqqDjfxNbWPIVXdkwqSmFTxOWWL(IControllerTemplateMapSpecialElement_Internal P_0)
		{
			YdoLvKNSGHuIAJNuZvJOYpVpYrWm = P_0;
		}

		public T GetMapping<T>() where T : ControllerTemplateSpecialElementMapping
		{
			return YdoLvKNSGHuIAJNuZvJOYpVpYrWm.GetMapping<T>();
		}

		T IControllerTemplateMapSpecialElement_Internal.GetMapping<T>()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetMapping
			return this.GetMapping<T>();
		}
	}

	private HardwareJoystickTemplateMap yaWAUthCkBNEZHdJOmETGCDIFCPUB;

	private string SRTaaXozoBNzoJEceMLwPDeIEHro;

	private string ZcbseKSsQzNAphfQQlQFrKWkLfS;

	private string MKtoZPIGCacgOTbJEGwTpsIWAAOK;

	private readonly Guid szuaWMepgdNKogkSawmClINWmBpJB;

	private readonly List<HardwareJoystickTemplateMap.Entry> sgXmOjkJBNpXCEfPbpRJonriaHgG;

	private readonly ControllerTemplateElementIdentifier[] RtlPjUhXsThuTSnDbVqwEKEitKCD;

	private readonly DeviceLocalizationInfo RuFXFaBHQFNVCYtGgteLaGVBIrbo;

	[NonSerialized]
	private Func<Guid, HardwareJoystickTemplateMap.Entry> dXBJWZvHvYDJtyQaLPeawDBHDvFdA;

	public string name => SRTaaXozoBNzoJEceMLwPDeIEHro;

	public string wXnvrcQpjzGQLpuTKnvzchYGVJMT => ZcbseKSsQzNAphfQQlQFrKWkLfS;

	public string QFzrzCoIZhzyyzqtbcGkSeySSgJc => MKtoZPIGCacgOTbJEGwTpsIWAAOK;

	public Guid EYmvhOajHQdZnATWhPkTwQeosUoab => szuaWMepgdNKogkSawmClINWmBpJB;

	string IHardwareControllerTemplateMap_Internal.name => SRTaaXozoBNzoJEceMLwPDeIEHro;

	Guid IHardwareControllerTemplateMap_Internal.typeGuid => szuaWMepgdNKogkSawmClINWmBpJB;

	string IHardwareControllerTemplateMap_Internal.typeKey => MKtoZPIGCacgOTbJEGwTpsIWAAOK;

	private Func<Guid, HardwareJoystickTemplateMap.Entry> WkJqSsMsSSYzUkQeoIdrKBMWboXN => RuecDnpwhBINVByaYoXbiqHWmPzP;

	public KvKIjjJtUTuaYVUulSaPgImHJaaT(HardwareJoystickTemplateMap P_0, List<HardwareJoystickTemplateMap.Entry> P_1, ControllerTemplateElementIdentifier[] P_2)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException();
		}
		if (P_1 == null)
		{
			throw new ArgumentNullException();
		}
		if (P_2 == null)
		{
			throw new ArgumentNullException();
		}
		yaWAUthCkBNEZHdJOmETGCDIFCPUB = P_0;
		SRTaaXozoBNzoJEceMLwPDeIEHro = P_0.name;
		ZcbseKSsQzNAphfQQlQFrKWkLfS = P_0.ClassName;
		szuaWMepgdNKogkSawmClINWmBpJB = P_0.Guid;
		MKtoZPIGCacgOTbJEGwTpsIWAAOK = P_0.Key;
		sgXmOjkJBNpXCEfPbpRJonriaHgG = P_1;
		RtlPjUhXsThuTSnDbVqwEKEitKCD = P_2;
		RuFXFaBHQFNVCYtGgteLaGVBIrbo = new DeviceLocalizationInfo(ControllerType.Joystick, true, EYmvhOajHQdZnATWhPkTwQeosUoab, new AList<string> { MKtoZPIGCacgOTbJEGwTpsIWAAOK }, null);
		RuFXFaBHQFNVCYtGgteLaGVBIrbo.FinishRuntimeSetup();
		bool flag = RuFXFaBHQFNVCYtGgteLaGVBIrbo.controllerType != ControllerType.Keyboard && RuFXFaBHQFNVCYtGgteLaGVBIrbo.controllerType != ControllerType.Mouse;
		for (int i = 0; i < RtlPjUhXsThuTSnDbVqwEKEitKCD.Length; i++)
		{
			if (RtlPjUhXsThuTSnDbVqwEKEitKCD[i] == null)
			{
				continue;
			}
			if (flag)
			{
				if (ControllerTemplateElementIdentifier.KJOGhtbCWHYIMNeHPDRgnNjpKzzM.tAoOtQtEpTVwRHgkRThRXyhYPFVF(RuFXFaBHQFNVCYtGgteLaGVBIrbo, RtlPjUhXsThuTSnDbVqwEKEitKCD[i], out var controllerTemplateElementIdentifier))
				{
					RtlPjUhXsThuTSnDbVqwEKEitKCD[i] = controllerTemplateElementIdentifier;
					continue;
				}
				ControllerTemplateElementIdentifier.KJOGhtbCWHYIMNeHPDRgnNjpKzzM.mnAadYcNJqnJqhQbeWDVcbLdGqdAC(RuFXFaBHQFNVCYtGgteLaGVBIrbo, RtlPjUhXsThuTSnDbVqwEKEitKCD[i]);
			}
			RtlPjUhXsThuTSnDbVqwEKEitKCD[i].FinishRuntimeSetup(RuFXFaBHQFNVCYtGgteLaGVBIrbo);
		}
	}

	public ControllerTemplateElementIdentifier uxbBsIjVGbgawGCwyaIzDNPgjMDX(Guid P_0, int P_1)
	{
		if (P_0 == Guid.Empty || P_1 < 0)
		{
			return null;
		}
		if (sgXmOjkJBNpXCEfPbpRJonriaHgG == null)
		{
			return null;
		}
		int num = -1;
		int count = sgXmOjkJBNpXCEfPbpRJonriaHgG.Count;
		for (int i = 0; i < count; i++)
		{
			if (sgXmOjkJBNpXCEfPbpRJonriaHgG[i] != null && sgXmOjkJBNpXCEfPbpRJonriaHgG[i].JoystickGuid == P_0)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return null;
		}
		HardwareJoystickTemplateMap.Entry entry = sgXmOjkJBNpXCEfPbpRJonriaHgG[num];
		if (entry == null)
		{
			return null;
		}
		int templateElementId = entry.GetTemplateElementId(P_1);
		if (templateElementId < 0)
		{
			return null;
		}
		return HardwareJoystickTemplateMap.iGsjNtkQyEAQlmykvIoJyfsgNozS(RtlPjUhXsThuTSnDbVqwEKEitKCD, templateElementId);
	}

	public int AbfyGtDPmkFJdlqfZfSPkPWsokPR(Guid P_0, int P_1, List<HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA> P_2)
	{
		if (P_2 == null)
		{
			throw new ArgumentNullException("results");
		}
		if (P_0 == Guid.Empty || P_1 < 0)
		{
			return 0;
		}
		if (sgXmOjkJBNpXCEfPbpRJonriaHgG == null)
		{
			return 0;
		}
		int num = -1;
		int count = sgXmOjkJBNpXCEfPbpRJonriaHgG.Count;
		for (int i = 0; i < count; i++)
		{
			if (sgXmOjkJBNpXCEfPbpRJonriaHgG[i] != null && sgXmOjkJBNpXCEfPbpRJonriaHgG[i].JoystickGuid == P_0)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return 0;
		}
		HardwareJoystickTemplateMap.Entry entry = sgXmOjkJBNpXCEfPbpRJonriaHgG[num];
		if (entry == null)
		{
			return 0;
		}
		int count2 = P_2.Count;
		int num2 = ((entry.elementIdentifierMappings != null) ? entry.elementIdentifierMappings.Count : 0);
		for (int j = 0; j < num2; j++)
		{
			if (entry.elementIdentifierMappings == null)
			{
				continue;
			}
			HardwareJoystickTemplateMap.ElementIdentifierMap elementIdentifierMap = entry.elementIdentifierMappings[j];
			if (elementIdentifierMap == null)
			{
				continue;
			}
			ControllerTemplateElementIdentifier controllerTemplateElementIdentifier = SZuTyoigNzcgKLDChhBFRwdkdhKEA(RtlPjUhXsThuTSnDbVqwEKEitKCD, elementIdentifierMap.templateId);
			if (controllerTemplateElementIdentifier == null)
			{
				continue;
			}
			if (controllerTemplateElementIdentifier.Rewired_002EInterfaces_002EIControllerTemplateElementIdentifier_002EelementType == ControllerTemplateElementType.Axis)
			{
				if (elementIdentifierMap.splitAxis)
				{
					if (elementIdentifierMap.joystickId != P_1 && elementIdentifierMap.joystickId2 != P_1)
					{
						continue;
					}
				}
				else if (elementIdentifierMap.joystickId != P_1)
				{
					continue;
				}
			}
			else if (elementIdentifierMap.joystickId != P_1)
			{
				continue;
			}
			P_2.Add(new HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA
			{
				IhtpnYRlqMiPrMNxIAdtxyiCeyrh = elementIdentifierMap.templateId,
				mfSwFnJqaubVGckClgYswnorqksAb = elementIdentifierMap.joystickId,
				bKiSaMoXBEBBrYElcYinrFOxWVty = elementIdentifierMap.joystickId2,
				dJsxBLjaWpYIjFpaxNcnMfgakBeg = (controllerTemplateElementIdentifier.Rewired_002EInterfaces_002EIControllerTemplateElementIdentifier_002EelementType == ControllerTemplateElementType.Axis && elementIdentifierMap.splitAxis)
			});
		}
		return P_2.Count - count2;
	}

	private HardwareJoystickTemplateMap.Entry RuecDnpwhBINVByaYoXbiqHWmPzP(Guid P_0)
	{
		if (sgXmOjkJBNpXCEfPbpRJonriaHgG == null)
		{
			return null;
		}
		for (int i = 0; i < sgXmOjkJBNpXCEfPbpRJonriaHgG.Count; i++)
		{
			if (sgXmOjkJBNpXCEfPbpRJonriaHgG[i].JoystickGuid == P_0)
			{
				return sgXmOjkJBNpXCEfPbpRJonriaHgG[i];
			}
		}
		return null;
	}

	private static ControllerTemplateElementIdentifier SZuTyoigNzcgKLDChhBFRwdkdhKEA(ControllerTemplateElementIdentifier[] P_0, int P_1)
	{
		if (P_0 == null)
		{
			return null;
		}
		for (int i = 0; i < P_0.Length; i++)
		{
			if (P_0[i] != null && P_0[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == P_1)
			{
				return P_0[i];
			}
		}
		return null;
	}

	int IHardwareControllerTemplateMap_Internal.GetElementIdentifierCount()
	{
		if (RtlPjUhXsThuTSnDbVqwEKEitKCD == null)
		{
			return 0;
		}
		return RtlPjUhXsThuTSnDbVqwEKEitKCD.Length;
	}

	IControllerTemplateElementIdentifier IHardwareControllerTemplateMap_Internal.GetTemplateElementIdentifier(int index)
	{
		if (RtlPjUhXsThuTSnDbVqwEKEitKCD == null)
		{
			return null;
		}
		return RtlPjUhXsThuTSnDbVqwEKEitKCD[index];
	}

	IControllerTemplateElementIdentifier IHardwareControllerTemplateMap_Internal.GetTemplateElementIdentifierById(int elementIdentifierId)
	{
		return HardwareJoystickTemplateMap.iGsjNtkQyEAQlmykvIoJyfsgNozS(RtlPjUhXsThuTSnDbVqwEKEitKCD, elementIdentifierId);
	}

	IControllerTemplateMapSpecialElement_Internal IHardwareControllerTemplateMap_Internal.GetSpecialTemplateElementByElementIdentifierId(int id)
	{
		return new RHqqDjfxNbWPIVXdkwqSmFTxOWWL(((IHardwareControllerTemplateMap_Internal)yaWAUthCkBNEZHdJOmETGCDIFCPUB).GetSpecialTemplateElementByElementIdentifierId(id));
	}

	xcslkxDzwrCojABLPbRuUvYdnRhl IHardwareControllerTemplateMap_Internal.GetAxisTarget(Controller controller, int elementIdentifierId)
	{
		return HardwareJoystickTemplateMap.TqSZZklISXOXmzpoVaOudkFgERsT(this, controller, elementIdentifierId, WkJqSsMsSSYzUkQeoIdrKBMWboXN);
	}

	xcslkxDzwrCojABLPbRuUvYdnRhl IHardwareControllerTemplateMap_Internal.GetButtonTarget(Controller controller, int elementIdentifierId)
	{
		return HardwareJoystickTemplateMap.kzZTgeoEHFvyChwYHvlmIUvKsdaL(this, controller, elementIdentifierId, WkJqSsMsSSYzUkQeoIdrKBMWboXN);
	}
}
