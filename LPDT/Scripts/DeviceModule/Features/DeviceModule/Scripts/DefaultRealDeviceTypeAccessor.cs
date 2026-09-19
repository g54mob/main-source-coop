using Features.DeviceModule.Scripts.DeviceData;
using JetBrains.Annotations;
using RSG.Muffin.MockSubmodule.MockModule;

namespace Features.DeviceModule.Scripts
{
	[PublicAPI]
	[MockRealization]
	public class DefaultRealDeviceTypeAccessor : IRealDeviceTypeAccessor
	{
		public DeviceType GetRealDeviceType(DeviceType defaultDeviceType)
		{
			return defaultDeviceType;
		}
	}
}
