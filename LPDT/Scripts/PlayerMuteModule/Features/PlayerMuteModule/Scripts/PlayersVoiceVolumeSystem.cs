using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Features.VoiceControlModule.Scripts;
using Zenject;

namespace Features.PlayerMuteModule.Scripts
{
	public class PlayersVoiceVolumeSystem : IInitializable, IDisposable
	{
		private readonly PlayersVolumeData _playersVolumeData;

		private readonly IGameUpdater _gameUpdater;

		private readonly IVoiceService _voiceService;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayersVoiceVolumeSystem(PlayersVolumeData playersVolumeData, IGameUpdater gameUpdater, IVoiceService voiceService, MultiplayerModel multiplayerModel)
		{
			_playersVolumeData = playersVolumeData;
			_gameUpdater = gameUpdater;
			_voiceService = voiceService;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += ProcessVolumes;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= ProcessVolumes;
		}

		private void ProcessVolumes()
		{
			foreach (KeyValuePair<int, PlayerVolumeInfo> item in _playersVolumeData.PlayersVolumeInfo)
			{
				if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId != item.Key)
				{
					_voiceService.ProcessEffectForSpeaker(item.Key, item.Value.Volume, "VoiceIntensity");
					_voiceService.ProcessEffectForSpeakerNon3d(item.Key, item.Value.Volume, "VoiceIntensity");
				}
			}
		}
	}
}
