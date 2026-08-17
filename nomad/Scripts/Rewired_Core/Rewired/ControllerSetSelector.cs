using System;
using System.Text;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	[Preserve]
	public sealed class ControllerSetSelector : ISerializationCallbackReceiver, IDeepCloneable
	{
		public enum Type
		{
			All = 0,
			ControllerType = 1,
			HardwareType = 2,
			ControllerTemplateType = 3,
			PersistentControllerInstance = 4,
			SessionControllerInstance = 5
		}

		[SerializeField]
		[Serialize(Name = "type")]
		private Type _type;

		[SerializeField]
		[Serialize(Name = "controllerType")]
		private ControllerType _controllerType;

		[SerializeField]
		[Serialize(Name = "guid")]
		private string _guid;

		[SerializeField]
		[Serialize(Name = "hardwareIdentifier")]
		private string _hardwareIdentifier;

		[SerializeField]
		[Serialize(Name = "controllerId")]
		private int _controllerId;

		[NonSerialized]
		private Guid tXgRtFWpurtpVCEZajHYMyoISntS;

		internal bool QVWXuasRsUAvognxOkisyYrwXInSA => _type != Type.All;

		public Type type
		{
			get
			{
				return _type;
			}
			set
			{
				if (value != _type)
				{
					AICxEzUesJecDKUKWoIegJRGPZBmb();
				}
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
				if (_type != Type.HardwareType)
				{
					return Guid.Empty;
				}
				return tXgRtFWpurtpVCEZajHYMyoISntS;
			}
			set
			{
				if (_type != Type.ControllerTemplateType)
				{
					Logger.LogWarning("hardwareTypeGuid can only be set when type is " + Type.HardwareType.ToString() + ".", requiredThreadSafety: true);
					return;
				}
				tXgRtFWpurtpVCEZajHYMyoISntS = value;
				_guid = value.ToString();
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
				if (_type != Type.ControllerTemplateType)
				{
					return Guid.Empty;
				}
				return tXgRtFWpurtpVCEZajHYMyoISntS;
			}
			set
			{
				if (_type != Type.ControllerTemplateType)
				{
					Logger.LogWarning("controllerTemplateTypeGuid can only be set when type is " + Type.ControllerTemplateType.ToString() + ".", requiredThreadSafety: true);
					return;
				}
				tXgRtFWpurtpVCEZajHYMyoISntS = value;
				_guid = value.ToString();
			}
		}

		public Guid deviceInstanceGuid
		{
			get
			{
				if (_type != Type.PersistentControllerInstance)
				{
					return Guid.Empty;
				}
				return tXgRtFWpurtpVCEZajHYMyoISntS;
			}
			set
			{
				if (_type != Type.PersistentControllerInstance)
				{
					Logger.LogWarning("deviceInstanceGuid can only be set when type is " + Type.PersistentControllerInstance.ToString() + ".", requiredThreadSafety: true);
					return;
				}
				tXgRtFWpurtpVCEZajHYMyoISntS = value;
				_guid = value.ToString();
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

		internal ControllerSetSelector(Type P_0)
			: this()
		{
			_type = P_0;
		}

		public ControllerSetSelector()
		{
			_controllerId = -1;
		}

		public ControllerSetSelector(ControllerSetSelector P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			_type = P_0._type;
			_controllerType = P_0._controllerType;
			_guid = P_0._guid;
			_hardwareIdentifier = P_0._hardwareIdentifier;
			_controllerId = P_0._controllerId;
			tXgRtFWpurtpVCEZajHYMyoISntS = P_0.tXgRtFWpurtpVCEZajHYMyoISntS;
		}

		internal ControllerSetSelector(Type P_0, ControllerType P_1, string P_2, string P_3, int P_4)
		{
			_type = P_0;
			_controllerType = P_1;
			_guid = P_2;
			tXgRtFWpurtpVCEZajHYMyoISntS = StringTools.ToGuid(P_2);
			_hardwareIdentifier = P_3;
			_controllerId = P_4;
		}

		public bool Matches(Controller controller)
		{
			if (controller == null)
			{
				return false;
			}
			if (_type != Type.All && _controllerType != controller.type)
			{
				return false;
			}
			switch (_type)
			{
			case Type.All:
			case Type.ControllerType:
				return true;
			case Type.HardwareType:
				if (tXgRtFWpurtpVCEZajHYMyoISntS != Guid.Empty)
				{
					return tXgRtFWpurtpVCEZajHYMyoISntS == controller.hardwareTypeGuid;
				}
				if (string.IsNullOrEmpty(_hardwareIdentifier))
				{
					return true;
				}
				return string.Equals(_hardwareIdentifier, controller.hardwareIdentifier, StringComparison.Ordinal);
			case Type.ControllerTemplateType:
				return controller.ImplementsTemplate(tXgRtFWpurtpVCEZajHYMyoISntS);
			case Type.PersistentControllerInstance:
				return controller.deviceInstanceGuid == tXgRtFWpurtpVCEZajHYMyoISntS;
			case Type.SessionControllerInstance:
				return controller.id == _controllerId;
			default:
				throw new NotImplementedException();
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringTools.WriteVar(stringBuilder, "Type", _type.ToString());
			StringTools.WriteVar(stringBuilder, "Controller Type", _controllerType.ToString());
			StringTools.WriteVar(stringBuilder, "Guid", _guid.ToString());
			StringTools.WriteVar(stringBuilder, "Hardware Identifier", _hardwareIdentifier.ToString());
			StringTools.WriteVar(stringBuilder, "Controller Id", _controllerId.ToString());
			return stringBuilder.ToString();
		}

		private void AICxEzUesJecDKUKWoIegJRGPZBmb()
		{
			_guid = string.Empty;
			tXgRtFWpurtpVCEZajHYMyoISntS = Guid.Empty;
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			tXgRtFWpurtpVCEZajHYMyoISntS = StringTools.ToGuid(_guid);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		object IDeepCloneable.DeepClone()
		{
			return new ControllerSetSelector(this);
		}

		public static ControllerSetSelector SelectAll()
		{
			return new ControllerSetSelector(Type.All);
		}

		public static ControllerSetSelector SelectControllerType(ControllerType controllerType)
		{
			return new ControllerSetSelector(Type.ControllerType)
			{
				_controllerType = controllerType
			};
		}

		public static ControllerSetSelector SelectHardwareType(ControllerType controllerType, Guid hardwareTypeGuid, string hardwareIdentifier)
		{
			return new ControllerSetSelector(Type.HardwareType)
			{
				_controllerType = controllerType,
				hardwareTypeGuid = hardwareTypeGuid,
				_hardwareIdentifier = hardwareIdentifier
			};
		}

		public static ControllerSetSelector SelectHardwareType(Controller controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			return SelectHardwareType(controller.type, controller.hardwareTypeGuid, controller.hardwareIdentifier);
		}

		public static ControllerSetSelector SelectControllerTemplateType(ControllerType controllerType, Guid controllerTemplateTypeGuid)
		{
			return new ControllerSetSelector(Type.ControllerTemplateType)
			{
				_controllerType = controllerType,
				controllerTemplateTypeGuid = controllerTemplateTypeGuid
			};
		}

		public static ControllerSetSelector SelectControllerTemplateType(IControllerTemplate controllerTemplate)
		{
			if (controllerTemplate == null)
			{
				throw new ArgumentNullException("controllerTemplate");
			}
			return SelectControllerTemplateType(controllerTemplate.controller.type, controllerTemplate.typeGuid);
		}

		public static ControllerSetSelector SelectPersistentControllerInstance(ControllerType controllerType, Guid deviceInstanceGuid)
		{
			return new ControllerSetSelector(Type.PersistentControllerInstance)
			{
				_controllerType = controllerType,
				deviceInstanceGuid = deviceInstanceGuid
			};
		}

		public static ControllerSetSelector SelectPersistentControllerInstance(Controller controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			return SelectPersistentControllerInstance(controller.type, controller.deviceInstanceGuid);
		}

		public static ControllerSetSelector SelectSessionControllerInstance(ControllerType controllerType, int controllerId)
		{
			return new ControllerSetSelector(Type.SessionControllerInstance)
			{
				_controllerType = controllerType,
				_controllerId = controllerId
			};
		}

		public static ControllerSetSelector SelectSessionControllerInstance(Controller controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			return SelectSessionControllerInstance(controller.type, controller.id);
		}
	}
}
