using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.DamageableTrackModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorMeleeAttackSystem : MonoSystem
	{
		private AnchorEnemyContext _context;

		private IDetectionContext _detectionContext;

		private IStatContext _statContext;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private bool _isEnabled;

		private bool _hitApplied;

		public bool HitApplied => _hitApplied;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context, IDetectionContext detectionContext, IStatContext statContext, PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_context = context;
			_detectionContext = detectionContext;
			_statContext = statContext;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_hitApplied = false;
			_context.ClearMeleeHit();
		}

		public override void Disable()
		{
			_isEnabled = false;
			_context.ClearMeleeHit();
		}

		public override void Clear()
		{
			_hitApplied = false;
			_context.ClearMeleeHit();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Initialized && _isEnabled && base.HasStateAuthority && !_hitApplied && _context.MeleeHitRequested)
			{
				_context.ClearMeleeHit();
				_hitApplied = true;
				TryDealHit();
			}
		}

		private void TryDealHit()
		{
			if (_context.TryGetAttackTargetPosition(out var position) && !(Vector3.Distance(base.transform.position, position) > _context.MeleeHitRange) && TryGetTargetDamageable(out var damageable))
			{
				damageable.Damage(new DamageData
				{
					Damage = _statContext.GetStatValue(EntityStatType.Damage),
					Position = position,
					Direction = (position - base.transform.position).normalized,
					DamageDealerPlayerID = base.Object.StateAuthority.PlayerId,
					Source = DamageDataSourceExtensions.ForEnemyAttack(_context.transform, EnemyType.Anchor.ToString(), DamageType.Melee)
				});
			}
		}

		private bool TryGetTargetDamageable(out IDamageable damageable)
		{
			if (!_context.HasLivePriorityPlayer())
			{
				return _context.TryGetAuxTargetDamageable(out damageable);
			}
			int playerId = _detectionContext.PriorityPlayer.NetworkObject.InputAuthority.PlayerId;
			return _playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out damageable);
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
