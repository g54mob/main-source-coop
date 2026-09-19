using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine;

namespace Features.DeviceModule.Scripts
{
	public class UnityDeviceTypeAccessor : IUnityDeviceTypeAccessor
	{
		private readonly DeviceTypeModel _deviceTypeModel;

		public UnityDeviceTypeAccessor(DeviceTypeModel deviceTypeModel)
		{
			_deviceTypeModel = deviceTypeModel;
		}

		public Features.DeviceModule.Scripts.DeviceData.DeviceType GetDefaultDeviceType()
		{
			return Application.platform switch
			{
				RuntimePlatform.Android => Features.DeviceModule.Scripts.DeviceData.DeviceType.Android, 
				RuntimePlatform.IPhonePlayer => Features.DeviceModule.Scripts.DeviceData.DeviceType.IOS, 
				RuntimePlatform.WindowsPlayer => Features.DeviceModule.Scripts.DeviceData.DeviceType.PersonalComputer, 
				RuntimePlatform.LinuxPlayer => Features.DeviceModule.Scripts.DeviceData.DeviceType.PersonalComputer, 
				RuntimePlatform.PS4 => Features.DeviceModule.Scripts.DeviceData.DeviceType.PlayStation4, 
				RuntimePlatform.PS5 => Features.DeviceModule.Scripts.DeviceData.DeviceType.PlayStation5, 
				RuntimePlatform.XboxOne => Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox, 
				RuntimePlatform.GameCoreXboxSeries => Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox, 
				RuntimePlatform.GameCoreXboxOne => Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox, 
				RuntimePlatform.MetroPlayerX86 => Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox, 
				RuntimePlatform.MetroPlayerX64 => Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox, 
				RuntimePlatform.MetroPlayerARM => Features.DeviceModule.Scripts.DeviceData.DeviceType.XBox, 
				RuntimePlatform.Switch => Features.DeviceModule.Scripts.DeviceData.DeviceType.Nintendo, 
				_ => _deviceTypeModel.CurrentDevice, 
			};
		}
	}
}
