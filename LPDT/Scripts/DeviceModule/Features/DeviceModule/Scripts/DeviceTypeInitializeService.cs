using Features.DeviceModule.Scripts.DeviceData;

namespace Features.DeviceModule.Scripts
{
	public class DeviceTypeInitializeService : IDeviceTypeInitializeService
	{
		private readonly DeviceTypeModel _deviceTypeModel;

		private readonly IRealDeviceTypeAccessor _realDeviceTypeAccessor;

		private readonly IUnityDeviceTypeAccessor _unityDeviceTypeAccessor;

		public DeviceTypeInitializeService(DeviceTypeModel deviceTypeModel, IRealDeviceTypeAccessor realDeviceTypeAccessor, IUnityDeviceTypeAccessor unityDeviceTypeAccessor)
		{
			_deviceTypeModel = deviceTypeModel;
			_realDeviceTypeAccessor = realDeviceTypeAccessor;
			_unityDeviceTypeAccessor = unityDeviceTypeAccessor;
		}

		public void Initialize()
		{
			DeviceType defaultDeviceType = _unityDeviceTypeAccessor.GetDefaultDeviceType();
			_deviceTypeModel.CurrentDevice = _realDeviceTypeAccessor.GetRealDeviceType(defaultDeviceType);
		}
	}
}
