using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.ProgressSavingModule.Scripts.PlayerInfo
{
	public class PlayerInfoModel : ISavable
	{
		private const string SAVE_ID = "PlayerInfo";

		private readonly ISavingManager _savingManager;

		private PlayerInfoDataHolder _dataHolder = new PlayerInfoDataHolder();

		public SavingGroup SavingGroup => SavingGroup.PlayerInfo;

		public int GameLaunchCount => _dataHolder.GameLaunchCount;

		public bool ThankPlayingPopupShown => _dataHolder.ThankPlayingPopupShown;

		public bool ReleaseAnnouncePopupShown => _dataHolder.ReleaseAnnouncePopupShown;

		public event Action<int> OnGameLaunchCountChanged;

		public PlayerInfoModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public void MarkThankPlayingPopupShown()
		{
			_dataHolder.ThankPlayingPopupShown = true;
		}

		public void MarkReleaseAnnouncePopupShown()
		{
			_dataHolder.ReleaseAnnouncePopupShown = true;
		}

		public int IncrementGameLaunchCount()
		{
			_dataHolder.GameLaunchCount++;
			this.OnGameLaunchCountChanged?.Invoke(_dataHolder.GameLaunchCount);
			return _dataHolder.GameLaunchCount;
		}

		public void LoadData()
		{
			PlayerInfoDataHolder playerInfoDataHolder = _savingManager.LoadDataForID<PlayerInfoDataHolder>("PlayerInfo");
			if (playerInfoDataHolder != null)
			{
				_dataHolder = playerInfoDataHolder;
			}
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("PlayerInfo", _dataHolder, SavingGroup.ToString());
		}
	}
}
