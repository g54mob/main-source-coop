using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using Features.AnimationModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperAttackSystem : MonoSystem
	{
		private const int OVERLAP_BUFFER_SIZE = 64;

		private const float ITEM_COLLISION_MASS = 3f;

		[SerializeField]
		private DamageableAnimationFunctionReactor _attackReactor;

		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		[SerializeField]
		private Transform _attackOrigin;

		[SerializeField]
		private Collider _attackCollider;

		private SleeperEnemyContext _context;

		private SleeperEnemySettings _settings;

		private IItemCostReduceService _itemCostReduceService;

		private PlayerStatesConfiguration _playerStatesConfiguration;

		private bool _isEnabled;

		private bool _hasDealtDamage;

		private readonly Collider[] _overlapBuffer = new Collider[64];

		private readonly HashSet<GameObject> _processedObjects = new HashSet<GameObject>();

		private readonly HashSet<IDamageable> _hitDamageables = new HashSet<IDamageable>();

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(SleeperEnemyContext context, SleeperEnemySettings settings, IItemCostReduceService itemCostReduceService, PlayerStatesConfiguration playerStatesConfiguration)
		{
			_context = context;
			_settings = settings;
			_itemCostReduceService = itemCostReduceService;
			_playerStatesConfiguration = playerStatesConfiguration;
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
				_attackReactor.TryDealBaseAttackDamage += DealAreaDamage;
				_animatorController.PlayAnimation(AnimationType.MeleeAttack);
			}
		}

		public override void Disable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = false;
				_attackReactor.TryDealBaseAttackDamage -= DealAreaDamage;
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
			int num = Physics.OverlapSphereNonAlloc(position, attackRadius, _overlapBuffer, _settings.InvestigateOverlapMask);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _overlapBuffer[i];
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
				float num2 = Mathf.Clamp01(Vector3.Distance(position, b) / Mathf.Max(0.0001f, attackRadius));
				float num3 = _settings.AttackForce * (1f - num2);
				IDamageable componentInParent = collider.GetComponentInParent<IDamageable>();
				MonoItem component;
				if (componentInParent != null)
				{
					if (_hitDamageables.Add(componentInParent))
					{
						if (componentInParent is PlayerDamageable)
						{
							componentInParent.Damage(new DamageData
							{
								Damage = statValue,
								Direction = normalized,
								Force = num3 * _playerStatesConfiguration.StunThrowMultiplier,
								DamageDealerPlayerID = base.Object.StateAuthority.PlayerId,
								ForceMode = ForceMode.Impulse,
								IsStunning = true,
								StunDurationPreset = _settings.AttackStunDurationPreset,
								Source = DamageDataSourceExtensions.ForEnemyAttack(_context.transform, EnemyType.Sleeper.ToString(), DamageType.Melee)
							});
						}
						else
						{
							componentInParent.DamageRPC(statValue, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForEnemyAttack(_context.transform, EnemyType.Sleeper.ToString(), DamageType.Melee)));
							componentInParent.AddRPCForce(num3, normalized, ForceMode.Impulse);
						}
					}
				}
				else if (FindComponent<MonoItem>(collider.gameObject, out component) && !_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(component, _settings.ItemsDamage, 3f, _attackCollider), base.Object.StateAuthority, isIgnoreLimits: true))
				{
					component.AddForce(num3, normalized, ForceMode.Impulse);
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
