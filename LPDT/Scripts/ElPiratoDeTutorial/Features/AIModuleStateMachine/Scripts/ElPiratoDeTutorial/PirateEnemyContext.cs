using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using QuickOutline.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	public class PirateEnemyContext : MonoBehaviour
	{
		[field: SerializeField]
		public PirateVisibilityTargetDetector TargetDetector { get; set; }

		[field: SerializeField]
		public EnemyMovableBase EnemyMovableBase { get; set; }

		[field: SerializeField]
		public PirateTutorialConfiguration PirateTutorialConfiguration { get; set; }

		[field: SerializeField]
		public PirateAnimationController PirateAnimationController { get; set; }

		[field: SerializeField]
		public EnemyWeaponBase PirateAttackController { get; set; }

		[field: SerializeField]
		public PirateAttackRotationComponent PirateAttackRotationComponent { get; set; }

		[field: SerializeField]
		public Transform RaycastShootPoint { get; set; }

		[field: SerializeField]
		public PirateAimController PirateAimController { get; set; }

		[field: SerializeField]
		public SimpleEnemyDamageable Damageable { get; set; }

		[field: SerializeField]
		public PirateAudioController PirateAudioController { get; set; }

		[field: SerializeField]
		public bool IsDespawnAfterFear { get; set; }

		[field: SerializeField]
		public Transform TipHolder { get; set; }

		[field: SerializeField]
		public Transform LeftRightMovementTipHolder { get; set; }

		[field: SerializeField]
		public Transform LeftRightWaveTipHolder { get; set; }

		[field: SerializeField]
		public Outline PirateOutline { get; set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase PirateStatEntity { get; set; }

		public IEnemyBehaviour TargetEnemy { get; set; }

		public bool IsFearing { get; set; }

		public bool IsDead { get; set; }

		public void ApplyWalkingSpeed()
		{
			if (!(PirateTutorialConfiguration == null) && !(EnemyMovableBase == null))
			{
				EnemyMovableBase.SetMovementSpeed(PirateTutorialConfiguration.WalkingSpeed);
			}
		}

		public void ApplyChaseSpeed()
		{
			if (!(PirateTutorialConfiguration == null) && !(EnemyMovableBase == null))
			{
				EnemyMovableBase.SetMovementSpeed(PirateTutorialConfiguration.ChaseSpeed);
			}
		}

		public bool IsTargetValid()
		{
			if (TargetEnemy != null && TargetEnemy.NetworkObject != null)
			{
				if (TargetEnemy is IEnemyDeadStateProvider enemyDeadStateProvider)
				{
					return !enemyDeadStateProvider.IsDead;
				}
				return true;
			}
			return false;
		}

		public bool TryGetTargetPosition(out Vector3 position)
		{
			position = Vector3.zero;
			if (!IsTargetValid())
			{
				return false;
			}
			if (!(TargetEnemy is IEnemyTargetProvider enemyTargetProvider))
			{
				position = TargetEnemy.NetworkObject.transform.position;
			}
			else
			{
				position = enemyTargetProvider.HitTarget.position;
			}
			return true;
		}
	}
}
