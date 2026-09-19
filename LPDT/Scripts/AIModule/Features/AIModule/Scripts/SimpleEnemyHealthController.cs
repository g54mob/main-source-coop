using System;
using Cysharp.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.DamageableTrackModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class SimpleEnemyHealthController : NetworkBehaviour
	{
		[SerializeField]
		private SimpleEnemyDamageable _simpleEnemyDamageable;

		[SerializeField]
		private NetworkObject _deathParticle;

		[SerializeField]
		private Transform _overrideSpawnPoint;

		[SerializeField]
		private LevelObjectData _levelObjectData;

		[SerializeField]
		private bool _processDamageInternally = true;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsNeedToSpawnItem", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsNeedToSpawnItem = true;

		private bool _isDead;

		private IItemSpawnService _itemSpawnService;

		private EnemyDeathEvent _enemyDeathEvent;

		[field: SerializeField]
		public float MaxHealth { get; private set; }

		public float CurrentHealth { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsNeedToSpawnItem
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimpleEnemyHealthController.IsNeedToSpawnItem. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimpleEnemyHealthController.IsNeedToSpawnItem. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public event Action OnEnemyDead;

		[Inject]
		public void InjectDependencies(IItemSpawnService itemSpawnService, EnemyDeathEvent enemyDeathEvent)
		{
			_itemSpawnService = itemSpawnService;
			_enemyDeathEvent = enemyDeathEvent;
		}

		private void Awake()
		{
			CurrentHealth = MaxHealth;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			if (_processDamageInternally)
			{
				_simpleEnemyDamageable.OnDamaged += ReceiveDamage;
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			if (_processDamageInternally)
			{
				_simpleEnemyDamageable.OnDamaged -= ReceiveDamage;
			}
		}

		private void ReceiveDamage(DamageData damageData)
		{
			if (!_processDamageInternally || _isDead)
			{
				return;
			}
			CurrentHealth -= damageData.Damage;
			if (CurrentHealth > 0f)
			{
				return;
			}
			_isDead = true;
			this.OnEnemyDead?.Invoke();
			if (!(base.Object == null) && base.Runner.IsSharedModeMasterClient)
			{
				IEnemyTypeProvider component;
				EnemyType enemyType = (TryGetComponent<IEnemyTypeProvider>(out component) ? component.EnemyType : EnemyType.None);
				_enemyDeathEvent.InvokeEnemyDeadByPlayer(damageData.DamageDealerPlayerID, enemyType);
				if (!TryGetComponent<IDeferredEnemyDespawn>(out var component2) || !component2.TryStartDeferredDespawn())
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (!(base.Runner == null) && !base.Runner.IsShutdown && !(base.Runner.ObjectProvider is IForbiddableObjectProvider { IsAcquireInstanceAllowed: false }) && base.Runner.IsSharedModeMasterClient && _levelObjectData.Item != null && IsNeedToSpawnItem)
			{
				Vector3 vector = ((_overrideSpawnPoint != null) ? _overrideSpawnPoint.position : base.transform.position);
				_itemSpawnService.SpawnItemsMultiple(_levelObjectData.Item, vector, _levelObjectData.ItemCount, 0f, _levelObjectData.UseSpread, null, _levelObjectData.UseSpread).Forget();
				if (!(_deathParticle == null))
				{
					base.Runner.SpawnAsync(_deathParticle, vector, Quaternion.identity, base.Runner.LocalPlayer);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsNeedToSpawnItem = _IsNeedToSpawnItem;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsNeedToSpawnItem = IsNeedToSpawnItem;
		}
	}
}
