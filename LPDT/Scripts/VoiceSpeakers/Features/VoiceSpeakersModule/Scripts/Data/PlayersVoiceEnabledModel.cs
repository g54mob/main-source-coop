using System;
using System.Collections.Generic;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.VoiceSpeakersModule.Scripts.Data
{
	[Serializable]
	public class PlayersVoiceEnabledModel : DataStreamSynchronizableBase<PlayersVoiceEnabledModel>
	{
		[field: SerializeField]
		public Global.SerializableDictionary.SerializableDictionary<int, bool> PlayersVoiceEnabledStatus { get; private set; } = new Global.SerializableDictionary.SerializableDictionary<int, bool>();

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<int, bool> OnPlayerVoiceEnabledChanged;

		public bool IsVoiceEnabled(int playerId)
		{
			bool value;
			return !PlayersVoiceEnabledStatus.TryGetValue(playerId, out value) || value;
		}

		public void SwitchVoiceEnabled(PlayerRef player, bool enabled)
		{
			SetVoiceEnabled(player.PlayerId, enabled);
			Synchronize();
		}

		protected override void SetNewValues(PlayersVoiceEnabledModel synchronizable)
		{
			foreach (KeyValuePair<int, bool> item in synchronizable.PlayersVoiceEnabledStatus)
			{
				SetVoiceEnabled(item.Key, item.Value);
			}
		}

		private void SetVoiceEnabled(int playerId, bool enabled)
		{
			bool value;
			bool num = PlayersVoiceEnabledStatus.TryGetValue(playerId, out value);
			PlayersVoiceEnabledStatus[playerId] = enabled;
			if (!num || value != enabled)
			{
				this.OnPlayerVoiceEnabledChanged?.Invoke(playerId, enabled);
			}
		}
	}
}
