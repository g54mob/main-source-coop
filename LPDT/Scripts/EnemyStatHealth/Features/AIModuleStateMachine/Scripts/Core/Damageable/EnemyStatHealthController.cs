using System;
using Features.AIModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using NetworkServices.ObjectsProvider;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyStatHealthController : NetworkBehaviour
	{
		[SerializeField]
		private SimpleEnemyDeadProcessor _deadProcessor;

		[SerializeField]
		private SimpleEnemyDamageable _simpleEnemyDamageable;

		[SerializeField]
		private EntityStatEntityMonoBase _statEntity;

		[Tooltip("When false the enemy stays in the world at 0 HP (no death event, loot or despawn) and only raises OnEnemyDead, so a state machine can drive a downed/stun flow.")]
		[SerializeField]
		private bool _despawnOnDeath = true;

		private bool _isDead;

		private EnemyDeathEvent _enemyDeathEvent;

		public event Action OnEnemyDead;

		[Inject]
		private void InjectDependencies(EnemyDeathEvent enemyDeathEvent)
		{
			_enemyDeathEvent = enemyDeathEvent;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			if (!(_simpleEnemyDamageable == null))
			{
				_simpleEnemyDamageable.OnDamaged += OnDamagedHandler;
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			if (!(_simpleEnemyDamageable == null))
			{
				_simpleEnemyDamageable.OnDamaged -= OnDamagedHandler;
			}
		}

		private void OnDamagedHandler(DamageData damageData)
		{
			if (!base.HasStateAuthority || _isDead)
			{
				return;
			}
			if (_statEntity == null)
			{
				Debug.LogError("EnemyStatHealthController on " + base.name + " has no EntityStatEntityMonoBase assigned.", this);
				return;
			}
			IStat stat = _statEntity.GetStat(EntityStatType.Health);
			if (stat == null)
			{
				Debug.LogError(string.Format("{0} on {1} cannot resolve {2} stat.", "EnemyStatHealthController", base.name, EntityStatType.Health), this);
				return;
			}
			float num = Mathf.Max(0f, stat.Value - damageData.Damage);
			stat.MaxValue = Mathf.Max(stat.MaxValue, num);
			stat.OverrideValue(num);
			if (num <= 0f)
			{
				Dead(damageData.DamageDealerPlayerID);
			}
		}

		private void Dead(int damageDealerPlayerId)
		{
			_isDead = true;
			this.OnEnemyDead?.Invoke();
			if (_despawnOnDeath)
			{
				IEnemyTypeProvider component;
				EnemyType enemyType = (TryGetComponent<IEnemyTypeProvider>(out component) ? component.EnemyType : EnemyType.None);
				_enemyDeathEvent.InvokeEnemyDeadByPlayer(damageDealerPlayerId, enemyType);
				_deadProcessor.ProcessEnemyDeath();
				if (!(base.Runner == null) && !(base.Object == null) && (!TryGetComponent<IDeferredEnemyDespawn>(out var component2) || !component2.TryStartDeferredDespawn()))
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		public void Revive()
		{
			if (base.HasStateAuthority)
			{
				_isDead = false;
				if (!(_statEntity == null))
				{
					IStat stat = _statEntity.GetStat(EntityStatType.Health);
					stat?.OverrideValue(stat.MaxValue);
				}
			}
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
