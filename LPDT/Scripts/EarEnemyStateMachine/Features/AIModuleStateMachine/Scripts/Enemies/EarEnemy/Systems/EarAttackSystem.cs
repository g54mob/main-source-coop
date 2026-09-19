using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using Features.AnimationModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class EarAttackSystem : MonoSystem
	{
		private const float ITEM_COLLISION_MASS = 3f;

		[SerializeField]
		private DamageableAnimationFunctionReactor _attackReactor;

		[SerializeField]
		private Transform _attackOrigin;

		[SerializeField]
		private Collider _attackCollider;

		private EarEnemyContext _context;

		private EarEnemySettings _settings;

		private IItemCostReduceService _itemCostReduceService;

		private bool _isEnabled;

		private bool _hasDealtDamage;

		private readonly HashSet<GameObject> _processedObjects = new HashSet<GameObject>();

		private readonly HashSet<IDamageable> _hitDamageables = new HashSet<IDamageable>();

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(EarEnemyContext context, EarEnemySettings settings, IItemCostReduceService itemCostReduceService)
		{
			_context = context;
			_settings = settings;
			_itemCostReduceService = itemCostReduceService;
		}

		private void OnValidate()
		{
			if (_attackOrigin == null)
			{
				_attackOrigin = base.transform;
			}
		}

		public override void Enable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = true;
				_hasDealtDamage = false;
				if (_attackReactor != null)
				{
					_attackReactor.TryDealBaseAttackDamage += DealAreaDamage;
				}
			}
		}

		public override void Disable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = false;
				if (_attackReactor != null)
				{
					_attackReactor.TryDealBaseAttackDamage -= DealAreaDamage;
				}
				Clear();
			}
		}

		public override void Clear()
		{
			_hasDealtDamage = false;
		}

		private void DealAreaDamage()
		{
			if (!base.HasStateAuthority || _hasDealtDamage)
			{
				return;
			}
			_hasDealtDamage = true;
			Vector3 position = _attackOrigin.position;
			float attackRadius = _settings.AttackRadius;
			float statValue = _context.GetStatValue(EntityStatType.Damage);
			_processedObjects.Clear();
			_hitDamageables.Clear();
			Collider[] array = Physics.OverlapSphere(position, attackRadius);
			foreach (Collider collider in array)
			{
				if (collider == null || !_processedObjects.Add(collider.gameObject) || collider.transform.IsChildOf(base.Object.transform))
				{
					continue;
				}
				Vector3 b = collider.ClosestPoint(position);
				Vector3 vector = collider.transform.position - base.Object.transform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude < 1E-06f)
				{
					vector = _context.transform.forward;
				}
				vector.Normalize();
				Vector3 normalized = (vector + Vector3.up * _settings.AttackKnockbackUpBias).normalized;
				float num = Mathf.Clamp01(Vector3.Distance(position, b) / Mathf.Max(0.0001f, attackRadius));
				float force = _settings.AttackForce * (1f - num);
				IDamageable componentInParent = collider.GetComponentInParent<IDamageable>();
				MonoItem component;
				if (componentInParent != null)
				{
					if (_hitDamageables.Add(componentInParent))
					{
						componentInParent.DamageRPC(statValue, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForEnemyAttack(_context.transform, EnemyType.Ear.ToString(), DamageType.Melee)));
						componentInParent.AddRPCForce(force, normalized, ForceMode.Impulse);
					}
				}
				else if (FindComponent<MonoItem>(collider.gameObject, out component) && !_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(component, _settings.ItemsDamage, 3f, _attackCollider), base.Object.StateAuthority, isIgnoreLimits: true))
				{
					component.AddForce(force, normalized, ForceMode.Impulse);
				}
			}
		}

		private static bool FindComponent<T>(GameObject source, out T component)
		{
			component = source.GetComponent<T>();
			T val = component;
			if (val == null)
			{
				component = source.GetComponentInParent<T>();
			}
			val = component;
			if (val == null)
			{
				component = source.GetComponentInChildren<T>();
			}
			return component != null;
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
