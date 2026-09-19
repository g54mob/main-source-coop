using System.Collections.Generic;
using FMOD;

namespace Photon.Voice.FMOD
{
	public class AudioInEnumerator : DeviceEnumeratorBase
	{
		private const int NAME_MAX_LENGTH = 1000;

		private const string LOG_PREFIX = "[PV] [FMOD] AudioInEnumerator: ";

		private global::FMOD.System coreSystem;

		public AudioInEnumerator(global::FMOD.System coreSystem, ILogger logger)
			: base(logger)
		{
			this.coreSystem = coreSystem;
			Refresh();
		}

		public override void Refresh()
		{
			RESULT recordNumDrivers = coreSystem.getRecordNumDrivers(out var numdrivers, out var _);
			if (recordNumDrivers != RESULT.OK)
			{
				Error = "failed to getRecordNumDrivers: " + recordNumDrivers;
				logger.Log(LogLevel.Error, "[PV] [FMOD] AudioInEnumerator: " + Error);
				return;
			}
			devices = new List<DeviceInfo>();
			for (int i = 0; i < numdrivers; i++)
			{
				recordNumDrivers = coreSystem.getRecordDriverInfo(i, out var name, 1000, out var _, out var _, out var _, out var _, out var _);
				if (recordNumDrivers != RESULT.OK)
				{
					Error = "failed to getRecordDriverInfo: " + recordNumDrivers;
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioInEnumerator: " + Error);
					return;
				}
				devices.Add(new DeviceInfo(i, name));
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
