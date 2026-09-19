using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabItemCarrySystem : MonoSystem
	{
		[SerializeField]
		private EnemyItemHolder _enemyItemHolder;

		[SerializeField]
		private float _itemSearchInterval = 1f;

		[SerializeField]
		private float _itemPickupDistance = 1.5f;

		[SerializeField]
		private List<LevelObjectType> _itemTypesWhitelist;

		private IMovementContext _movementContext;

		private INavigationService _navigationService;

		private SpawnedItemsModel _spawnedItemsModel;

		private CrabEnemyContext _crabContext;

		private bool _isEnabled;

		private float _nextSearchTime;

		private IItem _targetItem;

		private SimplePointGrabable _carriedGrabable;

		private bool _holdTakenByOther;

		public bool IsCarryingItem => _carriedGrabable != null;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IMovementContext movementContext, INavigationService navigationService, SpawnedItemsModel spawnedItemsModel, CrabEnemyContext crabEnemyContext)
		{
			_movementContext = movementContext;
			_navigationService = navigationService;
			_spawnedItemsModel = spawnedItemsModel;
			_crabContext = crabEnemyContext;
		}

		public override void Spawned()
		{
			base.Spawned();
			_enemyItemHolder.OnReleased += OnItemHolderReleased;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_enemyItemHolder.OnReleased -= OnItemHolderReleased;
			base.Despawned(runner, hasState);
		}

		public override void Enable()
		{
			_isEnabled = true;
			_targetItem = null;
			_nextSearchTime = 0f;
		}

		public override void Disable()
		{
			_isEnabled = false;
			_targetItem = null;
		}

		public override void Clear()
		{
			DropCarriedItem();
		}

		public void DropCarriedItem()
		{
			if (!(_carriedGrabable == null))
			{
				_enemyItemHolder.ReleaseGrab();
			}
		}

		public bool ConsumeHoldTakenByOther()
		{
			if (!_holdTakenByOther)
			{
				return false;
			}
			_holdTakenByOther = false;
			return true;
		}

		private void OnItemHolderReleased(GrabReleaseReason reason)
		{
			if (reason == GrabReleaseReason.TakenByOther)
			{
				_holdTakenByOther = true;
			}
			_carriedGrabable = null;
			_crabContext.CarriedItem = null;
		}

		private void Update()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority || IsCarryingItem)
			{
				return;
			}
			if (!IsItemValidTarget(_targetItem, excludeLastGrabbed: false, requireReachable: true, out var navMeshPoint))
			{
				_targetItem = null;
				if (Time.time < _nextSearchTime)
				{
					return;
				}
				_nextSearchTime = Time.time + _itemSearchInterval;
				_targetItem = FindNearestItem(out navMeshPoint);
				if (_targetItem == null)
				{
					return;
				}
			}
			Vector3 position = _targetItem.NetworkObject.transform.position;
			_movementContext.SetTargetPosition(navMeshPoint);
			_movementContext.SetTargetPositionCompleted(isCompleted: false);
			_movementContext.NeedToFindTargetPosition = false;
			if (!(Vector3.Distance(base.transform.position, position) > _itemPickupDistance))
			{
				TryGrabItem(_targetItem);
			}
		}

		private void TryGrabItem(IItem item)
		{
			if (item.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component) && _enemyItemHolder.TryGrab(component))
			{
				_carriedGrabable = component;
				_crabContext.CarriedItem = item;
				_crabContext.LastGrabbedItem = item;
				_targetItem = null;
			}
		}

		private IItem FindNearestItem(out Vector3 navMeshPoint)
		{
			IItem item = FindNearestItem(excludeLastGrabbed: true, out navMeshPoint);
			if (item != null)
			{
				return item;
			}
			return FindNearestItem(excludeLastGrabbed: false, out navMeshPoint);
		}

		private IItem FindNearestItem(bool excludeLastGrabbed, out Vector3 navMeshPoint)
		{
			IItem result = null;
			float num = float.MaxValue;
			navMeshPoint = default(Vector3);
			foreach (IItem item in _spawnedItemsModel.Items)
			{
				if (IsItemValidTarget(item, excludeLastGrabbed, requireReachable: true, out var navMeshPoint2))
				{
					float sqrMagnitude = (item.NetworkObject.transform.position - base.transform.position).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						result = item;
						num = sqrMagnitude;
						navMeshPoint = navMeshPoint2;
					}
				}
			}
			return result;
		}

		private bool IsItemValidTarget(IItem item, bool excludeLastGrabbed, bool requireReachable, out Vector3 navMeshPoint)
		{
			navMeshPoint = default(Vector3);
			if (item == null || item.NetworkObject == null || item.IsDespawned || item.IsConsumed || !item.IsSpawned)
			{
				return false;
			}
			if (excludeLastGrabbed && item == _crabContext.LastGrabbedItem)
			{
				return false;
			}
			if (!item.AvailableForEnemy)
			{
				return false;
			}
			if (!IsItemOfInterest(item))
			{
				return false;
			}
			if (!item.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				return false;
			}
			if (!_enemyItemHolder.CanGrab(component))
			{
				return false;
			}
			if (component.InCart || (component.Carts != null && component.Carts.Count > 0))
			{
				return false;
			}
			if (component.GrabbedByPlayersCount > 0)
			{
				return false;
			}
			if (!_navigationService.TryGetPointOnNavMeshProjected(item.NetworkObject.transform.position, _itemPickupDistance, out navMeshPoint))
			{
				return false;
			}
			if (requireReachable && !IsReachable(navMeshPoint))
			{
				return false;
			}
			return true;
		}

		private bool IsReachable(Vector3 navMeshPoint)
		{
			NavMeshAgent navMeshAgent = _movementContext.NavMeshAgent;
			if (!navMeshAgent.isActiveAndEnabled || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!navMeshAgent.CalculatePath(navMeshPoint, navMeshPath))
			{
				return false;
			}
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}

		private bool IsItemOfInterest(IItem item)
		{
			if (_itemTypesWhitelist == null || _itemTypesWhitelist.Count == 0)
			{
				return true;
			}
			if (!item.NetworkObject.TryGetComponent<LevelObjectMarker>(out var component))
			{
				return false;
			}
			return _itemTypesWhitelist.Contains(component.Type);
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
