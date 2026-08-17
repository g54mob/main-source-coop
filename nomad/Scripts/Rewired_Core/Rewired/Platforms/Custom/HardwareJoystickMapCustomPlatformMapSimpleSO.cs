using System;
using Rewired.Data.Mapping;

namespace Rewired.Platforms.Custom
{
	[Serializable]
	public class HardwareJoystickMapCustomPlatformMapSimpleSO : HardwareJoystickMapCustomPlatformMapSO
	{
		public HardwareJoystickMapCustomPlatformMapSimple platformMap;

		public override HardwareJoystickMap.Platform GetPlatformMap()
		{
			return platformMap;
		}
	}
}
