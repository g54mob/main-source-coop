using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine;

namespace Features.DeviceModule.Scripts
{
	public class DeviceService : IDeviceService
	{
		private readonly DeviceTypeModel _deviceTypeModel;

		public DeviceService(DeviceTypeModel deviceTypeModel)
		{
			_deviceTypeModel = deviceTypeModel;
		}

		public Features.DeviceModule.Scripts.DeviceData.DeviceType GetCurrentDevice()
		{
			return _deviceTypeModel.CurrentDevice;
		}

		public bool IsEditor()
		{
			return _deviceTypeModel.IsEditor;
		}

		public bool IsMobile()
		{
			Features.DeviceModule.Scripts.DeviceData.DeviceType currentDevice = _deviceTypeModel.CurrentDevice;
			return currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.Android || currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.IOS;
		}

		public bool IsConsole()
		{
			Features.DeviceModule.Scripts.DeviceData.DeviceType currentDevice = _deviceTypeModel.CurrentDevice;
			return currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.PlayStation5 || currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.PlayStation4 || currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.Nintendo || currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox || currentDevice == Features.DeviceModule.Scripts.DeviceData.DeviceType.SteamDeck;
		}

		public bool IsUWPDevice()
		{
			if (Application.platform != RuntimePlatform.MetroPlayerX86 && Application.platform != RuntimePlatform.MetroPlayerX64)
			{
				return Application.platform == RuntimePlatform.MetroPlayerARM;
			}
			return true;
		}

		public void SetCurrentDevice(Features.DeviceModule.Scripts.DeviceData.DeviceType deviceType)
		{
			_deviceTypeModel.CurrentDevice = deviceType;
			_deviceTypeModel.OnDeviceChanged?.Invoke();
		}
	}
}
