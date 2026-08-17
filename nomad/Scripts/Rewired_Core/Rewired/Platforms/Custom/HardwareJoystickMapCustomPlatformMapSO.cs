using System;
using Rewired.Data.Mapping;
using UnityEngine;

namespace Rewired.Platforms.Custom
{
	[Serializable]
	public abstract class HardwareJoystickMapCustomPlatformMapSO : ScriptableObject
	{
		[Tooltip("The joystick to which this platform map belongs. This must be assigned a HardwareJoystickMap (controller definition).")]
		public HardwareJoystickMap hardwareJoystickMap;

		public virtual bool Matches(Guid hardwareTypeGuid)
		{
			if (hardwareJoystickMap == null)
			{
				return false;
			}
			return hardwareJoystickMap.Guid == hardwareTypeGuid;
		}

		public abstract HardwareJoystickMap.Platform GetPlatformMap();
	}
}
