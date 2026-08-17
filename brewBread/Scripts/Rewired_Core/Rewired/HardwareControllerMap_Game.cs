using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Data.Mapping;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	[CustomObfuscation(rename = false)]
	internal class HardwareControllerMap_Game
	{
		public readonly string controllerName;

		public readonly HardwareControllerMapIdentifier hardwareMapIdentifier;

		public readonly int customControllerSourceId;

		public readonly ADictionary<int, ControllerElementIdentifier> elementIdentifiers;

		public readonly ControllerElementIdentifier[] elementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] buttonElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] axisElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] axis2DElementIdentifiers_cache;

		public readonly ControllerElementIdentifier[] hatElementIdentifiers_cache;

		public readonly IList<ControllerElementIdentifier> elementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> buttonElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> axisElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> axis2DElementIdentifiers_readOnly;

		public readonly IList<ControllerElementIdentifier> hatElementIdentifiers_readOnly;

		public readonly int[] buttonElementIdentifierIds;

		public readonly int[] axisElementIdentifierIds;

		public readonly int[] axis2DElementIdentifierIds;

		public readonly int[] hatElementIdentifierIds;

		public readonly int elementIdentifierCount;

		public readonly int axisCount;

		public readonly int buttonCount;

		public readonly int compoundElementCount;

		public readonly int axis2DCount;

		public readonly int hatCount;

		public readonly JoystickType[] joystickTypes;

		public readonly AxisCalibrationData[] hwAxisCalibrationData;

		public readonly AxisRange[] hwAxisRanges;

		public readonly HardwareAxisInfo[] hwAxisInfo;

		public readonly HardwareButtonInfo[] hwButtonInfo;

		public readonly HardwareJoystickMap.CompoundElement[] compoundElements;

		private HardwareControllerMap_Game(string P_0)
		{
			controllerName = P_0;
		}

		public HardwareControllerMap_Game(string P_0, int P_1, ControllerElementIdentifier[] P_2, int[] P_3, int[] P_4, AxisCalibrationData[] P_5, AxisRange[] P_6, HardwareAxisInfo[] P_7, HardwareButtonInfo[] P_8, HardwareJoystickMap.CompoundElement[] P_9)
			: this(P_0, P_2, P_3, P_4, P_5, P_6, P_7, P_8, P_9)
		{
			customControllerSourceId = P_1;
		}

		public HardwareControllerMap_Game(string P_0, HardwareControllerMapIdentifier P_1, JoystickType[] P_2, ControllerElementIdentifier[] P_3, int[] P_4, int[] P_5, AxisCalibrationData[] P_6, AxisRange[] P_7, HardwareAxisInfo[] P_8, HardwareButtonInfo[] P_9, HardwareJoystickMap.CompoundElement[] P_10)
			: this(P_0, P_3, P_4, P_5, P_6, P_7, P_8, P_9, P_10)
		{
			hardwareMapIdentifier = P_1;
			if (P_2 == null)
			{
				joystickTypes = new JoystickType[1];
			}
			else
			{
				joystickTypes = ArrayTools.ShallowCopy(P_2);
			}
		}

		public HardwareControllerMap_Game(string P_0, HardwareControllerMapIdentifier P_1, ControllerElementIdentifier[] P_2, int[] P_3, int[] P_4, AxisCalibrationData[] P_5, AxisRange[] P_6, HardwareAxisInfo[] P_7, HardwareButtonInfo[] P_8, HardwareJoystickMap.CompoundElement[] P_9)
			: this(P_0, P_1, null, P_2, P_3, P_4, P_5, P_6, P_7, P_8, P_9)
		{
		}

		private HardwareControllerMap_Game(string P_0, ControllerElementIdentifier[] P_1, int[] P_2, int[] P_3, AxisCalibrationData[] P_4, AxisRange[] P_5, HardwareAxisInfo[] P_6, HardwareButtonInfo[] P_7, HardwareJoystickMap.CompoundElement[] P_8)
			: this(P_0)
		{
			elementIdentifierCount = ((P_1 != null) ? P_1.Length : 0);
			int num = ((P_2 != null) ? P_2.Length : 0);
			int num2 = ((P_3 != null) ? P_3.Length : 0);
			P_8 = ArrayTools.DeepClone(P_8);
			compoundElements = P_8;
			compoundElementCount = ((P_8 != null) ? P_8.Length : 0);
			int num3 = 0;
			int num4 = 0;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < compoundElementCount; i++)
			{
				if (P_8[i] != null)
				{
					if (P_8[i].type == CompoundControllerElementType.Axis2D)
					{
						num3++;
						list.Add(P_8[i].elementIdentifier);
					}
					else if (P_8[i].type == CompoundControllerElementType.Hat)
					{
						num4++;
						list2.Add(P_8[i].elementIdentifier);
						HardwareJoystickMap.CompoundElement.SortHatElementsClockwise(P_8[i]);
					}
				}
			}
			int[] array = list.ToArray();
			int[] array2 = list2.ToArray();
			elementIdentifiers = new ADictionary<int, ControllerElementIdentifier>(elementIdentifierCount);
			elementIdentifiers_cache = new ControllerElementIdentifier[elementIdentifierCount];
			buttonElementIdentifiers_cache = new ControllerElementIdentifier[num];
			axisElementIdentifiers_cache = new ControllerElementIdentifier[num2];
			axis2DElementIdentifiers_cache = new ControllerElementIdentifier[num3];
			hatElementIdentifiers_cache = new ControllerElementIdentifier[num4];
			elementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(elementIdentifiers_cache);
			buttonElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(buttonElementIdentifiers_cache);
			axisElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(axisElementIdentifiers_cache);
			axis2DElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(axis2DElementIdentifiers_cache);
			hatElementIdentifiers_readOnly = new ReadOnlyCollection<ControllerElementIdentifier>(hatElementIdentifiers_cache);
			for (int j = 0; j < elementIdentifierCount; j++)
			{
				elementIdentifiers_cache[j] = P_1[j];
				elementIdentifiers.Add(P_1[j].id, P_1[j]);
			}
			for (int k = 0; k < num; k++)
			{
				int num5 = mTATJwIHXuihPPTCNDzzzVMJiCrt(P_1, P_2[k]);
				if (num5 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					buttonElementIdentifiers_cache[k] = P_1[num5];
				}
			}
			for (int l = 0; l < num2; l++)
			{
				int num6 = mTATJwIHXuihPPTCNDzzzVMJiCrt(P_1, P_3[l]);
				if (num6 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					axisElementIdentifiers_cache[l] = P_1[num6];
				}
			}
			for (int m = 0; m < num3; m++)
			{
				int num7 = mTATJwIHXuihPPTCNDzzzVMJiCrt(P_1, array[m]);
				if (num7 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					axis2DElementIdentifiers_cache[m] = P_1[num7];
				}
			}
			for (int n = 0; n < num4; n++)
			{
				int num8 = mTATJwIHXuihPPTCNDzzzVMJiCrt(P_1, array2[n]);
				if (num8 < 0)
				{
					Logger.LogError("Invalid hardware element identifier id!");
				}
				else
				{
					hatElementIdentifiers_cache[n] = P_1[num8];
				}
			}
			buttonElementIdentifierIds = P_2;
			axisElementIdentifierIds = P_3;
			axis2DElementIdentifierIds = array;
			hatElementIdentifierIds = array2;
			elementIdentifierCount = ((elementIdentifiers != null) ? elementIdentifiers.Count : 0);
			buttonCount = ((P_2 != null) ? P_2.Length : 0);
			axisCount = ((P_3 != null) ? P_3.Length : 0);
			axis2DCount = num3;
			hatCount = num4;
			hwAxisCalibrationData = P_4;
			hwAxisRanges = P_5;
			hwAxisInfo = P_6;
			hwButtonInfo = P_7;
		}

		public string GetElementIdentifierName(int elementIdentifierId)
		{
			if (!elementIdentifiers.ContainsKey(elementIdentifierId))
			{
				return string.Empty;
			}
			return elementIdentifiers[elementIdentifierId].name;
		}

		public string GetElementIdentifierPositiveName(int elementIdentifierId)
		{
			if (!elementIdentifiers.ContainsKey(elementIdentifierId))
			{
				return string.Empty;
			}
			return elementIdentifiers[elementIdentifierId].positiveName;
		}

		public string GetElementIdentifierNegativeName(int elementIdentifierId)
		{
			if (!elementIdentifiers.ContainsKey(elementIdentifierId))
			{
				return string.Empty;
			}
			return elementIdentifiers[elementIdentifierId].negativeName;
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
				if (elementIdentifiers_cache[i].name.Equals(elementIdentifierName, StringComparison.OrdinalIgnoreCase))
				{
					return GetAxisIndex(elementIdentifiers_cache[i].id);
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
				if (elementIdentifiers_cache[i].name.Equals(elementIdentifierName, StringComparison.OrdinalIgnoreCase))
				{
					return GetButtonIndex(elementIdentifiers_cache[i].id);
				}
			}
			return -1;
		}

		public ControllerElementIdentifier GetElementIdentifierById(int id)
		{
			int count = elementIdentifiers.Count;
			for (int i = 0; i < count; i++)
			{
				if (elementIdentifiers_cache[i].id == id)
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

		public ControllerElementType GetElementType(int elementIdentifierId)
		{
			if (!elementIdentifiers.ContainsKey(elementIdentifierId))
			{
				return ControllerElementType.Button;
			}
			return elementIdentifiers[elementIdentifierId].elementType;
		}

		private int mTATJwIHXuihPPTCNDzzzVMJiCrt(ControllerElementIdentifier[] P_0, int P_1)
		{
			if (P_0 == null)
			{
				return -1;
			}
			int result = -1;
			for (int i = 0; i < P_0.Length; i++)
			{
				if (P_0[i].id == P_1)
				{
					result = i;
					break;
				}
			}
			return result;
		}
	}
}
