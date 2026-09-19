using System.Collections.Generic;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	public class PlayerResurrectionService : IPlayerResurrectionService
	{
		private readonly IPlayerStateService _playerStateService;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly PlayerResurrectionByPlayerEventClass _playerResurrectionByPlayerEventClass;

		public PlayerResurrectionService(IPlayerStateService playerStateService, PlayerDamageablesTrackModel playerDamageablesTrackModel, PlayerResurrectionByPlayerEventClass playerResurrectionByPlayerEventClass)
		{
			_playerStateService = playerStateService;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_playerResurrectionByPlayerEventClass = playerResurrectionByPlayerEventClass;
		}

		public void ResurrectPlayer(int playerId, bool restoreHp, int resurrectedByPlayerId)
		{
			Debug.Log($"[HealthTrace] Resurrect p{playerId} by p{resurrectedByPlayerId} restoreHp={restoreHp} (ResurrectPlayer)");
			_playerStateService.ChangePlayerState(playerId, PlayerState.Alive);
			_playerResurrectionByPlayerEventClass.InvokePlayerResurrectedByPlayer(playerId, resurrectedByPlayerId);
			if (restoreHp)
			{
				_playerDamageablesTrackModel.AllPlayerDamageables[playerId].HealToFullValue();
			}
		}

		public void ResurrectAllPlayers(bool restoreHp)
		{
			foreach (KeyValuePair<int, IDamageable> allPlayerDamageable in _playerDamageablesTrackModel.AllPlayerDamageables)
			{
				if (_playerStateService.IsPlayerDead(allPlayerDamageable.Key))
				{
					Debug.Log($"[HealthTrace] Resurrect p{allPlayerDamageable.Key} restoreHp={restoreHp} (ResurrectAllPlayers)");
					_playerStateService.ChangePlayerState(allPlayerDamageable.Key, PlayerState.Alive);
					if (restoreHp)
					{
						allPlayerDamageable.Value.HealToFullValue();
					}
				}
			}
		}
	}
}
