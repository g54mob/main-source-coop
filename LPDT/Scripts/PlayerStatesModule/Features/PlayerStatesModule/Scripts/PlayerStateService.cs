using System.Collections.Generic;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts
{
	public class PlayerStateService : IPlayerStateService
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private Dictionary<int, Coroutine> _temporalStateRoutines = new Dictionary<int, Coroutine>();

		private Dictionary<int, PlayerState> _enqueuedStates = new Dictionary<int, PlayerState>();

		private bool _isChangingTemporalState;

		public PlayerStateService(PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, ICoroutineRunner coroutineRunner, SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_coroutineRunner = coroutineRunner;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		public void ChangePlayerState(PlayerState newState, bool forced)
		{
			ChangePlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, newState, forced);
		}

		public void ChangePlayerState(int playerId, PlayerState newState, bool forced)
		{
			if (!_isChangingTemporalState)
			{
				ChangePlayerStateUtil(playerId, newState);
			}
			else if (forced)
			{
				if (_temporalStateRoutines.ContainsKey(playerId) && _temporalStateRoutines[playerId] != null)
				{
					_coroutineRunner.StopCoroutine(_temporalStateRoutines[playerId]);
				}
				ChangePlayerStateUtil(playerId, newState);
			}
			else
			{
				_enqueuedStates[playerId] = newState;
			}
		}

		public bool IsPlayerAlive(int playerId)
		{
			if (IsConnectedActivePlayer(playerId) && _playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return state == PlayerState.Alive;
			}
			return false;
		}

		public bool IsPlayerStunned(int playerId)
		{
			if (IsConnectedActivePlayer(playerId) && _playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return state == PlayerState.PreDeadCrouch;
			}
			return false;
		}

		public PlayerState GetPlayerState(int playerId)
		{
			if (_playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return state;
			}
			return PlayerState.None;
		}

		public bool IsLocalPlayerHealthDepleted()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_spawnedEntityStatsModel.PlayerStats.ContainsKey(playerId))
			{
				return false;
			}
			return _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.Health).FullValue <= 0f;
		}

		public void SetStateChangeBlocked(bool isBlocked)
		{
			_playersStatesSynchronizer.IsStateBlocked = isBlocked;
		}

		public bool IsPlayerDead(int playerId)
		{
			if (IsConnectedActivePlayer(playerId) && _playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return state == PlayerState.Dead;
			}
			return false;
		}

		public bool IsPlayerTargetable(int playerId)
		{
			if (!_playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return false;
			}
			if (state != PlayerState.Alive)
			{
				return state == PlayerState.Disconnected;
			}
			return true;
		}

		private bool IsConnectedActivePlayer(int playerId)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				if (activePlayer.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}

		private void ChangePlayerStateUtil(int playerId, PlayerState newState)
		{
			if (!_playersStatesSynchronizer.TryGetState(playerId, out var state) || newState != state)
			{
				_playersStatesSynchronizer.SetStateSynchronized(playerId, newState);
			}
		}
	}
}
