using Features.MultiplayerSessionServices.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using UnityEngine;

namespace Features.AudioDevicesModule.Scripts
{
	public class MicrophoneService : IMicrophoneService
	{
		private const string OUTPUT_DEVICE_SUFFICS = "loopback";

		private readonly MicrophoneConfiguration _microphoneConfiguration;

		private readonly SpawnedVoiceModel _spawnedVoiceModel;

		private readonly MultiplayerModel _multiplayerModel;

		private int _currentMicroId = -1;

		private readonly MicrophoneModel _microphoneModel;

		public MicrophoneService(SpawnedVoiceModel spawnedVoiceModel, MicrophoneConfiguration microphoneConfiguration, MultiplayerModel multiplayerModel, MicrophoneModel microphoneModel)
		{
			_spawnedVoiceModel = spawnedVoiceModel;
			_microphoneConfiguration = microphoneConfiguration;
			_multiplayerModel = multiplayerModel;
			_microphoneModel = microphoneModel;
		}

		public void ChangeMicrophoneSensitivity(float value)
		{
			if (!(_microphoneModel.PlayerRecorder == null))
			{
				float t = value / 100f;
				float voiceDetectionThreshold = Mathf.Lerp(_microphoneConfiguration.MaxSensitivity, _microphoneConfiguration.MinSensitivity, t);
				_microphoneModel.PlayerRecorder.VoiceDetectionThreshold = voiceDetectionThreshold;
			}
		}
	}
}
