using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Networked
{
	public class EnemySpawnCountAnalyticsModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly EnemySpawnCountAnalyticsModel _enemySpawnCountAnalyticsModel;

		private EnemySpawnCountAnalyticsNetworkObject _enemySpawnCountAnalyticsNetworkObject;

		private bool _hasPendingCounts;

		private Dictionary<EnemyType, int> _pendingCounts;

		private bool _hasPendingKillCounts;

		private Dictionary<EnemyType, int> _pendingKillCounts;

		public EnemySpawnCountAnalyticsModelBridge(EnemySpawnCountAnalyticsModel enemySpawnCountAnalyticsModel)
		{
			_enemySpawnCountAnalyticsModel = enemySpawnCountAnalyticsModel;
		}

		public void Bind(EnemySpawnCountAnalyticsNetworkObject enemySpawnCountAnalyticsNetworkObject)
		{
			Unbind();
			_enemySpawnCountAnalyticsNetworkObject = enemySpawnCountAnalyticsNetworkObject;
			_enemySpawnCountAnalyticsNetworkObject.OnNetworkedCountsChanged += HandleNetworkedCountsChanged;
			_enemySpawnCountAnalyticsNetworkObject.OnNetworkedKillCountsChanged += HandleNetworkedKillCountsChanged;
			_enemySpawnCountAnalyticsNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_enemySpawnCountAnalyticsNetworkObject.OnDespawned += HandleDespawned;
			_enemySpawnCountAnalyticsModel.Counts.BindWriter(WriteCounts);
			_enemySpawnCountAnalyticsModel.KillCounts.BindWriter(WriteKillCounts);
			ApplyCountsFromNetwork(_enemySpawnCountAnalyticsNetworkObject.ReadCounts());
			ApplyKillCountsFromNetwork(_enemySpawnCountAnalyticsNetworkObject.ReadKillCounts());
			_enemySpawnCountAnalyticsModel.SetAuthorityProvider(() => _enemySpawnCountAnalyticsNetworkObject.HasStateAuthority);
			_enemySpawnCountAnalyticsModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_enemySpawnCountAnalyticsNetworkObject == null))
			{
				_enemySpawnCountAnalyticsNetworkObject.OnNetworkedCountsChanged -= HandleNetworkedCountsChanged;
				_enemySpawnCountAnalyticsNetworkObject.OnNetworkedKillCountsChanged -= HandleNetworkedKillCountsChanged;
				_enemySpawnCountAnalyticsNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_enemySpawnCountAnalyticsNetworkObject.OnDespawned -= HandleDespawned;
				_enemySpawnCountAnalyticsModel.Counts.BindWriter(null);
				_enemySpawnCountAnalyticsModel.KillCounts.BindWriter(null);
				_enemySpawnCountAnalyticsNetworkObject = null;
				_hasPendingCounts = false;
				_hasPendingKillCounts = false;
				_enemySpawnCountAnalyticsModel.SetAuthorityProvider(null);
				_enemySpawnCountAnalyticsModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<EnemySpawnCountAnalyticsNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedCountsChanged(IReadOnlyDictionary<EnemyType, int> counts)
		{
			ApplyCountsFromNetwork(counts);
		}

		private void HandleNetworkedKillCountsChanged(IReadOnlyDictionary<EnemyType, int> killCounts)
		{
			ApplyKillCountsFromNetwork(killCounts);
		}

		private void ApplyCountsFromNetwork(IReadOnlyDictionary<EnemyType, int> counts)
		{
			_enemySpawnCountAnalyticsModel.Counts.ApplyFromNetwork(counts);
		}

		private void ApplyKillCountsFromNetwork(IReadOnlyDictionary<EnemyType, int> killCounts)
		{
			_enemySpawnCountAnalyticsModel.KillCounts.ApplyFromNetwork(killCounts);
		}

		private bool WriteCounts(IReadOnlyDictionary<EnemyType, int> value)
		{
			if (!_enemySpawnCountAnalyticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] EnemySpawnCountAnalyticsModel.Counts was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCounts = new Dictionary<EnemyType, int>();
			foreach (KeyValuePair<EnemyType, int> item in value)
			{
				_pendingCounts[item.Key] = item.Value;
			}
			_hasPendingCounts = true;
			return true;
		}

		private bool WriteKillCounts(IReadOnlyDictionary<EnemyType, int> value)
		{
			if (!_enemySpawnCountAnalyticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] EnemySpawnCountAnalyticsModel.KillCounts was written without state authority; the write was ignored.");
				return false;
			}
			_pendingKillCounts = new Dictionary<EnemyType, int>();
			foreach (KeyValuePair<EnemyType, int> item in value)
			{
				_pendingKillCounts[item.Key] = item.Value;
			}
			_hasPendingKillCounts = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_enemySpawnCountAnalyticsNetworkObject == null || _enemySpawnCountAnalyticsNetworkObject.Object == null || _enemySpawnCountAnalyticsNetworkObject.Runner == null)
			{
				return false;
			}
			if (_enemySpawnCountAnalyticsNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_enemySpawnCountAnalyticsNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingCounts)
			{
				_hasPendingCounts = false;
				_enemySpawnCountAnalyticsNetworkObject.TryWriteCounts(_pendingCounts);
			}
			if (_hasPendingKillCounts)
			{
				_hasPendingKillCounts = false;
				_enemySpawnCountAnalyticsNetworkObject.TryWriteKillCounts(_pendingKillCounts);
			}
		}
	}
}
