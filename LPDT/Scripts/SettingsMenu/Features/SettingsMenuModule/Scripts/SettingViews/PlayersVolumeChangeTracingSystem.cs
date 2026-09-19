using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.SettingsMenuModule.Scripts.Data;
using Features.VoiceSpeakersModule.Scripts.Data;
using Photon.Voice.Unity.FMOD;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PlayersVolumeChangeTracingSystem : IInitializable, IDisposable
	{
		private const string VOICE_INTENSITY_PARAMETER = "VoiceIntensity";

		private readonly PlayersVolumeData _playersVolumeData;

		private readonly SpawnedVoiceModel _spawnedVoiceModel;

		private readonly IGameUpdater _gameUpdater;

		public PlayersVolumeChangeTracingSystem(PlayersVolumeData playersVolumeData, SpawnedVoiceModel spawnedVoiceModel, IGameUpdater gameUpdater)
		{
			_playersVolumeData = playersVolumeData;
			_spawnedVoiceModel = spawnedVoiceModel;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_playersVolumeData.OnPlayerVolumeRegistered += StartPlayerVolumeTracing;
			_playersVolumeData.BeforePlayerVolumeUnregistered += UnSubscribeUpdatePlayersVolume;
			_spawnedVoiceModel.OnSpeakerRegistered += UpdatePlayersVolume;
			_spawnedVoiceModel.OnSpeakerInstanceRegistered += SubscribeUpdatePlayersVolume;
			UpdatePlayersVolume();
			_gameUpdater.OnUpdate += UpdatePlayersVolume;
		}

		public void Dispose()
		{
			_playersVolumeData.OnPlayerVolumeRegistered -= StartPlayerVolumeTracing;
			_playersVolumeData.BeforePlayerVolumeUnregistered -= UnSubscribeUpdatePlayersVolume;
			_spawnedVoiceModel.OnSpeakerRegistered -= UpdatePlayersVolume;
			_gameUpdater.OnUpdate -= UpdatePlayersVolume;
			_spawnedVoiceModel.OnSpeakerInstanceRegistered -= SubscribeUpdatePlayersVolume;
			foreach (KeyValuePair<int, PlayerVolumeInfo> item in _playersVolumeData.PlayersVolumeInfo)
			{
				if (_spawnedVoiceModel.Speakers.TryGetValue(item.Key, out var value))
				{
					value.OnCreateAudioOut -= UpdatePlayersVolume;
				}
			}
		}

		private void StartPlayerVolumeTracing(int targetPlayer)
		{
			if (_playersVolumeData.PlayersVolumeInfo.TryGetValue(targetPlayer, out var value))
			{
				value.OnPlayerVolumeUpdate += OnPlayerVolumeUpdate;
			}
			UpdatePlayersVolume();
		}

		private void OnPlayerVolumeUpdate(float playerVolume)
		{
			UpdatePlayersVolume();
		}

		private void UpdatePlayersVolume()
		{
			foreach (KeyValuePair<int, PlayerVolumeInfo> item in _playersVolumeData.PlayersVolumeInfo)
			{
				if (_spawnedVoiceModel.Speakers.ContainsKey(item.Key))
				{
					_spawnedVoiceModel.Speakers[item.Key].EventInstance.setParameterByName("VoiceIntensity", item.Value.Volume * 100f);
				}
			}
		}

		private void SubscribeUpdatePlayersVolume(SpeakerFMOD speakerFMOD)
		{
			speakerFMOD.OnCreateAudioOut += UpdatePlayersVolume;
		}

		private void UnSubscribeUpdatePlayersVolume(int targetPlayerId)
		{
			if (_playersVolumeData.PlayersVolumeInfo.TryGetValue(targetPlayerId, out var value))
			{
				value.OnPlayerVolumeUpdate -= OnPlayerVolumeUpdate;
			}
		}
	}
}
