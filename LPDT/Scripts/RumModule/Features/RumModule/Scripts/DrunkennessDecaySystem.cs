using System;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class DrunkennessDecaySystem : IInitializable, IDisposable
	{
		private const float SyncIntervalSeconds = 0.25f;

		private const float DebugLogIntervalSeconds = 1f;

		private readonly IGameUpdater _gameUpdater;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly DrunkennessConfiguration _configuration;

		private EntityStatEntityNetworkedBase _statsEntity;

		private IStat _drunkennessStat;

		private float _syncTimer;

		private float _debugLogTimer;

		public DrunkennessDecaySystem(IGameUpdater gameUpdater, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, DrunkennessConfiguration configuration)
		{
			_gameUpdater = gameUpdater;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_configuration = configuration;
		}

		public void Initialize()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			_gameUpdater.OnUpdate += Tick;
			if (_configuration != null && _configuration.DebugLog)
			{
				Debug.Log($"[Drunk] DecaySystem init decay/s={_configuration.DecayPerSecond} " + $"max={_configuration.MaxDrunkenness}");
			}
			if (_multiplayerModel.NetworkRunner != null && _spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				OnPlayerStatRegistered(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
		}

		public void Dispose()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			_gameUpdater.OnUpdate -= Tick;
			_statsEntity = null;
			_drunkennessStat = null;
		}

		private void OnPlayerStatRegistered(int playerId)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && _spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				_statsEntity = value;
				_drunkennessStat = value.GetStat(EntityStatType.Drunkenness);
				EnsureStatBounds(sync: true);
				if (_configuration != null && _configuration.DebugLog)
				{
					Debug.Log($"[Drunk] DecaySystem bound playerId={playerId} " + $"statNull={_drunkennessStat == null} " + $"full={_drunkennessStat?.FullValue:0.###} " + $"value={_drunkennessStat?.Value:0.###} " + $"max={_drunkennessStat?.MaxValue:0.###}");
				}
			}
		}

		private void EnsureStatBounds(bool sync = false)
		{
			if (_drunkennessStat != null && !(_configuration == null))
			{
				bool flag = false;
				if (!Mathf.Approximately(_drunkennessStat.MinValue, 0f))
				{
					_drunkennessStat.MinValue = 0f;
					flag = true;
				}
				if (_drunkennessStat.MaxValue < _configuration.MaxDrunkenness)
				{
					_drunkennessStat.MaxValue = _configuration.MaxDrunkenness;
					flag = true;
				}
				if (sync && flag)
				{
					_statsEntity.SynchronizeStatValues(EntityStatType.Drunkenness, RPCType.InAllWays);
				}
			}
		}

		private void Tick()
		{
			if (_statsEntity == null || _drunkennessStat == null || _configuration == null || _drunkennessStat.FullValue <= 0f)
			{
				return;
			}
			EnsureStatBounds();
			float decayPerSecond = _configuration.DecayPerSecond;
			float num = decayPerSecond * Time.deltaTime;
			if (num <= 0f)
			{
				return;
			}
			float fullValue = _drunkennessStat.FullValue;
			float num2 = Mathf.Max(0f, fullValue - num);
			float num3 = num2 - fullValue;
			if (Mathf.Approximately(num3, 0f))
			{
				return;
			}
			_drunkennessStat.AddValue(num3);
			_syncTimer += Time.deltaTime;
			if (_syncTimer >= 0.25f || num2 <= 0f)
			{
				_syncTimer = 0f;
				_statsEntity.SynchronizeStatValues(EntityStatType.Drunkenness, RPCType.InAllWays);
			}
			if (_configuration.DebugLog)
			{
				_debugLogTimer += Time.deltaTime;
				if (!(_debugLogTimer < 1f))
				{
					_debugLogTimer = 0f;
					float num4 = ((decayPerSecond > 0f) ? (_drunkennessStat.FullValue / decayPerSecond) : float.PositiveInfinity);
					Debug.Log($"[Drunk] sober tick full={_drunkennessStat.FullValue:0.###} " + $"value={_drunkennessStat.Value:0.###} max={_drunkennessStat.MaxValue:0.###} " + $"decay/s={decayPerSecond:0.###} (~{num4:0.0}s to 0) " + $"dtDecay={num:0.####}");
				}
			}
		}
	}
}
