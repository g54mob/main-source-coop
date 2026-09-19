using System;
using Features.DeviceModule.Scripts.DeviceData;

namespace Features.DeviceModule.Scripts
{
	public class DeviceTypeModel
	{
		public Action OnDeviceChanged;

		public DeviceType CurrentDevice { get; internal set; }

		public bool IsEditor { get; set; }
	}
}
