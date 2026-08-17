using System;
using Rewired.Data.Mapping;

namespace Rewired.Platforms.Custom
{
	public interface IHardwareJoystickMapCustomPlatformMapProvider
	{
		HardwareJoystickMap.Platform GetPlatformMap(int customPlatformId, Guid hardwareTypeGuid);
	}
}
