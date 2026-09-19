using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FMODUnity;
using Features.MultiplayerSessionServices.Scripts;
using NetworkServices.NetworkEvents;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.Fusion;
using Photon.Voice.Unity;
using WebSocketSharp;
using Zenject;

namespace Features.AudioDevicesModule.Scripts
{
	public class MicrophoneSystem : IInitializable, IDisposable
	{
		private const string OUTPUT_DEVICE_SUFFICS = "loopback";

		private readonly MicrophoneModel _microphoneModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly FmodSharedMicCapture _fmodSharedMicCapture;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		public MicrophoneSystem(MicrophoneModel microphoneModel, MultiplayerModel multiplayerModel, FmodSharedMicCapture fmodSharedMicCapture, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_microphoneModel = microphoneModel;
			_multiplayerModel = multiplayerModel;
			_fmodSharedMicCapture = fmodSharedMicCapture;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		public void Initialize()
		{
			_microphoneModel.OnMicrophoneEnabledChanged += ProcessEnabledMicrophone;
			_microphoneModel.OnMicrophoneNameEnabledChanged += SetMicrophone;
			_microphoneModel.OnRecorderRegistered += ApplySelectedMicrophoneToRecorder;
			_microphoneModel.OnCaptureDeviceListRefreshRequested += RefreshMicrophoneDeviceList;
			_networkRunnerEventBus.Subscribe<OnPlayerJoinedEvent>(ReAnnounceVoiceOnPlayerJoined);
			InitializeMicrophones();
			if (_multiplayerModel.NetworkRunner == null)
			{
				_multiplayerModel.OnNetworkRunnerChanged += SubscribeOnClientStateChanged;
			}
			else
			{
				_multiplayerModel.NetworkRunner.GetComponent<FusionVoiceClient>().Client.StateChanged += HandleClientStateChanged;
			}
		}

		public void Dispose()
		{
			_microphoneModel.OnMicrophoneEnabledChanged -= ProcessEnabledMicrophone;
			_microphoneModel.OnMicrophoneNameEnabledChanged -= SetMicrophone;
			_microphoneModel.OnRecorderRegistered -= ApplySelectedMicrophoneToRecorder;
			_microphoneModel.OnCaptureDeviceListRefreshRequested -= RefreshMicrophoneDeviceList;
			_networkRunnerEventBus.Unsubscribe<OnPlayerJoinedEvent>(ReAnnounceVoiceOnPlayerJoined);
			_multiplayerModel.OnNetworkRunnerChanged -= SubscribeOnClientStateChanged;
			if (_multiplayerModel.NetworkRunner != null && _multiplayerModel.NetworkRunner.TryGetComponent<FusionVoiceClient>(out var component) && component != null)
			{
				component.Client.StateChanged -= HandleClientStateChanged;
			}
		}

		private void ReAnnounceVoiceOnPlayerJoined(OnPlayerJoinedEvent playerJoinedEvent)
		{
			if (!(playerJoinedEvent.Player == playerJoinedEvent.Runner.LocalPlayer))
			{
				Recorder playerRecorder = _microphoneModel.PlayerRecorder;
				if (!(playerRecorder == null) && playerRecorder.RecordingEnabled)
				{
					playerRecorder.RecordingEnabled = false;
					playerRecorder.RecordingEnabled = true;
				}
			}
		}

		private void SubscribeOnClientStateChanged()
		{
			_multiplayerModel.NetworkRunner.GetComponent<FusionVoiceClient>().Client.StateChanged += HandleClientStateChanged;
		}

		private void HandleClientStateChanged(ClientState previousState, ClientState newState)
		{
			if (newState == ClientState.Joined)
			{
				ProcessEnabledMicrophone();
			}
		}

		private void RefreshMicrophoneDeviceList()
		{
			InitializeMicrophones();
		}

		private Dictionary<string, int> InitializeMicrophones()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			if (RuntimeManager.CoreSystem.getRecordNumDrivers(out var numdrivers, out var numconnected) != RESULT.OK)
			{
				_microphoneModel.MicrophonesDriverID = dictionary;
				return dictionary;
			}
			for (int i = 0; i < numdrivers; i++)
			{
				if (RuntimeManager.CoreSystem.getRecordDriverInfo(i, out var name, 256, out var _, out numconnected, out var _, out var _, out var state) == RESULT.OK && (state & DRIVER_STATE.CONNECTED) != 0 && !name.ToLower().Contains("loopback"))
				{
					dictionary.TryAdd(name, i);
				}
			}
			_microphoneModel.MicrophonesDriverID = dictionary;
			return dictionary;
		}

		private void SetFirstMicrophone()
		{
			Dictionary<string, int> source = InitializeMicrophones();
			if (source.Any())
			{
				SetMicrophone(source.First().Key);
			}
		}

		private void SetMicrophone(string micName)
		{
			if (!micName.IsNullOrEmpty() && !(_microphoneModel.PlayerRecorder == null))
			{
				Dictionary<string, int> dictionary = InitializeMicrophones();
				if (dictionary.Any() && dictionary.TryGetValue(micName, out var value) && _microphoneModel.CurrentMicrophoneId != value)
				{
					ApplyMicrophoneToRecorder(value);
				}
			}
		}

		private void ApplySelectedMicrophoneToRecorder()
		{
			if (_microphoneModel.PlayerRecorder == null)
			{
				return;
			}
			Dictionary<string, int> dictionary = InitializeMicrophones();
			if (dictionary.Any())
			{
				if (_microphoneModel.CurrentMicrophoneName.IsNullOrEmpty() || !dictionary.TryGetValue(_microphoneModel.CurrentMicrophoneName, out var value))
				{
					value = dictionary.First().Value;
				}
				ApplyMicrophoneToRecorder(value);
			}
		}

		private void ApplyMicrophoneToRecorder(int microphoneId)
		{
			bool recordingEnabled = _microphoneModel.PlayerRecorder.RecordingEnabled;
			_microphoneModel.CurrentMicrophoneId = microphoneId;
			_microphoneModel.PlayerRecorder.RecordingEnabled = false;
			_microphoneModel.PlayerRecorder.SourceType = Recorder.InputSourceType.Factory;
			_microphoneModel.PlayerRecorder.InputFactory = CreateAudioInput;
			_microphoneModel.PlayerRecorder.RecordingEnabled = true;
			_microphoneModel.PlayerRecorder.RecordingEnabled = recordingEnabled;
		}

		private IAudioDesc CreateAudioInput()
		{
			return _fmodSharedMicCapture.AcquireReader(_microphoneModel.CurrentMicrophoneId, (int)_microphoneModel.PlayerRecorder.SamplingRate, _microphoneModel.PlayerRecorder.Logger);
		}

		private void ProcessEnabledMicrophone()
		{
			if (!(_microphoneModel.PlayerRecorder == null))
			{
				_microphoneModel.PlayerRecorder.RecordingEnabled = _microphoneModel.IsMicrophoneEnabled;
			}
		}
	}
}
