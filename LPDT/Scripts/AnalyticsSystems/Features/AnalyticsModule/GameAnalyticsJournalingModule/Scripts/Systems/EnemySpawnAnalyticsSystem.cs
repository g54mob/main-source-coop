using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class EnemySpawnAnalyticsSystem : IEnemySpawnAnalyticsFlush, IInitializable, IDisposable
	{
		private readonly EnemyTransformsModel _enemyTransformsModel;

		private readonly EnemySpawnCountAnalyticsModel _spawnCountModel;

		private readonly EnemyDeathEvent _enemyDeathEvent;

		private readonly GameAnalyticsEventSendService _analytics;

		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private readonly List<KeyValuePair<EnemyType, int>> _flushBuffer = new List<KeyValuePair<EnemyType, int>>();

		public EnemySpawnAnalyticsSystem(EnemyTransformsModel enemyTransformsModel, EnemySpawnCountAnalyticsModel spawnCountModel, EnemyDeathEvent enemyDeathEvent, GameAnalyticsEventSendService analytics, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent)
		{
			_enemyTransformsModel = enemyTransformsModel;
			_spawnCountModel = spawnCountModel;
			_enemyDeathEvent = enemyDeathEvent;
			_analytics = analytics;
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
		}

		public void Initialize()
		{
			_enemyTransformsModel.OnEnemyRegistered += OnEnemyRegistered;
			_enemyDeathEvent.OnEnemyDeadByPlayer += OnEnemyDeadByPlayer;
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += OnBeforeLevelChange;
		}

		public void Dispose()
		{
			_enemyTransformsModel.OnEnemyRegistered -= OnEnemyRegistered;
			_enemyDeathEvent.OnEnemyDeadByPlayer -= OnEnemyDeadByPlayer;
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= OnBeforeLevelChange;
			FlushPendingSpawns();
		}

		private void OnEnemyRegistered(EnemyType enemyType)
		{
			_spawnCountModel.RegisterSpawn(enemyType);
		}

		private void OnEnemyDeadByPlayer(int _, EnemyType enemyType)
		{
			_spawnCountModel.RegisterKill(enemyType);
		}

		private void OnBeforeLevelChange(BeforeLevelChangeNetworkEvent _)
		{
			FlushPendingSpawns();
		}

		public void FlushPendingSpawns()
		{
			if (_spawnCountModel.IsAuthority && _spawnCountModel.IsAttached)
			{
				FlushSpawns();
				FlushKills();
			}
		}

		private void FlushSpawns()
		{
			if (!_spawnCountModel.HasPendingSpawns)
			{
				return;
			}
			int pendingSpawnTypeCount = _spawnCountModel.PendingSpawnTypeCount;
			if (!_analytics.CanSendBatch(AnalyticsEventQuotaGroup.General, pendingSpawnTypeCount))
			{
				_spawnCountModel.ClearSpawns();
				return;
			}
			_spawnCountModel.CopyPendingSpawnsTo(_flushBuffer);
			_spawnCountModel.ClearSpawns();
			for (int i = 0; i < _flushBuffer.Count; i++)
			{
				KeyValuePair<EnemyType, int> keyValuePair = _flushBuffer[i];
				_analytics.TrackEnemySpawned(keyValuePair.Key.ToString(), keyValuePair.Value);
			}
			_flushBuffer.Clear();
		}

		private void FlushKills()
		{
			if (!_spawnCountModel.HasPendingKills)
			{
				return;
			}
			int pendingKillTypeCount = _spawnCountModel.PendingKillTypeCount;
			if (!_analytics.CanSendBatch(AnalyticsEventQuotaGroup.General, pendingKillTypeCount))
			{
				_spawnCountModel.ClearKills();
				return;
			}
			_spawnCountModel.CopyPendingKillsTo(_flushBuffer);
			_spawnCountModel.ClearKills();
			for (int i = 0; i < _flushBuffer.Count; i++)
			{
				KeyValuePair<EnemyType, int> keyValuePair = _flushBuffer[i];
				_analytics.TrackEnemyKilled(keyValuePair.Key.ToString(), keyValuePair.Value);
			}
			_flushBuffer.Clear();
		}
	}
}
