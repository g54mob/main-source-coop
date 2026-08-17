using System;
using System.Collections.Generic;

namespace EvilCore.Networking
{
	public interface IVoiceDeviceController
	{
		IReadOnlyList<VoiceAudioDevice> InputDevices { get; }

		IReadOnlyList<VoiceAudioDevice> OutputDevices { get; }

		string ActiveInputDeviceId { get; }

		string ActiveOutputDeviceId { get; }

		event Action OnAudioDevicesChanged;

		void SetInputDevice(string deviceId);

		void SetOutputDevice(string deviceId);

		void RefreshAudioDevices();
	}
}
