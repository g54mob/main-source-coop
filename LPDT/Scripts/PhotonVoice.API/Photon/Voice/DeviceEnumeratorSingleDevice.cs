using System.Collections.Generic;

namespace Photon.Voice
{
	internal class DeviceEnumeratorSingleDevice : DeviceEnumeratorBase
	{
		public DeviceEnumeratorSingleDevice(ILogger logger, string deviceName)
			: base(logger)
		{
			devices = new List<DeviceInfo>
			{
				new DeviceInfo(deviceName)
			};
		}

		public override void Refresh()
		{
			if (base.OnReady != null)
			{
				base.OnReady();
			}
		}

		public override void Dispose()
		{
		}
	}
}
