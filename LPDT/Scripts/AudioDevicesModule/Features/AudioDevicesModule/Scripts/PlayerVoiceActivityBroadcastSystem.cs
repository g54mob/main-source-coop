using System;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.AudioDevicesModule.Scripts
{
	public class PlayerVoiceActivityBroadcastSystem : IInitializable, IDisposable
	{
		private const float VoiceAnimationThreshold = 0.005f;

		private readonly MicrophoneModel _microphoneModel;

		private readonly PlayerVoiceActivityModel _playerVoiceActivityModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly IGameUpdater _gameUpdater;

		private bool _isLocalPlayerJoined;

		private bool _lastSentSpeaking;

		public PlayerVoiceActivityBroadcastSystem(MicrophoneModel microphoneModel, PlayerVoiceActivityModel playerVoiceActivityModel, MultiplayerModel multiplayerModel, NetworkRunnerEventBus eventBus, IGameUpdater gameUpdater)
		{
			_microphoneModel = microphoneModel;
			_playerVoiceActivityModel = playerVoiceActivityModel;
			_multiplayerModel = multiplayerModel;
			_eventBus = eventBus;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += SampleLocalVoice;
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoinedHandler);
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdownHandler);
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= SampleLocalVoice;
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoinedHandler);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdownHandler);
		}

		private void OnPlayerJoinedHandler(OnPlayerJoinedEvent eventData)
		{
			if (!(eventData.Player != eventData.Runner.LocalPlayer))
			{
				_isLocalPlayerJoined = true;
				_lastSentSpeaking = false;
			}
		}

		private void OnShutdownHandler(OnShutdownEvent eventData)
		{
			_isLocalPlayerJoined = false;
			_lastSentSpeaking = false;
		}

		private void SampleLocalVoice()
		{
			if (!_isLocalPlayerJoined || _multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!(_microphoneModel.PlayerRecorder == null))
			{
				bool flag = false;
				if (_microphoneModel.PlayerRecorder.TransmitEnabled)
				{
					flag = _microphoneModel.PlayerRecorder.LevelMeter.CurrentAvgAmp >= 0.005f;
				}
				if (flag != _lastSentSpeaking)
				{
					_lastSentSpeaking = flag;
					_playerVoiceActivityModel.SetSpeaking(playerId, flag);
				}
			}
		}
	}
}
