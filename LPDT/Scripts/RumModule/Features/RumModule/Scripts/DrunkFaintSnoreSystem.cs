using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GameUpdaterModule;
using Features.RagdollModule.Scripts;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class DrunkFaintSnoreSystem : IInitializable, IDisposable
	{
		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly DrunkennessConfiguration _configuration;

		private readonly IAudioService _audioService;

		private readonly IGameUpdater _gameUpdater;

		private readonly Dictionary<int, EventInstance> _snoreByPlayerId = new Dictionary<int, EventInstance>();

		private readonly List<int> _scratchIds = new List<int>(8);

		public DrunkFaintSnoreSystem(PlayersRagdollModel playersRagdollModel, DrunkennessConfiguration configuration, IAudioService audioService, IGameUpdater gameUpdater)
		{
			_playersRagdollModel = playersRagdollModel;
			_configuration = configuration;
			_audioService = audioService;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += Tick;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= Tick;
			StopAll();
		}

		private void Tick()
		{
			if (_configuration == null || _configuration.FaintSnoreLoopEvent.IsNull || _audioService == null)
			{
				return;
			}
			foreach (KeyValuePair<int, PlayerRagdollEntity> item in _playersRagdollModel.PlayersRagdoll)
			{
				PlayerRagdollEntity value = item.Value;
				if (value != null && value.HasSimulationReason(RagdollSimulationReasonEnum.DrunkFaint))
				{
					EnsureSnore(item.Key, value);
				}
				else
				{
					StopSnore(item.Key);
				}
			}
			_scratchIds.Clear();
			foreach (int key in _snoreByPlayerId.Keys)
			{
				if (!_playersRagdollModel.PlayersRagdoll.ContainsKey(key))
				{
					_scratchIds.Add(key);
				}
			}
			for (int i = 0; i < _scratchIds.Count; i++)
			{
				StopSnore(_scratchIds[i]);
			}
		}

		private void EnsureSnore(int playerId, PlayerRagdollEntity ragdoll)
		{
			if (_snoreByPlayerId.TryGetValue(playerId, out var value))
			{
				value.set3DAttributes(ragdoll.transform.position.To3DAttributes());
				return;
			}
			EventInstance value2 = _audioService.CreateInstance(_configuration.FaintSnoreLoopEvent);
			value2.set3DAttributes(ragdoll.transform.position.To3DAttributes());
			value2.start();
			_snoreByPlayerId[playerId] = value2;
		}

		private void StopSnore(int playerId)
		{
			if (_snoreByPlayerId.TryGetValue(playerId, out var value))
			{
				_snoreByPlayerId.Remove(playerId);
				_audioService.StopInstance(value, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_audioService.ReleaseInstance(value);
			}
		}

		private void StopAll()
		{
			if (_snoreByPlayerId.Count != 0)
			{
				_scratchIds.Clear();
				_scratchIds.AddRange(_snoreByPlayerId.Keys);
				for (int i = 0; i < _scratchIds.Count; i++)
				{
					StopSnore(_scratchIds[i]);
				}
			}
		}
	}
}
