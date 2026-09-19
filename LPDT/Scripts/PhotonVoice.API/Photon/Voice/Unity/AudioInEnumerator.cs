using System.Collections.Generic;

namespace Photon.Voice.Unity
{
	public class AudioInEnumerator : DeviceEnumeratorBase
	{
		public override string Error => null;

		public AudioInEnumerator(ILogger logger)
			: base(logger)
		{
			Refresh();
		}

		public override void Refresh()
		{
			string[] array = UnityMicrophone.devices;
			devices = new List<DeviceInfo>();
			foreach (string name in array)
			{
				devices.Add(new DeviceInfo(name));
			}
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
