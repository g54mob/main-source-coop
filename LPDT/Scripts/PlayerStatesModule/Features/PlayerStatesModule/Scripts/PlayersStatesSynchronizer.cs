using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Fusion;
using GameplayEvents;
using Global.SerializableDictionary;
using UnityEngine;
using Zenject;

namespace Features.PlayerStatesModule.Scripts
{
	[Serializable]
	public class PlayersStatesSynchronizer : JsonSynchronizableBaseWithCustomData<PlayersStatesSynchronizer, PlayerStateSynchronizeData>, ISessionCleanup
	{
		[SerializeField]
		private Global.SerializableDictionary.SerializableDictionary<int, int> _playersState = new Global.SerializableDictionary.SerializableDictionary<int, int>();

		private readonly MultiplayerModel _multiplayerModel;

		[Inject]
		private GameplayEventBus _gameplayEventBus;

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public bool IsStateBlocked { get; internal set; }

		public event Action<PlayerStateData> OnSomePlayerStateChanged;

		public event Action<PlayerStateData> OnSomePlayerStateExit;

		public PlayersStatesSynchronizer(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		protected override void SetNewValues(PlayersStatesSynchronizer model, bool isSynchronizedOnStart)
		{
			foreach (KeyValuePair<int, int> item in model._playersState)
			{
				SetState(item.Key, (PlayerState)item.Value);
			}
		}

		public void SetState(int playerId, PlayerState state)
		{
			if (IsStateBlocked && playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				return;
			}
			if (!_playersState.TryGetValue(playerId, out var value))
			{
				PlayerState oldState = PlayerState.None;
				PublishLifeCycleGameplayEvents(playerId, oldState, state);
				this.OnSomePlayerStateExit?.Invoke(new PlayerStateData(playerId, PlayerState.None));
				_playersState[playerId] = (int)state;
				this.OnSomePlayerStateChanged?.Invoke(new PlayerStateData(playerId, state));
				return;
			}
			PlayerState playerState = (PlayerState)value;
			if (state != playerState)
			{
				PublishLifeCycleGameplayEvents(playerId, playerState, state);
				this.OnSomePlayerStateExit?.Invoke(new PlayerStateData(playerId, playerState));
				_playersState[playerId] = (int)state;
				this.OnSomePlayerStateChanged?.Invoke(new PlayerStateData(playerId, state));
			}
		}

		public void SetStateSynchronized(int playerId, PlayerState state)
		{
			base.Data1 = new PlayerStateSynchronizeData
			{
				PlayerId = playerId,
				State = state
			};
			CustomSynchronize();
		}

		public bool TryGetState(int playerId, out PlayerState state)
		{
			if (_playersState.TryGetValue(playerId, out var value))
			{
				state = (PlayerState)value;
				return true;
			}
			state = PlayerState.None;
			return false;
		}

		public bool IsAllPlayerDead()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			int num = 0;
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				num++;
				if (!TryGetState(activePlayer.PlayerId, out var state))
				{
					return false;
				}
				if (state != PlayerState.Dead)
				{
					return false;
				}
			}
			return num > 0;
		}

		protected override void SetNewCustomValues(PlayerStateSynchronizeData data)
		{
			SetState(data.PlayerId, data.State);
		}

		protected override void PrepareForSynchronize()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null)
			{
				return;
			}
			HashSet<int> hashSet = new HashSet<int>();
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				hashSet.Add(activePlayer.PlayerId);
			}
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, int> item in _playersState)
			{
				if (!hashSet.Contains(item.Key))
				{
					list.Add(item.Key);
				}
			}
			foreach (int item2 in list)
			{
				_playersState.Remove(item2);
			}
		}

		public void Cleanup()
		{
			_playersState.Clear();
			this.OnSomePlayerStateChanged = null;
		}

		private bool IsLocalPlayer(int playerId)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			return networkRunner.LocalPlayer.PlayerId == playerId;
		}

		private void PublishLifeCycleGameplayEvents(int playerId, PlayerState oldState, PlayerState newState)
		{
			if (_gameplayEventBus != null)
			{
				bool isLocal = IsLocalPlayer(playerId);
				if (IsEnteringLethalDown(oldState, newState))
				{
					_gameplayEventBus.Publish(new OnPlayerDiedGameplayEvent(playerId, isLocal));
				}
				else if (newState == PlayerState.Alive && (oldState == PlayerState.Dead || oldState == PlayerState.PreDeadCrouch))
				{
					_gameplayEventBus.Publish(new OnPlayerResurrectedGameplayEvent(playerId, isLocal));
				}
			}
		}

		private static bool IsLethalDownPhase(PlayerState state)
		{
			return state == PlayerState.PreDeadCrouch;
		}

		private static bool IsEnteringLethalDown(PlayerState oldState, PlayerState newState)
		{
			if (IsLethalDownPhase(newState))
			{
				return !IsLethalDownPhase(oldState);
			}
			return false;
		}
	}
}
