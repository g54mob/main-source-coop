using System;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.AliveEnemyCount;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[NetworkBehaviourWeaved(2)]
	public class RatsHoleThrowingSystem : NetworkBehaviour
	{
		[SerializeField]
		private RatsHole _ratsHole;

		[SerializeField]
		private RatsHoleEjectSettings _ejectSettings;

		[SerializeField]
		private RatsHoleEnemySpawnSettings _enemySpawnSettings;

		[SerializeField]
		private Transform _enemySpawnPoint;

		[SerializeField]
		private SimplePointGrabable _grabable;

		private AliveEnemyCountModel _aliveEnemyCountModel;

		private bool _wasGrabbed;

		private float _ejectCooldownTimer;

		[WeaverGenerated]
		[DefaultForProperty("GuaranteedEnemySpawnTimer", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private TickTimer _GuaranteedEnemySpawnTimer;

		[WeaverGenerated]
		[DefaultForProperty("EmptyFlowRemainingItemDrops", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _EmptyFlowRemainingItemDrops;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe TickTimer GuaranteedEnemySpawnTimer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleThrowingSystem.GuaranteedEnemySpawnTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(TickTimer*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleThrowingSystem.GuaranteedEnemySpawnTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(TickTimer*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int EmptyFlowRemainingItemDrops
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleThrowingSystem.EmptyFlowRemainingItemDrops. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleThrowingSystem.EmptyFlowRemainingItemDrops. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Inject]
		public void InjectDependencies(AliveEnemyCountModel aliveEnemyCountModel)
		{
			_aliveEnemyCountModel = aliveEnemyCountModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_wasGrabbed = false;
			_ejectCooldownTimer = 0f;
			_grabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
			_grabable.LocalOnGrab += OnLocalGrab;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_grabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
			_grabable.LocalOnGrab -= OnLocalGrab;
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (_ejectCooldownTimer > 0f)
			{
				_ejectCooldownTimer -= base.Runner.DeltaTime;
			}
		}

		private void OnLocalGrab(int _)
		{
			HandleGrabRisingEdge();
		}

		private void OnGrabbedPlayersChanged()
		{
			if (_grabable.GrabbedByPlayersCount > 0)
			{
				HandleGrabRisingEdge();
			}
			else
			{
				_wasGrabbed = false;
			}
		}

		private void HandleGrabRisingEdge()
		{
			if (base.HasStateAuthority && !_wasGrabbed)
			{
				_wasGrabbed = true;
				TryHandleGrab();
			}
		}

		private void TryHandleGrab()
		{
			if (!(_ejectCooldownTimer > 0f))
			{
				if (_ratsHole.ContentCount <= 0)
				{
					HandleEmptyFlowGrab();
				}
				else
				{
					HandleDefaultFlowGrab();
				}
				StartGrabCooldown();
			}
		}

		private void HandleDefaultFlowGrab()
		{
			if (!TryStartEnemySpawn(_enemySpawnSettings.SpawnChance))
			{
				EjectStoredItems();
				if (_ratsHole.ContentCount == 0)
				{
					ResetEmptyFlow();
				}
			}
		}

		private void HandleEmptyFlowGrab()
		{
			if (IsGuaranteedEnemySpawnTimerActive())
			{
				TryStartEnemySpawn(1f);
				return;
			}
			DropNextEmptyFlowItem();
			if (EmptyFlowRemainingItemDrops <= 0)
			{
				GuaranteedEnemySpawnTimer = TickTimer.CreateFromSeconds(base.Runner, _enemySpawnSettings.GuaranteedEnemySpawnWindow);
			}
		}

		private void DropNextEmptyFlowItem()
		{
			if (EmptyFlowRemainingItemDrops <= 0)
			{
				EmptyFlowRemainingItemDrops = RollEmptyFlowItemDropCount();
			}
			if (TryAddEmptyFlowItemToContent())
			{
				EjectStoredItems();
				EmptyFlowRemainingItemDrops--;
			}
		}

		private int RollEmptyFlowItemDropCount()
		{
			int emptyFlowItemMinCount = _enemySpawnSettings.EmptyFlowItemMinCount;
			int emptyFlowItemMaxCount = _enemySpawnSettings.EmptyFlowItemMaxCount;
			return UnityEngine.Random.Range(emptyFlowItemMinCount, emptyFlowItemMaxCount + 1);
		}

		private bool IsGuaranteedEnemySpawnTimerActive()
		{
			if (GuaranteedEnemySpawnTimer.IsRunning)
			{
				return !GuaranteedEnemySpawnTimer.Expired(base.Runner);
			}
			return false;
		}

		private void StartGrabCooldown()
		{
			_ejectCooldownTimer = _ejectSettings.GrabCooldown;
		}

		private bool TryAddEmptyFlowItemToContent()
		{
			MonoItem emptyFlowItemPrefab = _enemySpawnSettings.EmptyFlowItemPrefab;
			if (!emptyFlowItemPrefab)
			{
				return false;
			}
			if (!TryBuildContentEntryFromPrefab(emptyFlowItemPrefab, out var entry))
			{
				return false;
			}
			return _ratsHole.TryAddContentEntry(entry);
		}

		private bool TryBuildContentEntryFromPrefab(MonoItem prefab, out RatsHoleContentEntry entry)
		{
			entry = null;
			if (!prefab)
			{
				return false;
			}
			NetworkPrefabId id = base.Runner.Prefabs.GetId(prefab.name);
			if (!id.IsValid)
			{
				return false;
			}
			ItemConfig defaultConfig = prefab.DefaultConfig;
			if (!defaultConfig)
			{
				return false;
			}
			ItemData itemData = new ItemData(defaultConfig);
			entry = new RatsHoleContentEntry(id, LevelObjectType.None, itemData.ItemType, (ushort)itemData.CurrencyValue, (ushort)itemData.MaxCurrencyValue, itemData.IsCollectable);
			return true;
		}

		private void EjectStoredItems()
		{
			int num = Mathf.Min(_ejectSettings.ItemsPerGrab, _ratsHole.ContentCount);
			Vector3 ejectDirection = ResolveEjectDirection();
			for (int i = 0; i < num; i++)
			{
				if (!_ratsHole.TryTakeContentEntry(_ejectSettings.SelectionMode, out var entry))
				{
					break;
				}
				EjectContentEntry(entry, ejectDirection, i);
			}
		}

		private bool TryStartEnemySpawn(float spawnChance)
		{
			if (!_enemySpawnSettings.EnemyPrefab.IsValid)
			{
				return false;
			}
			if (IsAtAliveEnemyCap())
			{
				return false;
			}
			if (spawnChance <= 0f || UnityEngine.Random.value > spawnChance)
			{
				return false;
			}
			return SpawnEnemy();
		}

		private bool SpawnEnemy()
		{
			if (!_enemySpawnSettings.EnemyPrefab.IsValid)
			{
				return false;
			}
			if (IsAtAliveEnemyCap())
			{
				return false;
			}
			Vector3 position = _enemySpawnPoint.position;
			Vector3 homePosition = _ratsHole.transform.position;
			Vector3 homeAbsorbPosition = _ratsHole.AbsorbWorldPosition;
			NetworkObject networkObject;
			try
			{
				networkObject = base.Runner.Spawn(_enemySpawnSettings.EnemyPrefab, position, Quaternion.identity, null, delegate(NetworkRunner networkRunner, NetworkObject obj)
				{
					if (obj.TryGetComponent<RatsHoleEnemyContext>(out var component3))
					{
						component3.SetHomePosition(homePosition, homeAbsorbPosition);
					}
				});
			}
			catch (Exception)
			{
				return false;
			}
			if (networkObject == null)
			{
				return false;
			}
			if (!networkObject.TryGetComponent<IEnemyBehaviour>(out var _))
			{
				if (networkObject.IsValid)
				{
					base.Runner.Despawn(networkObject);
				}
				return false;
			}
			if (networkObject.TryGetComponent<EnemySpawnDissolveEffect>(out var component2))
			{
				component2.StartAppear();
			}
			return true;
		}

		private bool IsAtAliveEnemyCap()
		{
			int num = Mathf.Max(1, _enemySpawnSettings.MaxAliveEnemiesPerHole);
			return _aliveEnemyCountModel.GetCount(EnemyType.RatsHoleEnemy) >= num;
		}

		private void ResetEmptyFlow()
		{
			GuaranteedEnemySpawnTimer = default(TickTimer);
			EmptyFlowRemainingItemDrops = 0;
		}

		private void EjectContentEntry(RatsHoleContentEntry entry, Vector3 ejectDirection, int ejectIndex)
		{
			if (!entry.PrefabId.IsValid)
			{
				return;
			}
			Vector3 value = _ratsHole.AbsorbWorldPosition + ejectDirection * _ejectSettings.SpawnOffset;
			float spawnSpreadRadius = _ejectSettings.SpawnSpreadRadius;
			if (spawnSpreadRadius > 0.0001f && ejectIndex > 0)
			{
				Vector2 vector = UnityEngine.Random.insideUnitCircle * spawnSpreadRadius;
				value += new Vector3(vector.x, 0f, vector.y);
			}
			Quaternion rotation = _ratsHole.AbsorbPoint.rotation;
			ItemData itemData = new ItemData(entry.ItemType, entry.CurrencyValue, entry.MaxCurrencyValue, entry.IsCollectable);
			NetworkObject networkObject;
			try
			{
				networkObject = base.Runner.Spawn(entry.PrefabId, value, rotation, null, delegate(NetworkRunner networkRunner, NetworkObject obj)
				{
					if (obj.TryGetComponent<MonoItem>(out var component))
					{
						component.Initialize(itemData);
					}
					if (obj.TryGetComponent<LevelObjectMarker>(out var component2))
					{
						component2.SetType(entry.LevelObjectType);
					}
				});
			}
			catch (Exception)
			{
				return;
			}
			if (!(networkObject == null))
			{
				ApplyEjectImpulse(networkObject, ejectDirection);
			}
		}

		private void ApplyEjectImpulse(NetworkObject spawnedObject, Vector3 ejectDirection)
		{
			float impulsePerMass = _ejectSettings.ImpulsePerMass;
			if (!(impulsePerMass <= 0f) && spawnedObject.TryGetComponent<Rigidbody>(out var component))
			{
				Vector3 normalized = (ejectDirection + Vector3.up * _ejectSettings.UpwardBias).normalized;
				float num = component.mass * impulsePerMass;
				ForceMode forceMode = _ejectSettings.ForceMode;
				if (spawnedObject.TryGetComponent<MonoItem>(out var component2))
				{
					component2.AddForce(num, normalized, forceMode);
				}
				else
				{
					component.AddForce(normalized * num, forceMode);
				}
			}
		}

		private Vector3 ResolveEjectDirection()
		{
			Vector3 vector = _ejectSettings.LocalEjectDirection;
			if (vector.sqrMagnitude <= 0.0001f)
			{
				vector = Vector3.forward;
			}
			return _ratsHole.AbsorbPoint.TransformDirection(vector.normalized);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			GuaranteedEnemySpawnTimer = _GuaranteedEnemySpawnTimer;
			EmptyFlowRemainingItemDrops = _EmptyFlowRemainingItemDrops;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_GuaranteedEnemySpawnTimer = GuaranteedEnemySpawnTimer;
			_EmptyFlowRemainingItemDrops = EmptyFlowRemainingItemDrops;
		}
	}
}
