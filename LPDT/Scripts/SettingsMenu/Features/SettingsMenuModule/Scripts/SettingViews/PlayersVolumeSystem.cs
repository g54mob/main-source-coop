using System;
using Features.SettingsMenuModule.Scripts.Data;
using NetworkServices.NetworkEvents;
using PlayerCustomization;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PlayersVolumeSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly PlayersVolumeData _playersVolumeData;

		public PlayersVolumeSystem(PlayerCustomizationModel playerCustomizationModel, PlayersVolumeData playersVolumeData, NetworkRunnerEventBus eventBus)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_playersVolumeData = playersVolumeData;
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_playerCustomizationModel.OnSlotsChanged += UpdatePlayersVolumeInfoCount;
			_playerCustomizationModel.OnClear += ClearPlayersVolumeInfoCount;
			_eventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeftHandler);
		}

		public void Dispose()
		{
			_playerCustomizationModel.OnSlotsChanged -= UpdatePlayersVolumeInfoCount;
			_playerCustomizationModel.OnClear -= ClearPlayersVolumeInfoCount;
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeftHandler);
		}

		private void OnPlayerLeftHandler(OnPlayerLeftEvent playerLeftEvent)
		{
			_playersVolumeData.UnregisterPlayerVolumeInfo(playerLeftEvent.Player.PlayerId);
		}

		private void UpdatePlayersVolumeInfoCount()
		{
			foreach (PlayerCustomizationSlotData slot in _playerCustomizationModel.Slots)
			{
				if (slot.PlayerCustomizationSlotDataStatus != PlayerCustomizationSlotDataStatus.Deleted && !_playersVolumeData.PlayersVolumeInfo.ContainsKey(slot.PlayerId))
				{
					_playersVolumeData.RegisterPlayerVolumeInfo(slot.PlayerId, 0.5f);
				}
			}
		}

		private void ClearPlayersVolumeInfoCount()
		{
			_playersVolumeData.PlayersVolumeInfo.Clear();
		}
	}
}
