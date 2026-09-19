using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using Zenject;

namespace Features.VoiceControlModule.Scripts.VoiceChangeOnStates
{
	public class VoiceStatesSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly IVoiceService _voiceService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly SpawnedVoiceModel _spawnedVoiceModel;

		private readonly Dictionary<int, bool> _deadVoiceStates = new Dictionary<int, bool>();

		private readonly Dictionary<int, bool> _pendingMutes = new Dictionary<int, bool>();

		private readonly IGameUpdater _gameUpdater;

		public VoiceStatesSystem(PlayersStatesSynchronizer playersStatesSynchronizer, IVoiceService voiceService, MultiplayerModel multiplayerModel, SpawnedVoiceModel spawnedVoiceModel, IGameUpdater gameUpdater)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_voiceService = voiceService;
			_multiplayerModel = multiplayerModel;
			_spawnedVoiceModel = spawnedVoiceModel;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerPlayerStateChanged;
			_spawnedVoiceModel.OnSpeakerNon3dRegistered += UpdateVolume;
			_gameUpdater.OnUpdate += UpdateVolume;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerPlayerStateChanged;
			_spawnedVoiceModel.OnSpeakerNon3dRegistered -= UpdateVolume;
			_gameUpdater.OnUpdate -= UpdateVolume;
		}

		private void UpdateVolume()
		{
			foreach (KeyValuePair<int, bool> pendingMute in _pendingMutes)
			{
				ApplyVolumeToSpeaker(pendingMute.Key, pendingMute.Value);
			}
		}

		private void OnSomePlayerPlayerStateChanged(PlayerStateData data)
		{
			int playerId = data.PlayerId;
			_deadVoiceStates.TryGetValue(playerId, out var value);
			bool flag = data.PlayerState == PlayerState.Dead;
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId && !value.Equals(flag))
			{
				ProcessLocalVoice(playerId, flag);
			}
			_deadVoiceStates[playerId] = flag;
			UpdateVoiceAudibility();
		}

		private void ProcessLocalVoice(int playerId, bool isDead)
		{
			if (isDead)
			{
				_voiceService.DespawnVoiceSpeaker(playerId);
				_voiceService.SpawnNon3dVoiceSpeaker(playerId);
			}
			else
			{
				_voiceService.DespawnNon3dVoiceSpeaker(playerId);
				_voiceService.SpawnVoiceSpeaker(playerId);
			}
		}

		private void UpdateVoiceAudibility()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			bool value;
			bool flag = _deadVoiceStates.TryGetValue(playerId, out value) && value;
			foreach (KeyValuePair<int, bool> deadVoiceState in _deadVoiceStates)
			{
				int key = deadVoiceState.Key;
				bool value2 = deadVoiceState.Value;
				if (key != playerId)
				{
					bool value3 = !flag && value2;
					_pendingMutes[key] = value3;
				}
			}
		}

		private void ApplyVolumeToSpeaker(int playerId, bool mute)
		{
			bool flag = _spawnedVoiceModel.Speakers.ContainsKey(playerId);
			bool flag2 = _spawnedVoiceModel.SpeakersNon3d.ContainsKey(playerId);
			if (flag || flag2)
			{
				float targetValue = ((!mute) ? 1 : 0);
				if (flag && mute)
				{
					_voiceService.SetVolumeForSpeaker(playerId, 0f);
				}
				if (flag2)
				{
					_voiceService.SetVolumeForSpeakerNon3d(playerId, targetValue);
				}
			}
		}
	}
}
