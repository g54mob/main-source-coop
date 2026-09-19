using System;
using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Systems;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public class CoinRobSwarmEnemyContext : MonoBehaviour
	{
		public class ItemStealTask
		{
			public IItem TargetItem;

			public Vector3 TargetPosition;

			public bool ReachedTarget;

			public bool GrabStarted;

			public bool GrabAttempted;

			public float AnimationTimer;

			public float TotalTimer;

			public float UnreachableTimer;

			public float StuckRepathTimer;

			public float ClosestApproachDistance = float.MaxValue;

			public bool IsOnKingDetour;
		}

		private readonly List<CoinRobBehaviour> _swarmUnits = new List<CoinRobBehaviour>();

		private readonly Dictionary<CoinRobBehaviour, ItemStealTask> _activeStealTasks = new Dictionary<CoinRobBehaviour, ItemStealTask>();

		private readonly HashSet<IItem> _rejectedStealItems = new HashSet<IItem>();

		private readonly List<CoinRobMemberReactiveAggroSystem> _memberAggroSystems = new List<CoinRobMemberReactiveAggroSystem>();

		private CoinRobSwarmChaseAggroSystem _chaseAggroSystem;

		private bool _reactiveAggroEnabled;

		private Action<int> _reactivePlayerAttackQueued;

		[field: SerializeField]
		public EnemySafeZoneAttackDetector SafeZoneAttackDetector { get; private set; }

		public bool IsReadyForPlayerAttack { get; set; }

		public int PendingPlayerAttackPlayerId { get; set; }

		public int LastMemberDamageDealerPlayerId { get; set; }

		public bool IsProvokedByMemberDamage { get; set; }

		public CoinRobBehaviour SwarmKing { get; set; }

		public EnemyRotator SwarmKingRotator { get; set; }

		public IReadOnlyList<CoinRobBehaviour> SwarmUnits => _swarmUnits;

		public Dictionary<CoinRobBehaviour, ItemStealTask> ActiveStealTasks => _activeStealTasks;

		public Vector3 ChasePosition { get; set; }

		public IItem ChaseTargetItem { get; set; }

		public Vector3 RunAwayPosition { get; set; }

		public Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole RunAwayDepositHole { get; set; }

		public Vector3 RunAwayStartPosition { get; set; }

		public Vector3 AreaPosition { get; private set; }

		public bool HasAreaPosition { get; private set; }

		public float WanderingTimer { get; set; }

		public bool IsPatrolWaiting { get; set; }

		public PlayerRef ChasePlayer { get; set; } = PlayerRef.None;

		public float AttackTimer { get; set; }

		public bool IsAttackPerforming { get; set; }

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; } = true;

		public CoinRobSwarmStateId FearInterruptedStateId { get; set; }

		public CoinRobSwarmStateId CurrentFsmStateId { get; set; }

		public int UnitsCount => _swarmUnits.Count;

		public Vector3 SwarmCenter
		{
			get
			{
				if (_swarmUnits.Count == 0)
				{
					if (!(SwarmKing != null))
					{
						return base.transform.position;
					}
					return SwarmKing.transform.position;
				}
				return _swarmUnits.Aggregate(Vector3.zero, (Vector3 current, CoinRobBehaviour unit) => current + unit.transform.position) / _swarmUnits.Count;
			}
		}

		public bool HasActiveMembers
		{
			get
			{
				if (!(SwarmKing != null))
				{
					return _swarmUnits.Count > 0;
				}
				return true;
			}
		}

		public void BindReactivePlayerAttackQueued(Action<int> handler)
		{
			_reactivePlayerAttackQueued = handler;
		}

		public void QueueReactivePlayerAttack(int playerId)
		{
			if (playerId > 0)
			{
				PendingPlayerAttackPlayerId = playerId;
				IsReadyForPlayerAttack = true;
				_reactivePlayerAttackQueued?.Invoke(playerId);
			}
		}

		public void ClearPendingReactivePlayerAttack()
		{
			IsReadyForPlayerAttack = false;
			PendingPlayerAttackPlayerId = 0;
			IsProvokedByMemberDamage = false;
		}

		public void RejectStealItem(IItem item)
		{
			if (item != null)
			{
				_rejectedStealItems.Add(item);
			}
		}

		public bool IsStealItemRejected(IItem item)
		{
			if (item != null)
			{
				return _rejectedStealItems.Contains(item);
			}
			return false;
		}

		public void ClearRejectedStealItems()
		{
			_rejectedStealItems.Clear();
		}

		public bool HasMembersCarryingLoot()
		{
			foreach (CoinRobBehaviour swarmUnit in _swarmUnits)
			{
				if (swarmUnit != null && swarmUnit.IsCarryingLoot)
				{
					return true;
				}
			}
			return false;
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
			AreaPosition = areaPosition;
			HasAreaPosition = areaPosition.sqrMagnitude > 0.0001f;
		}

		public Vector3 GetPatrolCenter()
		{
			if (!HasAreaPosition)
			{
				if (!(SwarmKing != null))
				{
					return base.transform.position;
				}
				return SwarmKing.transform.position;
			}
			return AreaPosition;
		}

		public bool IsKingElevatedAboveSwarm(float verticalThreshold)
		{
			if (SwarmKing == null || UnitsCount == 0)
			{
				return false;
			}
			return SwarmKing.transform.position.y - SwarmCenter.y >= verticalThreshold;
		}

		public float GetClosestMemberDistanceTo(Vector3 target)
		{
			float num = float.MaxValue;
			if (SwarmKing != null)
			{
				num = Mathf.Min(num, Vector3.Distance(SwarmKing.transform.position, target));
			}
			foreach (CoinRobBehaviour swarmUnit in _swarmUnits)
			{
				num = Mathf.Min(num, Vector3.Distance(swarmUnit.transform.position, target));
			}
			if (num == float.MaxValue)
			{
				num = Vector3.Distance(base.transform.position, target);
			}
			return num;
		}

		public void AddUnit(CoinRobBehaviour unit)
		{
			_swarmUnits.Add(unit);
		}

		public void RemoveUnit(CoinRobBehaviour unit)
		{
			_swarmUnits.Remove(unit);
			_activeStealTasks.Remove(unit);
		}

		public void ClearSwarm()
		{
			_swarmUnits.Clear();
			_activeStealTasks.Clear();
			_rejectedStealItems.Clear();
			SwarmKing = null;
			SwarmKingRotator = null;
			_memberAggroSystems.Clear();
			_chaseAggroSystem = null;
			RunAwayDepositHole = null;
		}

		public void RegisterMemberAggroSystem(CoinRobMemberReactiveAggroSystem system)
		{
			if (!(system == null) && !_memberAggroSystems.Contains(system))
			{
				system.AssignSwarmContext(this);
				_memberAggroSystems.Add(system);
				if (_reactiveAggroEnabled)
				{
					system.Enable();
				}
			}
		}

		public void UnregisterMemberAggroSystem(CoinRobMemberReactiveAggroSystem system)
		{
			if (!(system == null))
			{
				system.Disable();
				_memberAggroSystems.Remove(system);
			}
		}

		public void RegisterChaseAggroSystem(CoinRobSwarmChaseAggroSystem system, CoinRobCombatSettings combatSettings)
		{
			if (!(system == null))
			{
				system.AssignSwarmContext(this, combatSettings);
				_chaseAggroSystem = system;
				if (_reactiveAggroEnabled)
				{
					system.Enable();
				}
			}
		}

		public void UnregisterChaseAggroSystem(CoinRobSwarmChaseAggroSystem system)
		{
			if (!(system == null) && !(_chaseAggroSystem != system))
			{
				system.Disable();
				_chaseAggroSystem = null;
			}
		}

		public void EnableReactiveAggro()
		{
			_reactiveAggroEnabled = true;
			foreach (CoinRobMemberReactiveAggroSystem memberAggroSystem in _memberAggroSystems)
			{
				memberAggroSystem.Enable();
			}
			_chaseAggroSystem?.Enable();
		}

		public void DisableReactiveAggro()
		{
			_reactiveAggroEnabled = false;
			foreach (CoinRobMemberReactiveAggroSystem memberAggroSystem in _memberAggroSystems)
			{
				memberAggroSystem.Disable();
			}
			_chaseAggroSystem?.Disable();
		}
	}
}
