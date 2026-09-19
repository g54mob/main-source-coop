using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors
{
	public class CoinRobTargetSensor
	{
		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly CoinRobCombatSettings _combatSettings;

		private readonly CoinRobStealSettings _stealSettings;

		private readonly INavigationService _navigationService;

		private readonly IPlayerStateService _playerStateService;

		private readonly List<IItem> _secondaryStealCandidates = new List<IItem>();

		public CoinRobTargetSensor(CoinRobSwarmEnemyContext context, CoinRobSwarmEnemySettings swarmSettings, CoinRobCombatSettings combatSettings, CoinRobStealSettings stealSettings, INavigationService navigationService, IPlayerStateService playerStateService)
		{
			_context = context;
			_swarmSettings = swarmSettings;
			_combatSettings = combatSettings;
			_stealSettings = stealSettings;
			_navigationService = navigationService;
			_playerStateService = playerStateService;
		}

		public Collider[] OverlapAllAtSwarmCenter()
		{
			return Physics.OverlapSphere(_context.SwarmCenter, _combatSettings.TargetSearchRadius);
		}

		public bool TryFindPlayerNearSwarm(out PlayerRef player)
		{
			Collider[] colliders = Physics.OverlapSphere(_context.SwarmCenter, _combatSettings.TargetSearchRadius, _combatSettings.PlayerLayerMask);
			return TryGetPlayerInColliders(colliders, out player);
		}

		public bool TryGetPlayerInColliders(Collider[] colliders, out PlayerRef player)
		{
			player = PlayerRef.None;
			if (colliders == null)
			{
				return false;
			}
			foreach (Collider collider in colliders)
			{
				if (IsInLayerMask(collider.gameObject.layer, _combatSettings.PlayerLayerMask) && collider.TryGetComponent<NetworkObject>(out var component) && _playerStateService.IsPlayerAlive(component.StateAuthority.PlayerId))
				{
					player = component.StateAuthority;
					return true;
				}
			}
			return false;
		}

		public bool TryMarkStealItems(bool clearExistingTasks = true)
		{
			if (_context.UnitsCount == 0)
			{
				return false;
			}
			if (!clearExistingTasks && _context.ActiveStealTasks.Count > 0)
			{
				return true;
			}
			if (clearExistingTasks)
			{
				_context.ActiveStealTasks.Clear();
				_context.ClearRejectedStealItems();
			}
			TryAssignStealTask(_context.ChaseTargetItem);
			CollectSecondaryStealCandidates(_secondaryStealCandidates);
			for (int i = 0; i < _secondaryStealCandidates.Count; i++)
			{
				if (!HasFreeStealUnit())
				{
					break;
				}
				TryAssignStealTask(_secondaryStealCandidates[i]);
			}
			return _context.ActiveStealTasks.Count > 0;
		}

		public bool TryAssignMoreStealTasks()
		{
			return TryMarkStealItems(clearExistingTasks: false);
		}

		private bool HasFreeStealUnit()
		{
			for (int i = 0; i < _context.UnitsCount; i++)
			{
				CoinRobBehaviour coinRobBehaviour = _context.SwarmUnits[i];
				if (coinRobBehaviour != null && !_context.ActiveStealTasks.ContainsKey(coinRobBehaviour) && !coinRobBehaviour.IsCarryingLoot)
				{
					return true;
				}
			}
			return false;
		}

		private void CollectSecondaryStealCandidates(List<IItem> buffer)
		{
			buffer.Clear();
			AppendUniqueInterestItems(buffer, Physics.OverlapSphere(_context.SwarmCenter, _combatSettings.TargetSearchRadius));
			if (_context.ChaseTargetItem != null && _context.ChaseTargetItem.NetworkObject != null)
			{
				Vector3 position = _context.ChaseTargetItem.NetworkObject.transform.position;
				AppendUniqueInterestItems(buffer, Physics.OverlapSphere(position, _combatSettings.TargetSearchRadius));
			}
		}

		private void AppendUniqueInterestItems(List<IItem> buffer, Collider[] hits)
		{
			if (hits == null)
			{
				return;
			}
			for (int i = 0; i < hits.Length; i++)
			{
				IItem component;
				IItem item = (hits[i].TryGetComponent<IItem>(out component) ? component : hits[i].GetComponentInParent<IItem>());
				if (item != null && item != _context.ChaseTargetItem && IsItemOfInterest(item) && !buffer.Contains(item))
				{
					buffer.Add(item);
				}
			}
		}

		private bool TryAssignStealTask(IItem item)
		{
			if (item == null || item.NetworkObject == null || !item.AvailableForEnemy || !IsItemOfInterest(item) || IsItemInCart(item) || _context.IsStealItemRejected(item))
			{
				return false;
			}
			if (IsItemAlreadyAssigned(item))
			{
				return false;
			}
			Vector3 position = item.NetworkObject.transform.position;
			if (!TryGetStealDestination(position, out var stealDestination))
			{
				return false;
			}
			CoinRobBehaviour coinRobBehaviour = FindFreeUnitForSteal(item, position);
			if (coinRobBehaviour == null)
			{
				return false;
			}
			coinRobBehaviour.SetDestination(stealDestination);
			_context.ActiveStealTasks[coinRobBehaviour] = new CoinRobSwarmEnemyContext.ItemStealTask
			{
				TargetItem = item,
				TargetPosition = position,
				TotalTimer = 0f
			};
			return true;
		}

		private bool IsItemAlreadyAssigned(IItem item)
		{
			foreach (CoinRobSwarmEnemyContext.ItemStealTask value in _context.ActiveStealTasks.Values)
			{
				if (value.TargetItem == item)
				{
					return true;
				}
			}
			return false;
		}

		private CoinRobBehaviour FindFreeUnitForSteal(IItem item, Vector3 itemPosition)
		{
			CoinRobBehaviour result = null;
			float num = float.MaxValue;
			for (int i = 0; i < _context.UnitsCount; i++)
			{
				CoinRobBehaviour coinRobBehaviour = _context.SwarmUnits[i];
				if (!(coinRobBehaviour == null) && !coinRobBehaviour.IsCarryingLoot && !_context.ActiveStealTasks.ContainsKey(coinRobBehaviour) && !IsItemTooHighToSteal(item, coinRobBehaviour.transform.position))
				{
					float sqrMagnitude = (coinRobBehaviour.transform.position - itemPosition).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						result = coinRobBehaviour;
					}
				}
			}
			return result;
		}

		private bool TryGetStealDestination(Vector3 itemPosition, out Vector3 stealDestination)
		{
			if (!_navigationService.IsPointOnNavMeshProjected(itemPosition, out var _))
			{
				stealDestination = default(Vector3);
				return false;
			}
			stealDestination = _navigationService.ValidatePointOnNavmesh(itemPosition, _swarmSettings.SwarmRadius);
			return true;
		}

		private bool IsItemInCart(IItem item)
		{
			if (item == null || item.NetworkObject == null)
			{
				return false;
			}
			if (!item.NetworkObject.TryGetComponent<IPointGrabable>(out var component))
			{
				return false;
			}
			if (!component.InCart)
			{
				if (component.Carts != null)
				{
					return component.Carts.Count > 0;
				}
				return false;
			}
			return true;
		}

		public bool IsItemTooHighToSteal(IItem item, Vector3 unitPosition)
		{
			if (item == null || item.NetworkObject == null)
			{
				return false;
			}
			if (IsItemHeldByPlayer(item))
			{
				return false;
			}
			return item.NetworkObject.transform.position.y - unitPosition.y > _stealSettings.MaxStealHeightAboveUnit;
		}

		private bool IsItemHeldByPlayer(IItem item)
		{
			if (item.NetworkObject.TryGetComponent<IPointGrabable>(out var component))
			{
				return component.GrabbedByPlayers.Count > 0;
			}
			return false;
		}

		public bool IsTargetItemInRange(Vector3 position, IItem targetItem)
		{
			Collider[] array = Physics.OverlapSphere(position, _stealSettings.ItemCheckRadius);
			foreach (Collider collider in array)
			{
				IItem component;
				IItem item = (collider.TryGetComponent<IItem>(out component) ? component : collider.GetComponentInParent<IItem>());
				if (item != null && item == targetItem && IsItemOfInterest(item))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsItemOfInterest(IItem item)
		{
			if (item == null || item.NetworkObject == null)
			{
				return false;
			}
			if (item.Type == ItemType.Coin)
			{
				return true;
			}
			if (!item.NetworkObject.TryGetComponent<LevelObjectMarker>(out var component))
			{
				return false;
			}
			return _swarmSettings.IsItemOfInterest(component.Type);
		}

		private bool IsInLayerMask(int layer, LayerMask layerMask)
		{
			return (layerMask.value & (1 << layer)) != 0;
		}
	}
}
