using System;
using System.Collections.Generic;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts.Networked
{
	public class EnemySpawnStatesModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly EnemySpawnStatesModel _enemySpawnStatesModel;

		private EnemySpawnStatesNetworkObject _enemySpawnStatesNetworkObject;

		private bool _hasPendingSessionTimePassed;

		private int _pendingSessionTimePassed;

		private bool _hasPendingStates;

		private Dictionary<EnemyType, EnemySpawnStateData> _pendingStates;

		public EnemySpawnStatesModelBridge(EnemySpawnStatesModel enemySpawnStatesModel)
		{
			_enemySpawnStatesModel = enemySpawnStatesModel;
		}

		public void Bind(EnemySpawnStatesNetworkObject enemySpawnStatesNetworkObject)
		{
			Unbind();
			_enemySpawnStatesNetworkObject = enemySpawnStatesNetworkObject;
			_enemySpawnStatesNetworkObject.OnNetworkedSessionTimePassedChanged += HandleNetworkedSessionTimePassedChanged;
			_enemySpawnStatesNetworkObject.OnNetworkedStatesChanged += HandleNetworkedStatesChanged;
			_enemySpawnStatesNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_enemySpawnStatesNetworkObject.OnDespawned += HandleDespawned;
			_enemySpawnStatesModel.SessionTimePassed.BindWriter(WriteSessionTimePassed);
			_enemySpawnStatesModel.States.BindWriter(WriteStates);
			ApplySessionTimePassedFromNetwork(_enemySpawnStatesNetworkObject.SessionTimePassed);
			ApplyStatesFromNetwork(_enemySpawnStatesNetworkObject.ReadStates());
			_enemySpawnStatesModel.SetAuthorityProvider(() => _enemySpawnStatesNetworkObject.HasStateAuthority);
			_enemySpawnStatesModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_enemySpawnStatesNetworkObject == null))
			{
				_enemySpawnStatesNetworkObject.OnNetworkedSessionTimePassedChanged -= HandleNetworkedSessionTimePassedChanged;
				_enemySpawnStatesNetworkObject.OnNetworkedStatesChanged -= HandleNetworkedStatesChanged;
				_enemySpawnStatesNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_enemySpawnStatesNetworkObject.OnDespawned -= HandleDespawned;
				_enemySpawnStatesModel.SessionTimePassed.BindWriter(null);
				_enemySpawnStatesModel.States.BindWriter(null);
				_enemySpawnStatesNetworkObject = null;
				_hasPendingSessionTimePassed = false;
				_hasPendingStates = false;
				_enemySpawnStatesModel.SetAuthorityProvider(null);
				_enemySpawnStatesModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<EnemySpawnStatesNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedSessionTimePassedChanged(int sessionTimePassed)
		{
			ApplySessionTimePassedFromNetwork(sessionTimePassed);
		}

		private void HandleNetworkedStatesChanged(IReadOnlyDictionary<EnemyType, EnemySpawnStateData> states)
		{
			ApplyStatesFromNetwork(states);
		}

		private void ApplySessionTimePassedFromNetwork(int sessionTimePassed)
		{
			_enemySpawnStatesModel.SessionTimePassed.ApplyFromNetwork(sessionTimePassed);
		}

		private void ApplyStatesFromNetwork(IReadOnlyDictionary<EnemyType, EnemySpawnStateData> states)
		{
			_enemySpawnStatesModel.States.ApplyFromNetwork(states);
		}

		private bool WriteSessionTimePassed(int value)
		{
			if (!_enemySpawnStatesModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] EnemySpawnStatesModel.SessionTimePassed was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSessionTimePassed = value;
			_hasPendingSessionTimePassed = true;
			return true;
		}

		private bool WriteStates(IReadOnlyDictionary<EnemyType, EnemySpawnStateData> value)
		{
			if (!_enemySpawnStatesModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] EnemySpawnStatesModel.States was written without state authority; the write was ignored.");
				return false;
			}
			_pendingStates = new Dictionary<EnemyType, EnemySpawnStateData>();
			foreach (KeyValuePair<EnemyType, EnemySpawnStateData> item in value)
			{
				_pendingStates[item.Key] = item.Value;
			}
			_hasPendingStates = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_enemySpawnStatesNetworkObject == null || _enemySpawnStatesNetworkObject.Object == null || _enemySpawnStatesNetworkObject.Runner == null)
			{
				return false;
			}
			if (_enemySpawnStatesNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_enemySpawnStatesNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingSessionTimePassed)
			{
				_hasPendingSessionTimePassed = false;
				_enemySpawnStatesNetworkObject.TryWriteSessionTimePassed(_pendingSessionTimePassed);
			}
			if (_hasPendingStates)
			{
				_hasPendingStates = false;
				_enemySpawnStatesNetworkObject.TryWriteStates(_pendingStates);
			}
		}
	}
}
