using Features.DeviceModule.Scripts.DeviceData;

namespace Features.DeviceModule.Scripts
{
	public interface IDeviceService
	{
		DeviceType GetCurrentDevice();

		void SetCurrentDevice(DeviceType deviceType);

		bool IsEditor();

		bool IsMobile();

		bool IsConsole();

		bool IsUWPDevice();
	}
}
