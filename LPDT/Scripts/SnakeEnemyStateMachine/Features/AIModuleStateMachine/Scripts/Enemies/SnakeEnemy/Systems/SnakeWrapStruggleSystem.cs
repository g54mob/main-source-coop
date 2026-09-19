using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.StruggleBarModule.Scripts;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeWrapStruggleSystem : NetworkBehaviour
	{
		private readonly StatModifier _speedDecreaseModifier = new StatModifier(0f, ModifierType.PercentMulti);

		private SnakeEnemy _enemy;

		private SnakeEnemyContext _context;

		private IStruggleBarService _struggleBarService;

		private StruggleBarCompletedNetworkEvent _struggleBarCompletedNetworkEvent;

		private StruggleBarFailedNetworkEvent _struggleBarFailedNetworkEvent;

		private PlayerMovableModel _playerMovableModel;

		private PlayerGrabSimplePointGrabableModel _playerGrabModel;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private PlayerEnemyInteractionBlocksModel _interactionBlocksModel;

		private bool _isLocalStruggleActive;

		private bool _isJumpLocked;

		private IPointGrabable _blockedVictimGrabable;

		private int _blockedVictimPlayerId;

		private bool _wasStrangling;

		private float _strangleDamageCooldown;

		private int _immobilizedPlayerId;

		private bool _isSpeedEffectApplied;

		[Inject]
		public void InjectDependencies(SnakeEnemy enemy, SnakeEnemyContext context, IStruggleBarService struggleBarService, StruggleBarCompletedNetworkEvent struggleBarCompletedNetworkEvent, StruggleBarFailedNetworkEvent struggleBarFailedNetworkEvent, PlayerMovableModel playerMovableModel, PlayerGrabSimplePointGrabableModel playerGrabModel, PlayerDamageablesTrackModel playerDamageablesTrackModel, SpawnedEntityStatsModel spawnedEntityStatsModel, PlayerEnemyInteractionBlocksModel interactionBlocksModel)
		{
			_enemy = enemy;
			_context = context;
			_struggleBarService = struggleBarService;
			_struggleBarCompletedNetworkEvent = struggleBarCompletedNetworkEvent;
			_struggleBarFailedNetworkEvent = struggleBarFailedNetworkEvent;
			_playerMovableModel = playerMovableModel;
			_playerGrabModel = playerGrabModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_interactionBlocksModel = interactionBlocksModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_struggleBarCompletedNetworkEvent.OnNetworkEventSend += OnStruggleBarCompleted;
			_struggleBarFailedNetworkEvent.OnNetworkEventSend += OnStruggleBarFailed;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_struggleBarCompletedNetworkEvent.OnNetworkEventSend -= OnStruggleBarCompleted;
			_struggleBarFailedNetworkEvent.OnNetworkEventSend -= OnStruggleBarFailed;
			CancelLocalStruggleIfNeeded();
			ClearVictimJumpLock();
			ClearVictimGrabBlock();
			ReleaseImmobilizeIfNeeded();
			ResetStrangleDamageTimer();
			base.Despawned(runner, hasState);
		}

		public void ForceAbortOrphanedWrap()
		{
			if (!base.HasStateAuthority || _context == null)
			{
				return;
			}
			int wrapTargetPlayerId = _context.WrapTargetPlayerId;
			if ((bool)_context.IsWrapOrbitActive || wrapTargetPlayerId != 0)
			{
				if (wrapTargetPlayerId != 0)
				{
					ReleaseImmobilize(wrapTargetPlayerId);
				}
				else
				{
					ReleaseImmobilizeIfNeeded();
				}
				_context.WrapOrbitSystem.End();
				_context.SetPriorityPlayer(null);
				ResetStrangleDamageTimer();
				CancelLocalStruggleIfNeeded();
				ClearVictimJumpLock();
				ClearVictimGrabBlock();
			}
		}

		private void Update()
		{
			if (!(_context == null) && (bool)base.Object && base.Object.IsValid && !(base.Runner == null))
			{
				if (base.HasStateAuthority)
				{
					_context.TickWrapEscapeCooldown(Time.deltaTime);
					TickStrangleDamage(Time.deltaTime);
					UpdateAuthorityImmobilize();
				}
				UpdateLocalStruggle();
				UpdateVictimJumpLock();
				UpdateVictimGrabBlock();
			}
		}

		private void UpdateAuthorityImmobilize()
		{
			if (!_context.IsWrapOrbitActive || _context.WrapTargetPlayerId == 0 || !(_enemy != null) || _enemy.CurrentStateId != SnakeStateId.Wrap)
			{
				ReleaseImmobilizeIfNeeded();
				return;
			}
			int wrapTargetPlayerId = _context.WrapTargetPlayerId;
			if (_immobilizedPlayerId != wrapTargetPlayerId || !_isSpeedEffectApplied)
			{
				if (_immobilizedPlayerId > 0 && _immobilizedPlayerId != wrapTargetPlayerId)
				{
					ReleaseImmobilize(_immobilizedPlayerId);
				}
				ApplyImmobilize(wrapTargetPlayerId);
			}
		}

		private void UpdateLocalStruggle()
		{
			bool flag = (bool)_context.IsWrapOrbitActive && (bool)_context.IsWrapStrangling && _context.WrapTargetPlayerId == base.Runner.LocalPlayer.PlayerId;
			if (flag && !_isLocalStruggleActive)
			{
				_struggleBarService.Start(_context.WrapStruggleDrainPerSecond, _context.WrapStruggleBoostPerPress, _context.WrapStruggleStartNormalized);
				_isLocalStruggleActive = true;
			}
			else if (!flag && _isLocalStruggleActive)
			{
				CancelLocalStruggleIfNeeded();
			}
		}

		private void UpdateVictimJumpLock()
		{
			bool flag = (bool)_context.IsWrapOrbitActive && _context.WrapTargetPlayerId == base.Runner.LocalPlayer.PlayerId;
			if (flag && !_isJumpLocked)
			{
				_playerMovableModel.AddLockMovementReason(LockMovementReasonEnum.SnakeWrap);
				_isJumpLocked = true;
			}
			else if (!flag && _isJumpLocked)
			{
				ClearVictimJumpLock();
			}
		}

		private void UpdateVictimGrabBlock()
		{
			if (!_context.IsWrapOrbitActive || _context.WrapTargetPlayerId == 0)
			{
				ClearVictimGrabBlock();
				return;
			}
			int wrapTargetPlayerId = _context.WrapTargetPlayerId;
			if (_blockedVictimPlayerId != wrapTargetPlayerId)
			{
				ClearVictimGrabBlock();
			}
			if (_playerGrabModel.PlayerGrabables.TryGetValue(wrapTargetPlayerId, out var value))
			{
				value.LocalGrabBlocked = true;
				_blockedVictimGrabable = value;
				_blockedVictimPlayerId = wrapTargetPlayerId;
			}
		}

		private void TickStrangleDamage(float deltaTime)
		{
			if (!_context.IsWrapOrbitActive || !_context.IsWrapStrangling || _context.WrapTargetPlayerId == 0 || !(_enemy != null) || _enemy.CurrentStateId != SnakeStateId.Wrap)
			{
				ResetStrangleDamageTimer();
				return;
			}
			if (!_wasStrangling)
			{
				_wasStrangling = true;
				_strangleDamageCooldown = Mathf.Max(0.01f, _context.WrapStrangleDamageInterval);
			}
			_strangleDamageCooldown -= deltaTime;
			if (!(_strangleDamageCooldown > 0f))
			{
				_strangleDamageCooldown = Mathf.Max(0.01f, _context.WrapStrangleDamageInterval);
				TryDealWrapDamage(_context.WrapTargetPlayerId, _context.WrapStrangleDamage);
			}
		}

		private void OnStruggleBarCompleted(StruggleBarCompletedNetworkEvent networkEvent)
		{
			if (TryAcceptStruggleOutcome(networkEvent.PlayerId))
			{
				FinishWrapAfterStruggle();
			}
		}

		private void OnStruggleBarFailed(StruggleBarFailedNetworkEvent networkEvent)
		{
			if (TryAcceptStruggleOutcome(networkEvent.PlayerId))
			{
				TryDealWrapDamage(networkEvent.PlayerId, _context.WrapFinishDamage);
				FinishWrapAfterStruggle();
			}
		}

		private bool TryAcceptStruggleOutcome(int playerId)
		{
			if (!base.HasStateAuthority || _enemy == null || _context == null)
			{
				return false;
			}
			if (!_context.IsWrapOrbitActive)
			{
				return false;
			}
			if (_enemy.CurrentStateId != SnakeStateId.Wrap)
			{
				return false;
			}
			return playerId == _context.WrapTargetPlayerId;
		}

		private void FinishWrapAfterStruggle()
		{
			_context.BeginWrapEscapeCooldown();
			_context.SetPriorityPlayer(null);
			ResetStrangleDamageTimer();
			_enemy.TriggerEvent(SnakeEvent.OnWrapFinished);
		}

		private void TryDealWrapDamage(int playerId, float damage)
		{
			if (!(damage <= 0f) && playerId != 0 && _playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value) && value != null && value.IsActive)
			{
				value.DamageRPC(damage, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForEnemyAttack(base.transform, EnemyType.Snake.ToString(), DamageType.Melee)));
			}
		}

		private void ApplyImmobilize(int playerId)
		{
			_immobilizedPlayerId = playerId;
			_interactionBlocksModel.SetBlock(playerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
			ApplySpeedEffect(playerId);
		}

		private void ReleaseImmobilizeIfNeeded()
		{
			if (_immobilizedPlayerId > 0)
			{
				ReleaseImmobilize(_immobilizedPlayerId);
			}
		}

		private void ReleaseImmobilize(int playerId)
		{
			DiscardSpeedEffect(playerId);
			_interactionBlocksModel.ClearBlock(playerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
			if (_immobilizedPlayerId == playerId)
			{
				_immobilizedPlayerId = 0;
			}
		}

		private void ApplySpeedEffect(int playerId)
		{
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value) && CanSyncStatModifiers(value))
			{
				value.AddModifierSynchronized(EntityStatType.WalkSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.AddModifierSynchronized(EntityStatType.SprintSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.AddModifierSynchronized(EntityStatType.CrouchSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				_isSpeedEffectApplied = true;
			}
		}

		private void DiscardSpeedEffect(int playerId)
		{
			if (playerId <= 0)
			{
				return;
			}
			_isSpeedEffectApplied = false;
			if (!_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				return;
			}
			if (!CanSyncStatModifiers(value))
			{
				if (!(value == null))
				{
					value.RemoveModifierThatEqual(EntityStatType.WalkSpeed, _speedDecreaseModifier);
					value.RemoveModifierThatEqual(EntityStatType.SprintSpeed, _speedDecreaseModifier);
					value.RemoveModifierThatEqual(EntityStatType.CrouchSpeed, _speedDecreaseModifier);
				}
			}
			else
			{
				value.RemoveModifierSynchronized(EntityStatType.WalkSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.RemoveModifierSynchronized(EntityStatType.SprintSpeed, _speedDecreaseModifier, RPCType.InAllWays);
				value.RemoveModifierSynchronized(EntityStatType.CrouchSpeed, _speedDecreaseModifier, RPCType.InAllWays);
			}
		}

		private static bool CanSyncStatModifiers(EntityStatEntityNetworkedBase targetStatEntity)
		{
			if (targetStatEntity != null && targetStatEntity.Object != null)
			{
				return targetStatEntity.Object.IsValid;
			}
			return false;
		}

		private void CancelLocalStruggleIfNeeded()
		{
			if (_isLocalStruggleActive)
			{
				_struggleBarService.Cancel();
				_isLocalStruggleActive = false;
			}
		}

		private void ClearVictimJumpLock()
		{
			if (_isJumpLocked)
			{
				_playerMovableModel.RemoveLockMovementReason(LockMovementReasonEnum.SnakeWrap);
				_isJumpLocked = false;
			}
		}

		private void ClearVictimGrabBlock()
		{
			if (_blockedVictimGrabable != null)
			{
				bool localGrabBlocked = base.Runner != null && _blockedVictimPlayerId == base.Runner.LocalPlayer.PlayerId;
				_blockedVictimGrabable.LocalGrabBlocked = localGrabBlocked;
			}
			_blockedVictimGrabable = null;
			_blockedVictimPlayerId = 0;
		}

		private void ResetStrangleDamageTimer()
		{
			_wasStrangling = false;
			_strangleDamageCooldown = 0f;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
