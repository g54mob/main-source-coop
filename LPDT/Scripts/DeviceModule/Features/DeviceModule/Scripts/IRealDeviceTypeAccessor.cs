using Features.DeviceModule.Scripts.DeviceData;

namespace Features.DeviceModule.Scripts
{
	public interface IRealDeviceTypeAccessor
	{
		DeviceType GetRealDeviceType(DeviceType defaultDeviceType);
	}
}
