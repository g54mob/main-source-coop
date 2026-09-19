using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.VoiceSpeakersModule.Scripts.Data
{
	[Serializable]
	public class PlayerVoiceActivityModel : JsonSynchronizableBaseWithCustomData<PlayerVoiceActivityModel, PlayerVoiceActivitySyncData>, ISessionCleanup
	{
		[field: SerializeField]
		public SerializableDictionary<int, bool> SpeakingStates { get; private set; } = new SerializableDictionary<int, bool>();

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<int, bool> OnPlayerSpeakingChanged;

		public bool IsPlayerSpeaking(int playerId)
		{
			bool value;
			return SpeakingStates.TryGetValue(playerId, out value) && value;
		}

		public void SetSpeaking(int playerId, bool isSpeaking)
		{
			if (!SpeakingStates.TryGetValue(playerId, out var value) || value != isSpeaking)
			{
				SpeakingStates[playerId] = isSpeaking;
				this.OnPlayerSpeakingChanged?.Invoke(playerId, isSpeaking);
				base.Data1 = new PlayerVoiceActivitySyncData
				{
					PlayerId = playerId,
					IsSpeaking = isSpeaking
				};
				CustomSynchronize();
			}
		}

		protected override void SetNewValues(PlayerVoiceActivityModel model, bool isSynchronizedOnStart)
		{
			foreach (KeyValuePair<int, bool> speakingState in model.SpeakingStates)
			{
				SpeakingStates[speakingState.Key] = speakingState.Value;
				this.OnPlayerSpeakingChanged?.Invoke(speakingState.Key, speakingState.Value);
			}
		}

		protected override void SetNewCustomValues(PlayerVoiceActivitySyncData data)
		{
			SpeakingStates[data.PlayerId] = data.IsSpeaking;
			this.OnPlayerSpeakingChanged?.Invoke(data.PlayerId, data.IsSpeaking);
		}

		public void Cleanup()
		{
			SpeakingStates.Clear();
		}
	}
}
