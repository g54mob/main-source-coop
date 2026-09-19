using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.VoiceControlModule.Scripts;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class DrunkVoiceEffectsSystem : IInitializable, IDisposable
	{
		private readonly struct VoiceFxSnapshot
		{
			public readonly float Distortion;

			public readonly float LowPass;

			public readonly float Pitch;

			public readonly float Reverb;

			public static VoiceFxSnapshot Neutral => new VoiceFxSnapshot(0f, 1f, 0f, 0f);

			public VoiceFxSnapshot(float distortion, float lowPass, float pitch, float reverb)
			{
				Distortion = distortion;
				LowPass = lowPass;
				Pitch = pitch;
				Reverb = reverb;
			}

			public bool Approximately(VoiceFxSnapshot other)
			{
				if (Mathf.Approximately(Distortion, other.Distortion) && Mathf.Approximately(LowPass, other.LowPass) && Mathf.Approximately(Pitch, other.Pitch))
				{
					return Mathf.Approximately(Reverb, other.Reverb);
				}
				return false;
			}
		}

		private readonly IGameUpdater _gameUpdater;

		private readonly IVoiceService _voiceService;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly DrunkennessConfiguration _configuration;

		private readonly Dictionary<int, IStat> _drunkennessByPlayerId = new Dictionary<int, IStat>();

		private readonly Dictionary<int, VoiceFxSnapshot> _lastAppliedByPlayerId = new Dictionary<int, VoiceFxSnapshot>();

		private readonly Dictionary<int, float> _lastLoggedDrunkByPlayerId = new Dictionary<int, float>();

		public DrunkVoiceEffectsSystem(IGameUpdater gameUpdater, IVoiceService voiceService, SpawnedEntityStatsModel spawnedEntityStatsModel, DrunkennessConfiguration configuration)
		{
			_gameUpdater = gameUpdater;
			_voiceService = voiceService;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_configuration = configuration;
		}

		public void Initialize()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			_gameUpdater.OnUpdate += Tick;
			foreach (KeyValuePair<int, EntityStatEntityNetworkedBase> playerStat in _spawnedEntityStatsModel.PlayerStats)
			{
				OnPlayerStatRegistered(playerStat.Key);
			}
		}

		public void Dispose()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			_gameUpdater.OnUpdate -= Tick;
			foreach (int key in _drunkennessByPlayerId.Keys)
			{
				ApplyNeutral(key);
			}
			_drunkennessByPlayerId.Clear();
			_lastAppliedByPlayerId.Clear();
			_lastLoggedDrunkByPlayerId.Clear();
		}

		private void OnPlayerStatRegistered(int playerId)
		{
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				IStat stat = value.GetStat(EntityStatType.Drunkenness);
				if (stat != null)
				{
					_drunkennessByPlayerId[playerId] = stat;
				}
			}
		}

		private void Tick()
		{
			if (_configuration == null)
			{
				return;
			}
			List<int> list = null;
			foreach (KeyValuePair<int, IStat> item in _drunkennessByPlayerId)
			{
				int key = item.Key;
				if (!_spawnedEntityStatsModel.PlayerStats.ContainsKey(key))
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(key);
				}
				else
				{
					ApplyForPlayer(key, item.Value.FullValue);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					int num = list[i];
					ApplyNeutral(num);
					_drunkennessByPlayerId.Remove(num);
					_lastAppliedByPlayerId.Remove(num);
					_lastLoggedDrunkByPlayerId.Remove(num);
				}
			}
		}

		private void ApplyForPlayer(int playerId, float drunkenness)
		{
			VoiceFxSnapshot voiceFxSnapshot = new VoiceFxSnapshot(_configuration.EvaluateVoiceDistortion(drunkenness), _configuration.EvaluateVoiceLowPass(drunkenness), _configuration.EvaluateVoicePitch(drunkenness), _configuration.EvaluateVoiceReverb(drunkenness));
			if (_lastAppliedByPlayerId.TryGetValue(playerId, out var value) && value.Approximately(voiceFxSnapshot))
			{
				return;
			}
			_lastAppliedByPlayerId[playerId] = voiceFxSnapshot;
			ApplySnapshot(playerId, voiceFxSnapshot);
			if (_configuration.DebugLog)
			{
				float value2;
				float num = (_lastLoggedDrunkByPlayerId.TryGetValue(playerId, out value2) ? value2 : (-1f));
				if (!(Mathf.Abs(drunkenness - num) < 0.25f) || (drunkenness > 0f && num < 0f))
				{
					_lastLoggedDrunkByPlayerId[playerId] = drunkenness;
					Debug.Log($"[Drunk] voice player={playerId} drunk={drunkenness:0.###} " + $"dist={voiceFxSnapshot.Distortion:0.###} lp={voiceFxSnapshot.LowPass:0.###} " + $"pitch={voiceFxSnapshot.Pitch:0.###} rev={voiceFxSnapshot.Reverb:0.###}");
				}
			}
		}

		private void ApplyNeutral(int playerId)
		{
			VoiceFxSnapshot neutral = VoiceFxSnapshot.Neutral;
			_lastAppliedByPlayerId[playerId] = neutral;
			ApplySnapshot(playerId, neutral);
		}

		private void ApplySnapshot(int playerId, VoiceFxSnapshot snapshot)
		{
			string voiceDistortionParameterName = _configuration.VoiceDistortionParameterName;
			string voiceLowPassParameterName = _configuration.VoiceLowPassParameterName;
			string voicePitchParameterName = _configuration.VoicePitchParameterName;
			string voiceReverbParameterName = _configuration.VoiceReverbParameterName;
			_voiceService.ProcessEffectForSpeaker(playerId, snapshot.Distortion, voiceDistortionParameterName);
			_voiceService.ProcessEffectForSpeakerNon3d(playerId, snapshot.Distortion, voiceDistortionParameterName);
			_voiceService.ProcessEffectForSpeaker(playerId, snapshot.LowPass, voiceLowPassParameterName);
			_voiceService.ProcessEffectForSpeakerNon3d(playerId, snapshot.LowPass, voiceLowPassParameterName);
			_voiceService.ProcessEffectForSpeaker(playerId, snapshot.Pitch, voicePitchParameterName);
			_voiceService.ProcessEffectForSpeakerNon3d(playerId, snapshot.Pitch, voicePitchParameterName);
			_voiceService.ProcessEffectForSpeaker(playerId, snapshot.Reverb, voiceReverbParameterName);
			_voiceService.ProcessEffectForSpeakerNon3d(playerId, snapshot.Reverb, voiceReverbParameterName);
		}
	}
}
