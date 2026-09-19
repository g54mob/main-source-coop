using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.AudioDevicesModule.Scripts
{
	public class PlayersVoiceEnabledBroadcastSystem : IInitializable, IDisposable
	{
		private readonly MicrophoneModel _microphoneModel;

		private readonly PlayersVoiceEnabledModel _playersVoiceEnabledModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _eventBus;

		private bool _isLocalPlayerJoined;

		public PlayersVoiceEnabledBroadcastSystem(MicrophoneModel microphoneModel, PlayersVoiceEnabledModel playersVoiceEnabledModel, MultiplayerModel multiplayerModel, NetworkRunnerEventBus eventBus)
		{
			_microphoneModel = microphoneModel;
			_playersVoiceEnabledModel = playersVoiceEnabledModel;
			_multiplayerModel = multiplayerModel;
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_microphoneModel.OnMicrophoneEnabledChanged += BroadcastLocalState;
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoinedHandler);
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdownHandler);
		}

		public void Dispose()
		{
			_microphoneModel.OnMicrophoneEnabledChanged -= BroadcastLocalState;
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoinedHandler);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdownHandler);
		}

		private void OnPlayerJoinedHandler(OnPlayerJoinedEvent eventData)
		{
			if (!(eventData.Player != eventData.Runner.LocalPlayer))
			{
				_isLocalPlayerJoined = true;
				BroadcastLocalState();
			}
		}

		private void OnShutdownHandler(OnShutdownEvent eventData)
		{
			_isLocalPlayerJoined = false;
		}

		private void BroadcastLocalState()
		{
			if (_isLocalPlayerJoined && !(_multiplayerModel.NetworkRunner == null))
			{
				_playersVoiceEnabledModel.SwitchVoiceEnabled(_multiplayerModel.NetworkRunner.LocalPlayer, _microphoneModel.IsMicrophoneEnabled);
			}
		}
	}
}
