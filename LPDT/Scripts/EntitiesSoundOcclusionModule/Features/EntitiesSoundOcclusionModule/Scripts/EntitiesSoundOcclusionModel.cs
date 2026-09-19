using System;
using System.Collections.Generic;
using FMOD.Studio;
using Features.AudioServiceModule.Scripts;
using UnityEngine;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	public class EntitiesSoundOcclusionModel
	{
		private readonly Dictionary<int, float> _voiceDistanceMultiplierByPlayerId = new Dictionary<int, float>();

		internal Dictionary<EventInstance, ISoundSource> TrackedSounds { get; } = new Dictionary<EventInstance, ISoundSource>(new EventInstanceComparer());

		public event Action<Vector3, float, ISoundSource, string> OnEntitiesTriggeredBySound;

		internal void TriggerEntitiesTriggeredBySound(Vector3 soundPosition, float soundDistance, ISoundSource soundSource, string soundPath)
		{
			this.OnEntitiesTriggeredBySound?.Invoke(soundPosition, soundDistance, soundSource, soundPath);
		}

		public void SetVoiceDistanceMultiplier(int playerId, float multiplier)
		{
			if (multiplier <= 1f)
			{
				_voiceDistanceMultiplierByPlayerId.Remove(playerId);
			}
			else
			{
				_voiceDistanceMultiplierByPlayerId[playerId] = multiplier;
			}
		}

		public float GetVoiceDistanceMultiplier(int playerId)
		{
			if (!_voiceDistanceMultiplierByPlayerId.TryGetValue(playerId, out var value))
			{
				return 1f;
			}
			return value;
		}
	}
}
