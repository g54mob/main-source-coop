using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Data.Mapping;
using Rewired.Internal.Localization;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class HardwareControllerMap_Game
	{
		private enum BUaaQvOJrhYxPTwFqfcdHiCwToKf
		{
			Error = 0,
			FoundIndex = 1,
			IsWholeElement = 2
		}

		public readonly string controllerName;

		public readonly HardwareControllerMapIdentifier hardwareMapIdentifier;

		public readonly int customControllerSourceId;

		public readonly ADictionary<int, ControllerElementIdentifier> elementIdentifiers;

		public readonly ControllerElementIdentifier[] elementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] buttonElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] axisElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] axis2DElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] hatElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] dpadElementIdentifiers_cache;

		public readonly IList<ControllerElementIdentifier> elementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> buttonElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> axisElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> axis2DElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> hatElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> dpadElementIdentifiers_readOnly;

		public readonly int[] buttonElementIdentifierIds;

		public readonly int[] axisElementIdentifierIds;

		public readonly int[] axis2DElementIdentifierIds;

		public readonly int[] hatElementIdentifierIds;

		public readonly int[] dpadElementIdentifierIds;

		public readonly int elementIdentifierCount;

		public readonly int axisCount;

		public readonly int buttonCount;

		public readonly int compoundElementCount;

		public readonly int axis2DCount;

		public readonly int hatCount;

		public readonly int dpadCount;

		public readonly JoystickType[] joystickTypes;

		public readonly AxisCalibrationData[] hwAxisCalibrationData;

		public readonly AxisRange[] hwAxisRanges;

		public readonly HardwareAxisInfo[] hwAxisInfo;

		public readonly HardwareButtonInfo[] hwButtonInfo;

		public readonly HardwareJoystickMap.CompoundElement[] compoundElements;

		private readonly DeviceLocalizationInfo QdnhJPBVmgagootpXQEMwoIoFqkTA;

		public DeviceLocalizationInfo deviceLocalizationInfo => QdnhJPBVmgagootpXQEMwoIoFqkTA;

		public HardwareControllerMap_Game(string P_0, DeviceLocalizationInfo P_1, int P_2, ControllerElementIdentifier[] P_3, int[] P_4, int[] P_5, AxisCalibrationData[] P_6, AxisRange[] P_7, HardwareAxisInfo[] P_8, HardwareButtonInfo[] P_9, HardwareJoystickMap.CompoundElement[] P_10)
			: this(P_0, P_1, P_3, P_4, P_5, P_6, P_7, P_8, P_9, P_10)
		{
			customControllerSourceId = P_2;
		}

		public HardwareControllerMap_Game(string P_0, DeviceLocalizationInfo P_1, HardwareControllerMapIdentifier P_2, JoystickType[] P_3, ControllerElementIdentifier[] P_4, int[] P_5, int[] P_6, AxisCalibrationData[] P_7, AxisRange[] P_8, HardwareAxisInfo[] P_9, HardwareButtonInfo[] P_10, HardwareJoystickMap.CompoundElement[] P_11)
			: this(P_0, P_1, P_4, P_5, P_6, P_7, P_8, P_9, P_10, P_11)
		{
			hardwareMapIdentifier = P_2;
			if (P_3 == null)
			{
				joystickTypes = new JoystickType[1];
			}
			else
			{
				joystickTypes = ArrayTools.ShallowCopy(P_3);
			}
		}

		public HardwareControllerMap_Game(string P_0, HardwareControllerMapIdentifier P_1, ControllerElementIdentifier[] P_2, int[] P_3, int[] P_4, AxisCalibrationData[] P_5, AxisRange[] P_6, HardwareAxisInfo[] P_7, HardwareButtonInfo[] P_8, HardwareJoystickMap.CompoundElement[] P_9)
			: this(P_0, string.Equals(P_0, "Keyboard", StringComparison.OrdinalIgnoreCase) ? new DeviceLocalizationInfo(ControllerType.Keyboard, false, Consts.hardwareTypeGuid_universalKeyboard, new List<string> { "keyboard" }, null) : (string.Equals(P_0, "Mouse", StringComparison.OrdinalIgnoreCase) ? new DeviceLocalizationInfo(ControllerType.Mouse, false, Consts.hardwareTypeGuid_universalMouse, new List<string> { "mouse" }, null) : new DeviceLocalizationInfo()), P_1, null, P_2, P_3, P_4, P_5, P_6, P_7, P_8, P_9)
		{
		}

		private HardwareControllerMap_Game(string P_0, DeviceLocalizationInfo P_1, ControllerElementIdentifier[] P_2, int[] P_3, int[] P_4, AxisCalibrationData[] P_5, AxisRange[] P_6, HardwareAxisInfo[] P_7, HardwareButtonInfo[] P_8, HardwareJoystickMap.CompoundElement[] P_9)
		{
			controllerName = P_0;
			QdnhJPBVmgagootpXQEMwoIoFqkTA = P_1;
			if (QdnhJPBVmgagootpXQEMwoIoFqkTA == null)
			{
				QdnhJPBVmgagootpXQEMwoIoFqkTA = new DeviceLocalizationInfo();
			}
			QdnhJPBVmgagootpXQEMwoIoFqkTA.FinishRuntimeSetup();
			bool flag = QdnhJPBVmgagootpXQEMwoIoFqkTA.controllerType != ControllerType.Keyboard && QdnhJPBVmgagootpXQEMwoIoFqkTA.controllerType != ControllerType.Mouse;
			for (int i = 0; i < P_2.Length; i++)
			{
				if (P_2[i] == null)
				{
					continue;
				}
				if (flag)
				{
					if (ControllerElementIdentifier.cVMOubmkNChfxInhyRIuBcRKPeHT.GWVbPguLsfUJOoFoiCTSEfcLJAReb(QdnhJPBVmgagootpXQEMwoIoFqkTA, P_2[i], out var controllerElementIdentifier))
					{
						P_2[i] = controllerElementIdentifier;
						continue;
					}
					ControllerElementIdentifier.cVMOubmkNChfxInhyRIuBcRKPeHT.EkxzcoIUeaCajdridSIelNeKoqrx(QdnhJPBVmgagootpXQEMwoIoFqkTA, P_2[i]);
				}
				P_2[i].FinishRuntimeSetup(QdnhJPBVmgagootpXQEMwoIoFqkTA, QdnhJPBVmgagootpXQEMwoIoFqkTA.controllerType);
			}
			elementIdentifierCount = ((P_2 != null) ? P_2.Length : 0);
			int num = ((P_3 != null) ? P_3.Length : 0);
			int num2 = ((P_4 != null) ? P_4.Length : 0);
			P_9 = ArrayTools.DeepClone(P_9);
			compoundElements = P_9;
			compoundElementCount = ((P_9 != null) ? P_9.Length : 0);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			for (int j = 0; j < compoundElementCount; j++)
			{
				if (P_9[j] != null)
				{
					switch (P_9[j].type)
					{
					case CompoundControllerElementType.Axis2D:
						num3++;
						list.Add(P_9[j].elementIdentifier);
						break;
					case CompoundControllerElementType.Hat:
						num4++;
						list2.Add(P_9[j].elementIdentifier);
						HardwareJoystickMap.CompoundElement.SortHatElementsClockwise(P_9[j]);
						break;
					case CompoundControllerElementType.DPad:
						num5++;
						list3.Add(P_9[j].elementIdentifier);
						break;
					}
				}
			}
			int[] array = list.ToArray();
			int[] array2 = list2.ToArray();
			int[] array3 = list3.ToArray();
			elementIdentifiers = new ADictionary<int, ControllerElementIdentifier>(elementIdentifierCount);
			elementIdentifiers_cache = new ControllerElementIdentifier[elementIdentifierCount];
			buttonElementIdentifiers_cache = new ControllerElementIdentifier[num];
			axisElementIdentifiers_cache = new ControllerElementIdentifier[num2];
			axis2DElementIdentifiers_cache = new ControllerElementIdentifier[num3];
			hatElementIdentifiers_cache = new ControllerElementIdentifier[num4];
			dpadElementIdentifiers_cache = new ControllerElementIdentifier[num5];
			elementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(elementIdentifiers_cache);
			buttonElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(buttonElementIdentifiers_cache);
			axisElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(axisElementIdentifiers_cache);
			axis2DElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(axis2DElementIdentifiers_cache);
			hatElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(hatElementIdentifiers_cache);
			dpadElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(dpadElementIdentifiers_cache);
			for (int k = 0; k < elementIdentifierCount; k++)
			{
				elementIdentifiers_cache[k] = P_2[k];
				elementIdentifiers.Add(P_2[k].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid, P_2[k]);
			}
			for (int l = 0; l < num; l++)
			{
				int num6 = KvcuXCEdVrpiDIUtAqOzBpgDDxuA(P_2, P_3[l]);
				if (num6 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					buttonElementIdentifiers_cache[l] = P_2[num6];
				}
			}
			for (int m = 0; m < num2; m++)
			{
				int num7 = KvcuXCEdVrpiDIUtAqOzBpgDDxuA(P_2, P_4[m]);
				if (num7 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					axisElementIdentifiers_cache[m] = P_2[num7];
				}
			}
			for (int n = 0; n < num3; n++)
			{
				int num8 = KvcuXCEdVrpiDIUtAqOzBpgDDxuA(P_2, array[n]);
				if (num8 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					axis2DElementIdentifiers_cache[n] = P_2[num8];
				}
			}
			for (int num9 = 0; num9 < num4; num9++)
			{
				int num10 = KvcuXCEdVrpiDIUtAqOzBpgDDxuA(P_2, array2[num9]);
				if (num10 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					hatElementIdentifiers_cache[num9] = P_2[num10];
				}
			}
			for (int num11 = 0; num11 < num5; num11++)
			{
				int num12 = KvcuXCEdVrpiDIUtAqOzBpgDDxuA(P_2, array3[num11]);
				if (num12 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					dpadElementIdentifiers_cache[num11] = P_2[num12];
				}
			}
			buttonElementIdentifierIds = P_3;
			axisElementIdentifierIds = P_4;
			axis2DElementIdentifierIds = array;
			hatElementIdentifierIds = array2;
			dpadElementIdentifierIds = array3;
			elementIdentifierCount = ((elementIdentifiers != null) ? elementIdentifiers.Count : 0);
			buttonCount = ((P_3 != null) ? P_3.Length : 0);
			axisCount = ((P_4 != null) ? P_4.Length : 0);
			axis2DCount = num3;
			hatCount = num4;
			dpadCount = num5;
			hwAxisCalibrationData = P_5;
			hwAxisRanges = P_6;
			hwAxisInfo = P_7;
			hwButtonInfo = P_8;
		}

		public string GetElementIdentifierName(int elementIdentifierId)
		{
			if (!elementIdentifiers.TryGetValue(elementIdentifierId, out var value))
			{
				return string.Empty;
			}
			return value.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
		}

		public string GetElementIdentifierPositiveName(int elementIdentifierId)
		{
			if (!elementIdentifiers.TryGetValue(elementIdentifierId, out var value))
			{
				return string.Empty;
			}
			return value.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EpositiveName;
		}

		public string GetElementIdentifierNegativeName(int elementIdentifierId)
		{
			if (!elementIdentifiers.TryGetValue(elementIdentifierId, out var value))
			{
				return string.Empty;
			}
			return value.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EnegativeName;
		}

		public int GetAxisIndex(int elementIdentifierId)
		{
			for (int i = 0; i < axisCount; i++)
			{
				if (axisElementIdentifierIds[i] == elementIdentifierId)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetAxisIndex(string elementIdentifierName)
		{
			if (elementIdentifierName == null || elementIdentifierName == string.Empty)
			{
				return -1;
			}
			int count = elementIdentifiers.Count;
			for (int i = 0; i < count; i++)
			{
				if (elementIdentifiers_cache[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename.Equals(elementIdentifierName, StringComparison.OrdinalIgnoreCase))
				{
					return GetAxisIndex(elementIdentifiers_cache[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid);
				}
			}
			return -1;
		}

		public int GetButtonIndex(int elementIdentifierId)
		{
			for (int i = 0; i < buttonCount; i++)
			{
				if (buttonElementIdentifierIds[i] == elementIdentifierId)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetButtonIndex(string elementIdentifierName)
		{
			if (elementIdentifierName == null || elementIdentifierName == string.Empty)
			{
				return -1;
			}
			int count = elementIdentifiers.Count;
			for (int i = 0; i < count; i++)
			{
				if (elementIdentifiers_cache[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename.Equals(elementIdentifierName, StringComparison.OrdinalIgnoreCase))
				{
					return GetButtonIndex(elementIdentifiers_cache[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid);
				}
			}
			return -1;
		}

		public ControllerElementIdentifier GetElementIdentifierById(int id)
		{
			int count = elementIdentifiers.Count;
			for (int i = 0; i < count; i++)
			{
				if (elementIdentifiers_cache[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == id)
				{
					return elementIdentifiers_cache[i];
				}
			}
			return null;
		}

		public ControllerElementIdentifier GetButtonElementIdentifierById(int id)
		{
			int num = buttonCount;
			for (int i = 0; i < num; i++)
			{
				if (buttonElementIdentifierIds[i] == id)
				{
					return buttonElementIdentifiers_cache[i];
				}
			}
			return null;
		}

		public ControllerElementIdentifier GetAxisElementIdentifierById(int id)
		{
			int num = axisCount;
			for (int i = 0; i < num; i++)
			{
				if (axisElementIdentifierIds[i] == id)
				{
					return axisElementIdentifiers_cache[i];
				}
			}
			return null;
		}

		public HardwareJoystickMap.CompoundElement GetAxis2DData(int index)
		{
			if (compoundElements == null)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < compoundElements.Length; i++)
			{
				if (compoundElements[i] != null && compoundElements[i].type == CompoundControllerElementType.Axis2D)
				{
					if (num == index)
					{
						return compoundElements[i];
					}
					num++;
				}
			}
			return null;
		}

		public HardwareJoystickMap.CompoundElement GetHatData(int index)
		{
			if (compoundElements == null)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < compoundElements.Length; i++)
			{
				if (compoundElements[i] != null && compoundElements[i].type == CompoundControllerElementType.Hat)
				{
					if (num == index)
					{
						return compoundElements[i];
					}
					num++;
				}
			}
			return null;
		}

		public HardwareJoystickMap.CompoundElement GetDPadData(int index)
		{
			if (compoundElements == null)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < compoundElements.Length; i++)
			{
				if (compoundElements[i] != null && compoundElements[i].type == CompoundControllerElementType.DPad)
				{
					if (num == index)
					{
						return compoundElements[i];
					}
					num++;
				}
			}
			return null;
		}

		public ControllerElementType GetElementType(int elementIdentifierId)
		{
			if (!elementIdentifiers.ContainsKey(elementIdentifierId))
			{
				return ControllerElementType.Button;
			}
			return elementIdentifiers[elementIdentifierId].elementType;
		}

		public bool TryGetCompoundElementMemberCombinedLocalizedName(IList<ActionElementMap> aems, out string result)
		{
			result = null;
			if (aems == null || compoundElements == null || aems.Count == 0)
			{
				return false;
			}
			int count = aems.Count;
			for (int i = 0; i < compoundElements.Length; i++)
			{
				HardwareJoystickMap.CompoundElement compoundElement = compoundElements[i];
				if (compoundElement == null)
				{
					continue;
				}
				int num = 0;
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap = aems[j];
					if (actionElementMap != null && ArrayTools.IndexOf(compoundElement.componentElementIdentifiers, actionElementMap.elementIdentifierId) >= 0)
					{
						num++;
					}
				}
				if (num != count)
				{
					continue;
				}
				ControllerElementIdentifier elementIdentifierById = GetElementIdentifierById(compoundElement.elementIdentifier);
				if (elementIdentifierById == null)
				{
					continue;
				}
				int num2;
				switch (iWSnQCGKDXIUHBXFjahUblQtmywgb(compoundElement, elementIdentifierById, aems, out num2))
				{
				case BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement:
					result = elementIdentifierById.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
					break;
				case BUaaQvOJrhYxPTwFqfcdHiCwToKf.FoundIndex:
					if (num2 >= 0)
					{
						result = elementIdentifierById.GetCompoundElementSpecialName(num2);
					}
					break;
				}
				if (!string.IsNullOrEmpty(result))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetCompoundElementMemberCombinedGlyph(IList<ActionElementMap> aems, bool getGlyph, bool getFinalKey, out object glyphResult, out string finalKey)
		{
			glyphResult = null;
			finalKey = null;
			if (aems == null || compoundElements == null || aems.Count == 0)
			{
				return false;
			}
			int count = aems.Count;
			for (int i = 0; i < compoundElements.Length; i++)
			{
				HardwareJoystickMap.CompoundElement compoundElement = compoundElements[i];
				if (compoundElement == null)
				{
					continue;
				}
				int num = 0;
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap = aems[j];
					if (actionElementMap != null && ArrayTools.IndexOf(compoundElement.componentElementIdentifiers, actionElementMap.elementIdentifierId) >= 0)
					{
						num++;
					}
				}
				if (num != count)
				{
					continue;
				}
				ControllerElementIdentifier elementIdentifierById = GetElementIdentifierById(compoundElement.elementIdentifier);
				if (elementIdentifierById == null)
				{
					continue;
				}
				int num2;
				switch (iWSnQCGKDXIUHBXFjahUblQtmywgb(compoundElement, elementIdentifierById, aems, out num2))
				{
				case BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement:
					if (getGlyph)
					{
						glyphResult = elementIdentifierById.glyph;
					}
					if (getFinalKey)
					{
						finalKey = elementIdentifierById.GetFinalGlyphKey(AxisRange.Full);
					}
					break;
				case BUaaQvOJrhYxPTwFqfcdHiCwToKf.FoundIndex:
					if (num2 >= 0)
					{
						if (getGlyph)
						{
							glyphResult = elementIdentifierById.GetCompoundElementSpecialGlyph(num2);
						}
						if (getFinalKey)
						{
							finalKey = elementIdentifierById.GetCompoundElementSpecialFinalGlyphKey(num2);
						}
					}
					break;
				}
				if (getGlyph && glyphResult != null)
				{
					return true;
				}
				if (getFinalKey && !string.IsNullOrEmpty(finalKey))
				{
					return true;
				}
			}
			return false;
		}

		private int KvcuXCEdVrpiDIUtAqOzBpgDDxuA(ControllerElementIdentifier[] P_0, int P_1)
		{
			if (P_0 == null)
			{
				return -1;
			}
			int result = -1;
			for (int i = 0; i < P_0.Length; i++)
			{
				if (P_0[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == P_1)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private static BUaaQvOJrhYxPTwFqfcdHiCwToKf iWSnQCGKDXIUHBXFjahUblQtmywgb(HardwareJoystickMap.CompoundElement P_0, ControllerElementIdentifier P_1, IList<ActionElementMap> P_2, out int P_3)
		{
			P_3 = -1;
			if (P_0 == null || P_1 == null || P_2 == null)
			{
				return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
			}
			if (P_0.componentElementIdentifiers == null)
			{
				return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
			}
			if (P_1.elementType != ControllerElementType.CompoundElement)
			{
				return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
			}
			int count = P_2.Count;
			ControllerElementIdentifier.ToElementNameLocalizerTypes(P_1.elementType, P_1.compoundElementType, out var _, out var resultCompoundElementType);
			FDNFDGKMldROgCHjPdSVTnUzAnLgb.fOURUkakBckGnlgOkmwbemAJMKXu fOURUkakBckGnlgOkmwbemAJMKXu;
			switch (P_1.compoundElementType)
			{
			case CompoundControllerElementType.Axis2D:
			{
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap = P_2[j];
					if (actionElementMap == null)
					{
						continue;
					}
					if (actionElementMap.elementType != ControllerElementType.Axis)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
					int num9 = ArrayTools.IndexOf(P_0.componentElementIdentifiers, actionElementMap.elementIdentifierId);
					if (num9 < 0)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
					if (actionElementMap.axisRange == AxisRange.Full)
					{
						switch (num9)
						{
						case 0:
							num3++;
							break;
						case 1:
							num4++;
							break;
						default:
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
						}
						continue;
					}
					switch (num9)
					{
					case 0:
						if (actionElementMap.axisRange == AxisRange.Positive)
						{
							num6++;
						}
						else if (actionElementMap.axisRange == AxisRange.Negative)
						{
							num5++;
						}
						break;
					case 1:
						if (actionElementMap.axisRange == AxisRange.Positive)
						{
							num7++;
						}
						else if (actionElementMap.axisRange == AxisRange.Negative)
						{
							num8++;
						}
						break;
					default:
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
				}
				if (num3 == 1)
				{
					if (num4 == 1)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
					}
					if (num7 == 1 && num8 == 1)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
					}
				}
				else if (num4 == 1)
				{
					if (num3 == 1)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
					}
					if (num5 == 1 && num6 == 1)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
					}
				}
				else if (num5 == 1 && num6 == 1)
				{
					if (num7 == 1 && num8 == 1)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
					}
					if (FDNFDGKMldROgCHjPdSVTnUzAnLgb.tGJPnPxMDZQkntURokRfyFQymnPw(resultCompoundElementType, out fOURUkakBckGnlgOkmwbemAJMKXu))
					{
						P_3 = fOURUkakBckGnlgOkmwbemAJMKXu.mzqArobTQIFTrvtmnTEwpJtuvJTTA(AxisDirection.Horizontal);
						if (P_3 >= 0)
						{
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.FoundIndex;
						}
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
				}
				else
				{
					if (num7 != 1 || num8 != 1)
					{
						break;
					}
					if (num5 == 1 && num6 == 1)
					{
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
					}
					if (FDNFDGKMldROgCHjPdSVTnUzAnLgb.tGJPnPxMDZQkntURokRfyFQymnPw(resultCompoundElementType, out fOURUkakBckGnlgOkmwbemAJMKXu))
					{
						P_3 = fOURUkakBckGnlgOkmwbemAJMKXu.mzqArobTQIFTrvtmnTEwpJtuvJTTA(AxisDirection.Vertical);
						if (P_3 >= 0)
						{
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.FoundIndex;
						}
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
				}
				break;
			}
			case CompoundControllerElementType.DPad:
			{
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					ActionElementMap actionElementMap = P_2[i];
					if (actionElementMap != null)
					{
						if (actionElementMap.elementType != ControllerElementType.Button)
						{
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
						}
						int num2 = ArrayTools.IndexOf(P_0.componentElementIdentifiers, actionElementMap.elementIdentifierId);
						if (num2 < 0)
						{
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
						}
						num |= 1 << num2;
					}
				}
				switch (num)
				{
				case 15:
					return BUaaQvOJrhYxPTwFqfcdHiCwToKf.IsWholeElement;
				case 5:
					if (FDNFDGKMldROgCHjPdSVTnUzAnLgb.tGJPnPxMDZQkntURokRfyFQymnPw(resultCompoundElementType, out fOURUkakBckGnlgOkmwbemAJMKXu))
					{
						P_3 = fOURUkakBckGnlgOkmwbemAJMKXu.mzqArobTQIFTrvtmnTEwpJtuvJTTA(AxisDirection.Vertical);
						if (P_3 >= 0)
						{
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.FoundIndex;
						}
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
					break;
				case 10:
					if (FDNFDGKMldROgCHjPdSVTnUzAnLgb.tGJPnPxMDZQkntURokRfyFQymnPw(resultCompoundElementType, out fOURUkakBckGnlgOkmwbemAJMKXu))
					{
						P_3 = fOURUkakBckGnlgOkmwbemAJMKXu.mzqArobTQIFTrvtmnTEwpJtuvJTTA(AxisDirection.Horizontal);
						if (P_3 >= 0)
						{
							return BUaaQvOJrhYxPTwFqfcdHiCwToKf.FoundIndex;
						}
						return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
					}
					break;
				}
				break;
			}
			}
			return BUaaQvOJrhYxPTwFqfcdHiCwToKf.Error;
		}
	}
}
