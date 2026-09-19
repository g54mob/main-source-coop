using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Attack
{
	[NetworkBehaviourWeaved(0)]
	public class TargetAttackSystem : MonoSystem
	{
		private IDetectionContext _detectionContext;

		private IAttackTimingContext _attackTimingContext;

		private IStatContext _statContext;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IDetectionContext detectionContext, IAttackTimingContext attackTimingContext, IStatContext statContext, PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_detectionContext = detectionContext;
			_attackTimingContext = attackTimingContext;
			_statContext = statContext;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				return;
			}
			if (_attackTimingContext.AttackCooldown > 0f)
			{
				_attackTimingContext.AttackCooldown -= base.Runner.DeltaTime;
				return;
			}
			PlayerDataHolder priorityPlayer = _detectionContext.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				return;
			}
			Vector3 position = priorityPlayer.NetworkObject.transform.position;
			if (!(Vector3.Distance(base.transform.position, position) > _attackTimingContext.DistanceToAttack))
			{
				int playerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
				if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value))
				{
					value.Damage(new DamageData
					{
						Damage = _statContext.GetStatValue(EntityStatType.Damage),
						Position = position,
						Direction = (position - base.transform.position).normalized,
						DamageDealerPlayerID = base.Object.StateAuthority.PlayerId,
						Source = DamageDataSourceExtensions.ForEnemyAttack(base.transform, base.gameObject.name, DamageType.Melee)
					});
					_attackTimingContext.AttackCooldown = _attackTimingContext.TimeToAttack;
				}
			}
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
