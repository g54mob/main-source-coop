using System;

namespace Epic.OnlineServices.Lobby
{
	public struct LocalRTCOptions
	{
		public uint Flags { get; }

		public bool UseManualAudioInput { get; }

		public bool UseManualAudioOutput { get; }

		public bool LocalAudioDeviceInputStartsMuted { get; }

		public IntPtr Reserved { get; }
	}
}
