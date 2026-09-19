using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Damage
{
	[NetworkBehaviourWeaved(0)]
	public abstract class EnemyDamageReactionSystemBase : MonoSystem
	{
		[SerializeField]
		private SimpleEnemyDamageable[] _damageables;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		protected void InjectBaseDependencies(SpawnedPlayersModel spawnedPlayersModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public override void Enable()
		{
			if (!base.HasStateAuthority || _isEnabled)
			{
				return;
			}
			SimpleEnemyDamageable[] damageables = _damageables;
			foreach (SimpleEnemyDamageable simpleEnemyDamageable in damageables)
			{
				if (simpleEnemyDamageable != null)
				{
					simpleEnemyDamageable.OnDamaged += OnDamaged;
				}
			}
			_isEnabled = true;
		}

		public override void Disable()
		{
			if (!_isEnabled)
			{
				return;
			}
			SimpleEnemyDamageable[] damageables = _damageables;
			foreach (SimpleEnemyDamageable simpleEnemyDamageable in damageables)
			{
				if (simpleEnemyDamageable != null)
				{
					simpleEnemyDamageable.OnDamaged -= OnDamaged;
				}
			}
			_isEnabled = false;
		}

		public override void Clear()
		{
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			Disable();
			base.Despawned(runner, hasState);
		}

		protected abstract void React(PlayerDataHolder attacker, DamageData damageData);

		private void OnDamaged(DamageData damageData)
		{
			if (base.HasStateAuthority && TryResolveDamageDealer(damageData.DamageDealerPlayerID, out var player))
			{
				React(player, damageData);
			}
		}

		private bool TryResolveDamageDealer(int playerId, out PlayerDataHolder player)
		{
			player = null;
			if (playerId <= 0)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				if (player2.Key.PlayerId == playerId)
				{
					if (player2.Value?.NetworkObject == null)
					{
						return false;
					}
					player = player2.Value;
					return true;
				}
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
