using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.LevelGatesModule.Scripts;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;
using UnityEngine;

namespace Features.LevelGatesModule.Data
{
	[Serializable]
	public class PlayersGatesModelSynchronizedModel : DataStreamSynchronizableBaseWithCustomData<PlayersGatesModelSynchronizedModel, GateSynchronizeData>, ISessionCleanup
	{
		[SerializeField]
		private List<PlayerGateState> _players = new List<PlayerGateState>();

		public IReadOnlyList<PlayerGateState> Players => _players;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action OnPlayersChanged;

		public event Action<int, bool> OnPlayerInsideGateChanged;

		protected override void SetNewValues(PlayersGatesModelSynchronizedModel model)
		{
			_players.Clear();
			if (model == null)
			{
				return;
			}
			_players.AddRange(model._players);
			this.OnPlayersChanged?.Invoke();
			foreach (PlayerGateState player in _players)
			{
				this.OnPlayerInsideGateChanged?.Invoke(player.OwnerId, player.PlayerInsideGate);
			}
		}

		protected override void OnSetNewCustomValues(GateSynchronizeData data)
		{
			switch (data.Operation)
			{
			case GateSynchronizeOperation.EnteredGate:
				SetPlayerInsideGateInternal(data.OwnerId, insideGate: true);
				break;
			case GateSynchronizeOperation.ExitedGate:
				SetPlayerInsideGateInternal(data.OwnerId, insideGate: false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.OnPlayersChanged?.Invoke();
		}

		public bool TryGetPlayerState(int ownerId, out PlayerGateState state)
		{
			int num = _players.FindIndex((PlayerGateState p) => p.OwnerId == ownerId);
			if (num < 0)
			{
				state = default(PlayerGateState);
				return false;
			}
			state = _players[num];
			return true;
		}

		public void SetPlayerInsideGate(int ownerId, bool insideGate)
		{
			SetPlayerInsideGateInternal(ownerId, insideGate);
			this.OnPlayersChanged?.Invoke();
			base.Data1 = new GateSynchronizeData
			{
				OwnerId = ownerId,
				IsInsideGate = insideGate,
				Operation = ((!insideGate) ? GateSynchronizeOperation.ExitedGate : GateSynchronizeOperation.EnteredGate)
			};
			CustomSynchronize();
		}

		public void Cleanup()
		{
			_players.Clear();
			this.OnPlayersChanged?.Invoke();
		}

		private void SetPlayerInsideGateInternal(int ownerId, bool insideGate)
		{
			int num = _players.FindIndex((PlayerGateState gateState) => gateState.OwnerId == ownerId);
			if (num < 0)
			{
				_players.Add(new PlayerGateState
				{
					OwnerId = ownerId,
					PlayerInsideGate = insideGate
				});
			}
			else
			{
				PlayerGateState value = _players[num];
				value.PlayerInsideGate = insideGate;
				_players[num] = value;
			}
			this.OnPlayerInsideGateChanged?.Invoke(ownerId, insideGate);
		}
	}
}
