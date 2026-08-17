using System;
using System.Collections.Generic;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public sealed class CustomControllerElementSelector
	{
		[CustomObfuscation(rename = false)]
		public enum ElementType
		{
			Axis = 0,
			Button = 1
		}

		[CustomObfuscation(rename = false)]
		public enum SelectorType
		{
			Name = 0,
			Index = 1,
			Id = 2
		}

		[Tooltip("The target Custom Controller element type.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ElementType _elementType;

		[Tooltip("The method to use to look up the target Custom Controller element.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private SelectorType _selectorType = SelectorType.Id;

		[Tooltip("The target Custom Controller element name.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _elementName;

		[Tooltip("The target Custom Controller element index.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(-1, 2147483647)]
		private int _elementIndex;

		[Tooltip("The target Custom Controller element id.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(-1, 2147483647)]
		private int _elementId = -1;

		[HideInInspector]
		private int yMuTIRbkUwbWCLxYxUNhpitKrtqF = -1;

		[HideInInspector]
		private int MXOaIGouyZgNeJvbzPTPSLEClreBA = -1;

		public ElementType elementType
		{
			get
			{
				return _elementType;
			}
			set
			{
				if (_elementType != value)
				{
					_elementType = value;
					ClearCache();
				}
			}
		}

		public SelectorType selectorType
		{
			get
			{
				return _selectorType;
			}
			set
			{
				if (_selectorType != value)
				{
					_selectorType = value;
					ClearCache();
				}
			}
		}

		public string elementName
		{
			get
			{
				return _elementName;
			}
			set
			{
				if (!(_elementName == value))
				{
					_elementName = value;
					ClearCache();
				}
			}
		}

		public int elementIndex
		{
			get
			{
				return _elementIndex;
			}
			set
			{
				if (_elementIndex != value)
				{
					_elementIndex = value;
					ClearCache();
				}
			}
		}

		public int elementId
		{
			get
			{
				return _elementId;
			}
			set
			{
				if (_elementId != value)
				{
					_elementId = value;
					ClearCache();
				}
			}
		}

		public bool isAssigned => selectorType switch
		{
			SelectorType.Id => _elementId >= 0, 
			SelectorType.Index => _elementIndex >= 0, 
			SelectorType.Name => !string.IsNullOrEmpty(_elementName), 
			_ => throw new NotImplementedException(), 
		};

		public int GetElementIndex(Rewired.CustomController customController)
		{
			if (customController == null)
			{
				return -1;
			}
			if (yMuTIRbkUwbWCLxYxUNhpitKrtqF >= 0 && yMuTIRbkUwbWCLxYxUNhpitKrtqF != customController.id)
			{
				ClearCache();
			}
			if (MXOaIGouyZgNeJvbzPTPSLEClreBA >= 0)
			{
				return MXOaIGouyZgNeJvbzPTPSLEClreBA;
			}
			yMuTIRbkUwbWCLxYxUNhpitKrtqF = customController.id;
			switch (_selectorType)
			{
			case SelectorType.Id:
			{
				IList<ControllerElementIdentifier> list = ETiwJmJEjvSasVnssoYlizoZTjKu(customController, _elementType);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == _elementId)
					{
						MXOaIGouyZgNeJvbzPTPSLEClreBA = j;
						break;
					}
				}
				break;
			}
			case SelectorType.Index:
			{
				if (_elementIndex < 0)
				{
					return -1;
				}
				IList<ControllerElementIdentifier> list = ETiwJmJEjvSasVnssoYlizoZTjKu(customController, _elementType);
				if (_elementIndex >= list.Count)
				{
					return -1;
				}
				MXOaIGouyZgNeJvbzPTPSLEClreBA = _elementIndex;
				break;
			}
			case SelectorType.Name:
			{
				if (_elementName == null)
				{
					return -1;
				}
				IList<ControllerElementIdentifier> list = ETiwJmJEjvSasVnssoYlizoZTjKu(customController, _elementType);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename.Equals(_elementName))
					{
						MXOaIGouyZgNeJvbzPTPSLEClreBA = i;
						break;
					}
				}
				break;
			}
			default:
				throw new NotImplementedException();
			}
			return MXOaIGouyZgNeJvbzPTPSLEClreBA;
		}

		public string GetSelectorFormattedString()
		{
			return selectorType switch
			{
				SelectorType.Id => "Id: " + _elementId, 
				SelectorType.Index => "Index: " + _elementIndex, 
				SelectorType.Name => "Name: " + _elementName, 
				_ => throw new NotImplementedException(), 
			};
		}

		private IList<ControllerElementIdentifier> ETiwJmJEjvSasVnssoYlizoZTjKu(Rewired.CustomController P_0, ElementType P_1)
		{
			return P_1 switch
			{
				ElementType.Axis => P_0.AxisElementIdentifiers, 
				ElementType.Button => P_0.ButtonElementIdentifiers, 
				_ => throw new NotImplementedException(), 
			};
		}

		public void ClearCache()
		{
			yMuTIRbkUwbWCLxYxUNhpitKrtqF = -1;
			MXOaIGouyZgNeJvbzPTPSLEClreBA = -1;
		}
	}
}
