using System;
using System.Collections.Generic;

namespace Features.SettingsMenuModule.Scripts.Data
{
	public class PlayersVolumeData
	{
		public Dictionary<int, PlayerVolumeInfo> PlayersVolumeInfo { get; } = new Dictionary<int, PlayerVolumeInfo>();

		public event Action<int> OnPlayerVolumeRegistered;

		public event Action<int> OnPlayerVolumeUnregistered;

		public event Action<int> BeforePlayerVolumeUnregistered;

		public event Action OnPlayerVolumeClear;

		public void RegisterPlayerVolumeInfo(int playerID, float startValue)
		{
			PlayersVolumeInfo.Add(playerID, new PlayerVolumeInfo(startValue));
			this.OnPlayerVolumeRegistered?.Invoke(playerID);
		}

		public void UnregisterPlayerVolumeInfo(int playerID)
		{
			this.BeforePlayerVolumeUnregistered?.Invoke(playerID);
			PlayersVolumeInfo.Remove(playerID);
			this.OnPlayerVolumeUnregistered?.Invoke(playerID);
		}

		public void ClearPlayers()
		{
			PlayersVolumeInfo.Clear();
			this.OnPlayerVolumeClear?.Invoke();
		}
	}
}
