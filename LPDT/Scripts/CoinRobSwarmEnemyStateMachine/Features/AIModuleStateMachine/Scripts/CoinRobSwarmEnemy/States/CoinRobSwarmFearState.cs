using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Settings;
using Features.ItemsModule.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmFearState : StateBase<CoinRobSwarmStateId>
	{
		private const float FearHoleTimeoutSeconds = 12f;

		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly FearHoleAbsorbAnimationSettings _fearHoleAbsorbAnimationSettings;

		private readonly RatsHoleRegistry _ratsHoleRegistry;

		private Vector3 _holePosition;

		private Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole _fearHole;

		private float _holeAbsorbRadius;

		private float _elapsed;

		private bool _hasHoleDestination;

		private readonly Dictionary<CoinRobBehaviour, Vector3> _memberFearDestinations = new Dictionary<CoinRobBehaviour, Vector3>();

		private readonly Dictionary<CoinRobBehaviour, float> _absorbVisualStartTimes = new Dictionary<CoinRobBehaviour, float>();

		private readonly HashSet<CoinRobBehaviour> _membersStartedDespawn = new HashSet<CoinRobBehaviour>();

		public CoinRobSwarmFearState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobSwarmFormationService formation, CoinRobChaseSettings chaseSettings, FearHoleAbsorbAnimationSettings fearHoleAbsorbAnimationSettings, RatsHoleRegistry ratsHoleRegistry)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_formation = formation;
			_chaseSettings = chaseSettings;
			_fearHoleAbsorbAnimationSettings = fearHoleAbsorbAnimationSettings;
			_ratsHoleRegistry = ratsHoleRegistry;
		}

		public override void OnEnter()
		{
			_elapsed = 0f;
			_memberFearDestinations.Clear();
			_absorbVisualStartTimes.Clear();
			_membersStartedDespawn.Clear();
			_enemy.SetVisualState(CoinRobSwarmVisualState.Fear);
			_context.DisableReactiveAggro();
			_context.ClearPendingReactivePlayerAttack();
			_context.ActiveStealTasks.Clear();
			_enemy.CancelCoinChase();
			_enemy.CancelPlayerAttackForFear();
			Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole = null;
			if (_context.FearInterruptedStateId == CoinRobSwarmStateId.RunAway)
			{
				_formation.TryGetRunAwayDepositHole(out hole);
			}
			_hasHoleDestination = _formation.TrySetFearHoleDestination(out _fearHole, out _holePosition, out _holeAbsorbRadius, hole, _memberFearDestinations);
			if (!_hasHoleDestination)
			{
				CompleteFear();
			}
		}

		public override void OnExit()
		{
			_hasHoleDestination = false;
			_fearHole = null;
			_memberFearDestinations.Clear();
			_absorbVisualStartTimes.Clear();
			_membersStartedDespawn.Clear();
		}

		public override void OnLogic()
		{
			if (_hasHoleDestination)
			{
				_elapsed += _enemy.GetTickDelta();
				AbsorbCarriedItemsAtFearHole(force: false);
				bool flag = _elapsed >= 12f;
				StartReachedMembersDespawn(flag);
				if (flag || AreAllMembersStartedDespawn())
				{
					AbsorbCarriedItemsAtFearHole(force: true);
					StartReachedMembersDespawn(force: true);
					CompleteFear();
				}
			}
		}

		private bool IsMemberNearHole(CoinRobBehaviour member, float reachDistance)
		{
			Vector3 memberFearDestination = GetMemberFearDestination(member);
			float num = reachDistance + member.StoppingDistance;
			if (Vector3.Distance(member.transform.position, memberFearDestination) <= num)
			{
				return true;
			}
			if (member.PathPending)
			{
				return false;
			}
			if (member.HasPath && member.PathStatus == NavMeshPathStatus.PathComplete)
			{
				return member.RemainingDistance <= num;
			}
			return false;
		}

		private Vector3 GetMemberFearDestination(CoinRobBehaviour member)
		{
			if (!(member != null) || !_memberFearDestinations.TryGetValue(member, out var value))
			{
				return _holePosition;
			}
			return value;
		}

		private void StartReachedMembersDespawn(bool force)
		{
			TryStartMemberDespawn(_context.SwarmKing, _chaseSettings.FearArrivalDistance, force);
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null))
				{
					float reachDistance = (swarmUnit.HasAttachedItem ? GetCarrierDepositDistance() : (_chaseSettings.FearArrivalDistance + _holeAbsorbRadius));
					TryStartMemberDespawn(swarmUnit, reachDistance, force);
				}
			}
		}

		private void TryStartMemberDespawn(CoinRobBehaviour member, float reachDistance, bool force)
		{
			if (!(member == null) && !IsMemberDespawnStarted(member) && (force || IsMemberNearHole(member, reachDistance)))
			{
				AbsorbCarriedItemAtFearHole(member, force: true);
				member.StopMovement();
				TryStartAbsorbVisual(member);
				_membersStartedDespawn.Add(member);
				if (member.TryGetComponent<EnemyDeathDissolveEffect>(out var component))
				{
					component.TryStartDeferredDespawn(EnemyDissolveReason.Despawn);
				}
			}
		}

		private void TryStartAbsorbVisual(CoinRobBehaviour member)
		{
			if (!(member == null) && !_absorbVisualStartTimes.ContainsKey(member))
			{
				_absorbVisualStartTimes[member] = _elapsed;
				if (_fearHole != null)
				{
					member.PlayFearHoleAbsorbVisualToTargetRpc(_fearHoleAbsorbAnimationSettings.ScaleDuration, _fearHoleAbsorbAnimationSettings.EndScale, (int)_fearHoleAbsorbAnimationSettings.ScaleEase, _fearHole.AbsorbWorldPosition);
				}
				else
				{
					member.PlayFearHoleAbsorbVisualRpc(_fearHoleAbsorbAnimationSettings.ScaleDuration, _fearHoleAbsorbAnimationSettings.EndScale, (int)_fearHoleAbsorbAnimationSettings.ScaleEase);
				}
			}
		}

		private bool AreAllMembersStartedDespawn()
		{
			bool result = false;
			if (_context.SwarmKing != null)
			{
				result = true;
				if (!IsMemberDespawnStarted(_context.SwarmKing))
				{
					return false;
				}
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null))
				{
					result = true;
					if (!IsMemberDespawnStarted(swarmUnit))
					{
						return false;
					}
				}
			}
			return result;
		}

		private bool IsMemberDespawnStarted(CoinRobBehaviour member)
		{
			if (!(member == null))
			{
				return _membersStartedDespawn.Contains(member);
			}
			return true;
		}

		private void AbsorbCarriedItemsAtFearHole(bool force)
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit != null)
				{
					AbsorbCarriedItemAtFearHole(swarmUnit, force);
				}
			}
		}

		private void AbsorbCarriedItemAtFearHole(CoinRobBehaviour swarmUnit, bool force)
		{
			if (swarmUnit == null || !swarmUnit.HasAttachedItem || (!force && !IsCarrierAtFearDeposit(swarmUnit)))
			{
				return;
			}
			IItem attachedItem = swarmUnit.AttachedItem;
			swarmUnit.ReleaseAttachedItem();
			if (attachedItem != null && !attachedItem.IsDespawned)
			{
				Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole nearestHole = _fearHole;
				if (((!(nearestHole == null) && nearestHole.IsReady) || _ratsHoleRegistry.TryGetNearest(swarmUnit.transform.position, out nearestHole)) && nearestHole != null && nearestHole.IsReady)
				{
					nearestHole.TryAbsorbItem(attachedItem);
				}
			}
		}

		private bool IsCarrierAtFearDeposit(CoinRobBehaviour carrier)
		{
			if (carrier != null)
			{
				return IsMemberNearHole(carrier, GetCarrierDepositDistance());
			}
			return false;
		}

		private float GetCarrierDepositDistance()
		{
			return Mathf.Max(0f, (_fearHole != null) ? _fearHole.AbsorbTriggerDistance : _holeAbsorbRadius);
		}

		private void CompleteFear()
		{
			_enemy.DespawnSwarmWithDissolve();
		}
	}
}
