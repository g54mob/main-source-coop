using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class PlayersAudioPresenter : PresenterBehaviour<PlayersAudioViewBase>
	{
		private readonly Dictionary<int, PlayersSoundSettingsItemPresenter> _playersSoundSettings = new Dictionary<int, PlayersSoundSettingsItemPresenter>();

		private readonly IPlayerAudioViewFactory _playerAudioViewFactory;

		private readonly PlayersVolumeData _playersVolumeData;

		private readonly SettingsWindow _settingsWindow;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayersAudioPresenter(IPlayerAudioViewFactory playerAudioViewFactory, PlayersVolumeData playersVolumeData, SettingsWindow settingsWindow, MultiplayerModel multiplayerModel)
		{
			_playerAudioViewFactory = playerAudioViewFactory;
			_playersVolumeData = playersVolumeData;
			_settingsWindow = settingsWindow;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			_playersVolumeData.OnPlayerVolumeRegistered += AddedPlayerAudioSettings;
			_playersVolumeData.OnPlayerVolumeUnregistered += RemovePlayerAudioSettings;
			_playersVolumeData.OnPlayerVolumeClear += ClearPlayersVolume;
			AddExistingPlayerVolume();
		}

		protected override void OnDisposed()
		{
			_playersVolumeData.OnPlayerVolumeRegistered -= AddedPlayerAudioSettings;
			_playersVolumeData.OnPlayerVolumeUnregistered -= RemovePlayerAudioSettings;
			_playersVolumeData.OnPlayerVolumeClear -= ClearPlayersVolume;
		}

		private void ClearPlayersVolume()
		{
			foreach (KeyValuePair<int, PlayersSoundSettingsItemPresenter> playersSoundSetting in _playersSoundSettings)
			{
				playersSoundSetting.Value.DestroyView();
			}
			_playersSoundSettings.Clear();
			UpdatePrioritySelectable();
		}

		private void AddedPlayerAudioSettings(int playerID)
		{
			if (playerID != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				PlayersSoundSettingsItemPresenter playersSoundSettingsItemPresenter = _playerAudioViewFactory.CreatePlayerAudioButtonView(base.View.SettingsItemContainer, _settingsWindow);
				playersSoundSettingsItemPresenter.SetTargetPlayer(playerID);
				if (!_playersSoundSettings.TryAdd(playerID, playersSoundSettingsItemPresenter))
				{
					_playersSoundSettings[playerID] = playersSoundSettingsItemPresenter;
				}
				UpdatePrioritySelectable();
			}
		}

		private void RemovePlayerAudioSettings(int playerID)
		{
			_playersSoundSettings[playerID].DestroyView();
			_playersSoundSettings.Remove(playerID);
			UpdatePrioritySelectable();
		}

		private void UpdatePrioritySelectable()
		{
			if (_playersSoundSettings.Count > 0)
			{
				int num = int.MaxValue;
				foreach (int key in _playersSoundSettings.Keys)
				{
					if (key < num)
					{
						num = key;
					}
				}
				base.View.SelectableSettingsTab.UpdatePrioritySelectable(_playersSoundSettings[num].GetPrioritySelectable());
			}
			else
			{
				base.View.SelectableSettingsTab.UpdatePrioritySelectable(null);
			}
		}

		private void AddExistingPlayerVolume()
		{
			foreach (KeyValuePair<int, PlayerVolumeInfo> item in _playersVolumeData.PlayersVolumeInfo)
			{
				AddedPlayerAudioSettings(item.Key);
			}
		}
	}
}
