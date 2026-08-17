using System;
using System.Collections.Generic;
using Rewired.Data.Mapping;

namespace Rewired.Platforms.Custom
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	public sealed class HardwareJoystickMapCustomPlatformMapSimple : HardwareJoystickMapCustomPlatformMapSimpleBase
	{
		public HardwareJoystickMapCustomPlatformMapSimpleBase[] variants;

		public override IList<HardwareJoystickMap.Platform> GetVariants()
		{
			return variants;
		}

		protected override object CreateInstance()
		{
			return new HardwareJoystickMapCustomPlatformMapSimple();
		}
	}
}
