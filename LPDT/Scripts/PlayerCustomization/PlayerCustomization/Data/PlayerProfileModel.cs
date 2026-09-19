using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;
using UnityEngine;

namespace PlayerCustomization.Data
{
	public class PlayerProfileModel : ISavable
	{
		private const string SAVE_ID = "PlayerProfile";

		private readonly ISavingManager _savingManager;

		private PlayerProfileDataHolder _dataHolder;

		public SavingGroup SavingGroup => SavingGroup.PlayerProfile;

		public string PlayerName => _dataHolder.PlayerName;

		public Color PlayerColor => _dataHolder.PlayerColor;

		public event Action<string> OnPlayerNameChanged;

		public event Action<Color> OnPlayerColorChanged;

		public PlayerProfileModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public void SetPlayerName(string playerName)
		{
			_dataHolder.PlayerName = playerName;
			this.OnPlayerNameChanged?.Invoke(playerName);
		}

		public void SetPlayerColor(Color color)
		{
			_dataHolder.PlayerColor = color;
			this.OnPlayerColorChanged?.Invoke(color);
		}

		public void LoadData()
		{
			PlayerProfileDataHolder playerProfileDataHolder = _savingManager.LoadDataForID<PlayerProfileDataHolder>("PlayerProfile");
			if (playerProfileDataHolder != null)
			{
				_dataHolder = playerProfileDataHolder;
				return;
			}
			_dataHolder = new PlayerProfileDataHolder
			{
				PlayerColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f))
			};
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("PlayerProfile", _dataHolder, SavingGroup.ToString());
		}
	}
}
