using System;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[Preserve]
	public sealed class ControllerSetSelector_Editor : IDeepCloneable
	{
		[Serialize]
		[SerializeField]
		private ControllerSetSelector.Type _type;

		[Serialize]
		[SerializeField]
		private ControllerType _controllerType;

		[Serialize]
		[SerializeField]
		private string _hardwareTypeGuidString;

		[Serialize]
		[SerializeField]
		private string _hardwareIdentifier;

		[Serialize]
		[SerializeField]
		private string _controllerTemplateTypeGuidString;

		[Serialize]
		[SerializeField]
		private string _deviceInstanceGuidString;

		[Serialize]
		[SerializeField]
		private int _customControllerSourceId;

		[Serialize]
		[SerializeField]
		private int _controllerId;

		public ControllerSetSelector.Type type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public ControllerType controllerType
		{
			get
			{
				return _controllerType;
			}
			set
			{
				_controllerType = value;
			}
		}

		public Guid hardwareTypeGuid
		{
			get
			{
				return StringTools.ToGuid(_hardwareTypeGuidString);
			}
			set
			{
				_hardwareTypeGuidString = value.ToString();
			}
		}

		public string hardwareTypeGuidString
		{
			get
			{
				return _hardwareTypeGuidString;
			}
			set
			{
				_hardwareTypeGuidString = value;
			}
		}

		public string hardwareIdentifier
		{
			get
			{
				return _hardwareIdentifier;
			}
			set
			{
				_hardwareIdentifier = value;
			}
		}

		public Guid controllerTemplateTypeGuid
		{
			get
			{
				return StringTools.ToGuid(_controllerTemplateTypeGuidString);
			}
			set
			{
				_controllerTemplateTypeGuidString = value.ToString();
			}
		}

		public string controllerTemplateTypeGuidString
		{
			get
			{
				return _controllerTemplateTypeGuidString;
			}
			set
			{
				_controllerTemplateTypeGuidString = value;
			}
		}

		public Guid deviceInstanceGuid
		{
			get
			{
				return StringTools.ToGuid(_deviceInstanceGuidString);
			}
			set
			{
				_deviceInstanceGuidString = value.ToString();
			}
		}

		public string deviceInstanceGuidString
		{
			get
			{
				return _deviceInstanceGuidString;
			}
			set
			{
				_deviceInstanceGuidString = value;
			}
		}

		public int controllerId
		{
			get
			{
				return _controllerId;
			}
			set
			{
				_controllerId = value;
			}
		}

		public int customControllerSourceId
		{
			get
			{
				return _customControllerSourceId;
			}
			set
			{
				_customControllerSourceId = value;
			}
		}

		internal ControllerSetSelector_Editor(ControllerSetSelector.Type P_0)
			: this()
		{
			_type = P_0;
		}

		public ControllerSetSelector_Editor()
		{
			_controllerId = -1;
			_customControllerSourceId = -1;
			_hardwareTypeGuidString = Guid.Empty.ToString();
		}

		public ControllerSetSelector_Editor(ControllerSetSelector_Editor P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			_type = P_0._type;
			_controllerType = P_0._controllerType;
			_hardwareTypeGuidString = P_0._hardwareTypeGuidString;
			_controllerTemplateTypeGuidString = P_0._controllerTemplateTypeGuidString;
			_deviceInstanceGuidString = P_0._deviceInstanceGuidString;
			_hardwareIdentifier = P_0._hardwareIdentifier;
			_customControllerSourceId = P_0._customControllerSourceId;
			_controllerId = P_0._controllerId;
		}

		internal ControllerSetSelector JjjvrcNzdYqOoQcCPTEXsoBqcxlz()
		{
			string text = string.Empty;
			if (_type != ControllerSetSelector.Type.All && _controllerType == ControllerType.Custom)
			{
				CustomController_Editor customControllerById = ReInput.UserData.GetCustomControllerById(_controllerId);
				if (customControllerById != null)
				{
					text = customControllerById.typeGuidString;
				}
			}
			else
			{
				switch (_type)
				{
				case ControllerSetSelector.Type.ControllerTemplateType:
					text = _controllerTemplateTypeGuidString;
					break;
				case ControllerSetSelector.Type.HardwareType:
					text = _hardwareTypeGuidString;
					break;
				case ControllerSetSelector.Type.PersistentControllerInstance:
					text = _deviceInstanceGuidString;
					break;
				default:
					throw new NotImplementedException();
				case ControllerSetSelector.Type.All:
				case ControllerSetSelector.Type.ControllerType:
				case ControllerSetSelector.Type.SessionControllerInstance:
					break;
				}
			}
			return new ControllerSetSelector(_type, _controllerType, text, _hardwareIdentifier, _controllerId);
		}

		object IDeepCloneable.DeepClone()
		{
			return new ControllerSetSelector_Editor(this);
		}
	}
}
