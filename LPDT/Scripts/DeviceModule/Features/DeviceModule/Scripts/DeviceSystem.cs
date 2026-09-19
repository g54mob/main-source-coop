using UnityEngine;
using Zenject;

namespace Features.DeviceModule.Scripts
{
	public class DeviceSystem : IInitializable
	{
		private readonly IDeviceTypeInitializeService _deviceTypeInitializeService;

		private readonly DeviceTypeModel _deviceTypeModel;

		private readonly DeviceEditorSwitcherConfiguration _deviceEditorSwitcherConfiguration;

		private readonly IDeviceService _deviceService;

		public DeviceSystem(DeviceTypeModel deviceTypeModel, DeviceEditorSwitcherConfiguration deviceEditorSwitcherConfiguration, IDeviceService deviceService, IDeviceTypeInitializeService deviceTypeInitializeService)
		{
			_deviceTypeModel = deviceTypeModel;
			_deviceEditorSwitcherConfiguration = deviceEditorSwitcherConfiguration;
			_deviceService = deviceService;
			_deviceTypeInitializeService = deviceTypeInitializeService;
		}

		public void Initialize()
		{
			if (Application.isEditor)
			{
				_deviceService.SetCurrentDevice(_deviceEditorSwitcherConfiguration.EditorDeviceType);
				_deviceTypeModel.IsEditor = true;
			}
			else
			{
				_deviceTypeInitializeService.Initialize();
			}
		}
	}
}
