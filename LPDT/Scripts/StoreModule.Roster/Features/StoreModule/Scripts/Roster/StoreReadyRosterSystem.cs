using System;
using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.StoreModule.Scripts.Networked;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts.Roster
{
	public sealed class StoreReadyRosterSystem : IStoreReadyRoster, ITickable
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly SessionStateSnapshot _sessionStateSnapshot;

		private readonly List<int> _readyPlayers = new List<int>();

		private readonly HashSet<int> _readySet = new HashSet<int>();

		private readonly HashSet<int> _scratch = new HashSet<int>();

		public IReadOnlyList<int> ReadyPlayers => _readyPlayers;

		public event Action OnPlayerIdsChanged;

		public event Action<int> OnPlayerReady;

		public StoreReadyRosterSystem(MultiplayerModel multiplayerModel, SessionStateSnapshot sessionStateSnapshot)
		{
			_multiplayerModel = multiplayerModel;
			_sessionStateSnapshot = sessionStateSnapshot;
		}

		public bool IsPlayerReady(int playerId)
		{
			return _readySet.Contains(playerId);
		}

		public void Tick()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				if (_readyPlayers.Count > 0)
				{
					ResetRoster();
				}
				return;
			}
			bool flag = _sessionStateSnapshot.LocalState != SessionState.None || _sessionStateSnapshot.GlobalState != SessionState.None;
			if (_sessionStateSnapshot.IsActive && flag && _sessionStateSnapshot.LocalState != SessionState.Shop && _sessionStateSnapshot.GlobalState != SessionState.Shop)
			{
				if (_readyPlayers.Count > 0)
				{
					ResetRoster();
				}
				return;
			}
			_scratch.Clear();
			StoreReadyNetworkObject[] array = UnityEngine.Object.FindObjectsByType<StoreReadyNetworkObject>(FindObjectsSortMode.None);
			foreach (StoreReadyNetworkObject storeReadyNetworkObject in array)
			{
				if (!(storeReadyNetworkObject.Runner != networkRunner) && storeReadyNetworkObject.IsReady)
				{
					_scratch.Add(storeReadyNetworkObject.Object.StateAuthority.PlayerId);
				}
			}
			if (_scratch.SetEquals(_readySet))
			{
				return;
			}
			List<int> list = null;
			foreach (int item in _scratch)
			{
				if (!_readySet.Contains(item))
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(item);
				}
			}
			_readySet.Clear();
			_readySet.UnionWith(_scratch);
			_readyPlayers.Clear();
			_readyPlayers.AddRange(_scratch);
			if (list != null)
			{
				foreach (int item2 in list)
				{
					this.OnPlayerReady?.Invoke(item2);
				}
			}
			this.OnPlayerIdsChanged?.Invoke();
		}

		private void ResetRoster()
		{
			_readySet.Clear();
			_readyPlayers.Clear();
		}
	}
}
